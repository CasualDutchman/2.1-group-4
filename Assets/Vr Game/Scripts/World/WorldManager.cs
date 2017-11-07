using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    //can only add objects in Debug Mode. only for demo
    public Material defaultMaterial, portalMaterial;
    public GameObject portalObject;
    public Light normalLight, portalLight;
    public GameObject gunObject, controllerObject;
    public GameObject currentSkybox, otherSkybox;
    //-------

    //list of all possible worlds and worldtype
    public List<WorldType> worldTypes = new List<WorldType>();
    public List<World> worldList = new List<World>();

    public int worldIndex = 0; //how many world have we been to

    public World currentWorld; //current world || previous and next world for portal
    World nextWorld = null;
    public World prevWorld = null;

    GameMode currentGameMode;

    Transform bike;

    public bool hasPortal;

    List<int> traveledWorlds = new List<int>(); // where have we already been

    //these variables are shown in the inspector, information on hover over
    public int startWorld;
    public float speed = 5;
    public int amountOfPanels = 10;

    public float percentageSpawn = 0.7f; // how many to spawn on panels (0 - 1)

    void Start () {
        SetWorlds();

        currentWorld = new TutorialWorld(this, LayerMask.NameToLayer("Ground"), Vector3.zero, Vector3.zero); // create first world
        normalLight.color = currentWorld.sunColor;

        currentGameMode = new TutorialMode(); //first gamemode
        currentGameMode.SetupGame(this);

        worldIndex++;
    }
	
    //this function adds the needed worldtype to the world. Without this function changes in the worldtype's list will not reflect on the world's worldtype and result in errors
    void SetWorlds() {
        foreach(World w in worldList) {
            w.worldType = worldTypes[w.typeID];
        }
    }

	void Update () {
        currentGameMode.UpdateGame();

        if (!currentGameMode.isActive()) {
            if (!hasPortal) {
                currentWorld.SpawnPortal(currentGameMode.quickPortal);
                hasPortal = true;
            }
        }

        //only update the world if it is available

        if(currentWorld != null && currentWorld.IsOn()){
            currentWorld.UpdateWorld();
        }

        if (nextWorld != null && nextWorld.IsOn()) {
            nextWorld.UpdateWorld();
        }

        if (prevWorld != null && prevWorld.IsOn()) {
            prevWorld.UpdateWorld();
        }
    }

    //function what happens when the player goes through the portal
    public void ChangeWorlds() {
        if (gunObject.activeSelf) {
            controllerObject.GetComponent<SteamVRController>().ForceDrop();
            gunObject.SetActive(false);
        }

        foreach (Transform child in nextWorld.worldObject.transform) {
            child.gameObject.layer = LayerMask.NameToLayer("Ground");
            child.gameObject.SetActive(true);

            MeshRenderer mr = child.GetComponent<MeshRenderer>();
            if (mr != null) {
                mr.material = defaultMaterial;
                mr.material.SetTexture("_MainTex", nextWorld.texturemap);
                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }

            ChangeAll(child, defaultMaterial, LayerMask.NameToLayer("Ground"));
        }

        for (int i = 0; i < currentWorld.worldObject.transform.childCount; i++) {
            Transform child = currentWorld.worldObject.transform.GetChild(i);
            child.gameObject.layer = LayerMask.NameToLayer("Portal");

            MeshRenderer mr = child.GetComponent<MeshRenderer>();
            if (mr != null) {
                mr.material = portalMaterial;
                mr.material.SetTexture("_MainTex", currentWorld.texturemap);
            }

            ChangeAll(child, portalMaterial, LayerMask.NameToLayer("Portal"));

            if (i > currentWorld.portalindex)
                child.gameObject.SetActive(false);
        }

        normalLight.color = nextWorld.sunColor;
        portalLight.color = currentWorld.sunColor;

        currentSkybox.GetComponent<MeshRenderer>().material.color = nextWorld.sunColor;
        otherSkybox.GetComponent<MeshRenderer>().material.color = currentWorld.sunColor;

        currentWorld.portalObject.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.3f);
        currentWorld.portalObject.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);

        prevWorld = currentWorld;
        prevWorld.portalMask = LayerMask.NameToLayer("Portal");
        currentWorld = nextWorld;
        currentWorld.portalMask = LayerMask.NameToLayer("Ground");
        nextWorld = null;

        currentGameMode = (GameMode)System.Activator.CreateInstance(System.Type.GetType(currentWorld.availableGamemodes[Random.Range(0, currentWorld.availableGamemodes.Count)]));
        currentGameMode.SetupGame(this);
    }

    //repeats itself, to fix objects when chaning worlds
    void ChangeAll(Transform parent, Material nextmat, LayerMask mask) {
        foreach (Transform child in parent) {
            if (child.GetComponent<MeshRenderer>()) {
                child.gameObject.layer = mask;

                Texture temp = child.GetComponent<MeshRenderer>().material.GetTexture("_MainTex");
                child.GetComponent<MeshRenderer>().material = nextmat;
                child.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", temp);
            }
            ChangeAll(child, nextmat, mask);
        }
    }

    //create a new worl
    public void SetupNewWorld(Vector3 pos, Vector3 euler, bool isTutorial=false) {
        List<int> templist = new List<int>();
        for (int i = 0; i < worldList.Count; i++) {
            if (!traveledWorlds.Contains(i))
                templist.Add(i);
        }

        if(templist.Count == 0) {
            for (int i = 0; i < worldList.Count; i++) {
                templist.Add(i);
            }
        }

        int nextWorldIndex = templist[Random.Range(0, templist.Count)];
        traveledWorlds.Add(nextWorldIndex);

        nextWorld = worldList[isTutorial ? startWorld : nextWorldIndex].Copy();
        nextWorld.SetupWorld(this, LayerMask.NameToLayer("Portal"), pos, euler);
        portalLight.color = nextWorld.sunColor;
        worldIndex++;
    }

    //give the spawned panels list to the gamemode
    public void SetGameModespanels( World w) {
        currentGameMode.SetPanels(w);
    }

    //when a player dies
    public void PlayerDies() {
        speed = 0;
    }

    //callback to add score to gamemode
    public void AddScore() {
        currentGameMode.Score();
    }

    //callback for hitting the bike
    public void OnHitCallback(Collider other) {
        if(currentGameMode != null) {
            currentGameMode.OnHit(other);
        }
    }

    #region EditorFunctions
    /*
     * All these functions are needed to add, remove and change in the custom editor
     */
    public void AddWorld() {
        worldList.Add(new World());
    }

    public void RemoveWorld(int index) {
        if (worldList.Count == 1)
            return;

        worldList.RemoveAt(index);
    }

    public void AddWorldType() {
        worldTypes.Add(new WorldType());
    }

    public void RemoveWorldType(int index) {
        if (worldTypes.Count == 1)
            return;

        foreach(World world in worldList) {
            if(world.worldType.name == worldTypes[index].name) {
                world.worldType = worldTypes[0];
                world.typeID = 0;
                Debug.LogWarning("You deleted a Worldtype that was used on world: " + world.name);
            }
        }
        worldTypes.RemoveAt(index);
    }
    #endregion
}

[System.Serializable]
public class WorldType {

    public string name;

    //list of all the prefabs that can be spawned
    public List<GameObject> grass;
    public List<GameObject> trees;
    public List<GameObject> bushes;
    public List<GameObject> rocks;
    public List<GameObject> buildings;
    public List<GameObject> groundPanels;
    public List<GameObject> misc;

    //does it have prefabs or not
    public bool hasGrass;
    public bool hasBushes;
    public bool hasTrees;
    public bool hasRocks;
    public bool hasBuildings;

    //toggle to spawn rocks on the spawnpoint of trees
    public bool treeSpawnAsRock;

    public WorldType() {
        name = "New Worldtype";

        grass = new List<GameObject>();
        grass.Add(null);//it needs at least 1 to avoid nullpointer

        bushes = new List<GameObject>();
        bushes.Add(null);
        trees = new List<GameObject>();
        trees.Add(null);
        rocks = new List<GameObject>();
        rocks.Add(null);
        buildings = new List<GameObject>();
        buildings.Add(null);
        groundPanels = new List<GameObject>();
        groundPanels.Add(null);
        misc = new List<GameObject>();
        misc.Add(null);
    }

    public GameObject GetGroundPanel() { // get a random object from the list, applies to all lists, but misc 
        if (groundPanels.Count > 0) {
            return groundPanels[Random.Range(0, groundPanels.Count)];
        }
        return null;
    }

    public GameObject GetGrass() {
        if (hasGrass && grass.Count > 0) {
            return grass[Random.Range(0, grass.Count)];
        }
        return null;
    }

    public GameObject GetBush() {
        if (hasBushes && bushes.Count > 0) {
            return bushes[Random.Range(0, bushes.Count)];
        }
        return null;
    }

    public GameObject GetTree() {
        if (hasTrees && trees.Count > 0) {
            return trees[Random.Range(0, trees.Count)];
        }
        return null;
    }

    public GameObject GetRock() {
        if (hasRocks && rocks.Count > 0) {
            return rocks[Random.Range(0, rocks.Count)];
        }
        return null;
    }

    public GameObject GetBuilding() {
        if (hasBuildings && buildings.Count > 0) {
            return buildings[Random.Range(0, buildings.Count)];
        }
        return null;
    }

    public GameObject GetMisc(int i) {
        if (buildings.Count > 0) {
            return buildings[i];
        }
        return null;
    }
}

[System.Serializable]
public class GameMode {

    protected AudioSource bikeMic; //audiosource for all narrator audio

    protected bool active = true;
    protected bool started = false;//true when gamemode is starting up
    protected bool playing = false;//true when gamemode is playing
    protected bool ending = false;//true when gamemode is ending

    protected float startTime; //second of starting
    protected float endTime; //seconds of ending
    protected float tooLongTime; // seconds when the game ends, even when the goal is not met

    protected WorldManager manager;
    protected World world;

    protected float timer;

    public bool quickPortal = false; // spawn a portal right in front of the player, for quick getaway

    public virtual void SetupGame(WorldManager wm) {
        manager = wm;
        bikeMic = GameObject.FindGameObjectWithTag("bikeMic").GetComponent<AudioSource>();
    }

    /*  expendable system
     *  has a start, play and end
     *  all on timers
     */
    public virtual void UpdateGame() { 
        if (active) {
            timer += Time.deltaTime;

            if (started) {
                OnStart();
                if (timer >= startTime) {
                    started = false;
                    playing = true;
                    timer = 0;
                }
            }

            if (playing) {
                OnPlay();
                if (tooLongTime > 0 && timer >= tooLongTime) {
                    EndGame();
                }
            }

            if (ending) {
                OnEnd();
                if (timer >= endTime) {
                    active = false;
                    timer = 0;
                }
            }
        }
    }

    //call when game is over and new world needs to be spawned
    protected void EndGame() {
        started = false;
        playing = false;
        ending = true;
        timer = 0;
    }

    /// <summary>
    /// This is called every frame the gamemode is starting. Useful for playing audio to indicate it has finished.
    /// </summary>
    public virtual void OnStart() { }

    /// <summary>
    /// This is called every frame the gamemode is playing
    /// </summary>
    public virtual void OnPlay() { }

    /// <summary>
    /// This is called every frame the gamemode is ending. Useful for playing audio to indicate it has finished.
    /// </summary>
    public virtual void OnEnd() { }

    public virtual void Score() { }

    public void SetPanels(World w) {
        world = w;
    }

    //called whenever it hits something
    public virtual void OnHit(Collider other) { }

    public bool isActive() { return active; }

    //start a sound on the audiocource of the bike
    protected void StartSound(AudioClip clip) {
        bikeMic.clip = clip;
        if (!bikeMic.isPlaying) {
            bikeMic.Play();
        }
    }
}
