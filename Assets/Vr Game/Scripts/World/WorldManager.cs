using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    //holds the default material for the combined mesh, only accesable in debugmode
    public Material defaultMaterial, portalMaterial;
    public GameObject portalObject;
    public GameObject normalCamera, portalCamera;
    public Light normalLight, portalLight;

    public GameObject currentSkybox, otherSkybox;

    //list of all possible worlds and worldtype
    public List<WorldType> worldTypes = new List<WorldType>();
    public List<World> worldList = new List<World>();

    public int worldIndex = 0;
    public World currentWorld;
    World nextWorld = null;
    public World prevWorld = null;
    GameMode currentGameMode;

    Transform bike;

    public bool hasPortal;

    List<int> traveledWorlds = new List<int>();

    //these variables are shown in the inspector, information on hover over
    public int startWorld;
    public float speed = 5;
    public int amountOfPanels = 10;

	void Start () {
        SetWorlds();

        currentWorld = worldList[startWorld].Copy();
        traveledWorlds.Add(startWorld);
        currentWorld.SetupWorld(this, 11, Vector3.zero, Vector3.zero);
        normalLight.color = currentWorld.sunColor;

        //this long line will look at the string of the world's possible gamemodes and converts it to a useable compiled piece (e.g. "TempleRun" can be used as the GameMode TempleRun instead of as a string)
        currentGameMode = (GameMode)System.Activator.CreateInstance(System.Type.GetType(currentWorld.availableGamemodes[Random.Range(0, currentWorld.availableGamemodes.Count)]));
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
                currentWorld.SpawnPortal();
                portalCamera.GetComponent<Skybox>().material = nextWorld.skyboxMaterial;
                hasPortal = true;
            }
        }

        if(currentWorld != null && currentWorld.skyboxMaterial != null){
            currentWorld.UpdateWorld();
        }

        if (nextWorld != null && nextWorld.skyboxMaterial != null) {
            nextWorld.UpdateWorld();
        }

        if (prevWorld != null && prevWorld.skyboxMaterial != null) {
            prevWorld.UpdateWorld();
        }
    }

    //function what happens when the player goes through the portal
    public void ChangeWorlds() {
        foreach (Transform child in nextWorld.worldObject.transform) {
            child.gameObject.layer = LayerMask.NameToLayer("Ground");
            child.gameObject.SetActive(true);

            MeshRenderer mr = child.GetComponent<MeshRenderer>();
            if (mr != null) {
                mr.material = defaultMaterial;
                mr.material.SetTexture("_MainTex", nextWorld.texturemap);
            }
        }

        for (int i = 0; i < currentWorld.worldObject.transform.childCount; i++) {
            Transform child = currentWorld.worldObject.transform.GetChild(i);
            child.gameObject.layer = LayerMask.NameToLayer("Portal");

            MeshRenderer mr = child.GetComponent<MeshRenderer>();
            if (mr != null) {
                mr.material = portalMaterial;
                mr.material.SetTexture("_MainTex", currentWorld.texturemap);
            }

            foreach (Transform c in child) {
                if (c.childCount > 0) {
                    c.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Portal");
                    MeshRenderer mrc = c.GetChild(0).GetComponent<MeshRenderer>();
                    if (mrc != null) {
                        mrc.material = portalMaterial;
                        mrc.material.SetTexture("_MainTex", currentWorld.texturemap);
                    }
                }
            }

            if (i > currentWorld.portalindex)
                child.gameObject.SetActive(false);
        }

        normalLight.color = nextWorld.sunColor;
        portalLight.color = currentWorld.sunColor;

        Material temp = normalCamera.GetComponent<Skybox>().material;
        normalCamera.GetComponent<Skybox>().material = portalCamera.GetComponent<Skybox>().material;
        portalCamera.GetComponent<Skybox>().material = temp;

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

    public void SetupNewWorld(Vector3 pos, Vector3 euler) {
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

        nextWorld = worldList[nextWorldIndex].Copy();
        nextWorld.SetupWorld(this, 8, pos, euler);
        portalLight.color = nextWorld.sunColor;
        worldIndex++;
    }

    public void SetGameModespanels( World w) {
        currentGameMode.SetPanels(w);
    }

    //when a player dies
    public void PlayerDies() {
        speed = 0;
    }

    //spawns an example panel for the designer to look how a panel's hierachy NEEDS to be!
    [ContextMenu("Create Example")]
    public void CreateExampleGroundPlane() {
        GameObject parent = new GameObject("[parent, names not in brackets cannot be changed]");
        GameObject mesh = new GameObject("mesh [add mesh of panel]");
        mesh.transform.SetParent(parent.transform);

        GameObject next = new GameObject("next [place where mesh ends]");
        next.transform.SetParent(parent.transform);

        GameObject bounds = new GameObject("bounds [change boxcollider to define path]");
        bounds.transform.SetParent(parent.transform);

        GameObject trees = new GameObject("trees [parent for tree spawns]");
        trees.transform.SetParent(parent.transform);

        GameObject treeexample = new GameObject("[example spawnpoint for tree]");
        treeexample.transform.SetParent(trees.transform);

        GameObject rocks = new GameObject("rocks [parent for rock spawns]");
        rocks.transform.SetParent(parent.transform);

        GameObject rockexample = new GameObject("[example spawnpoint for rock]");
        rockexample.transform.SetParent(rocks.transform);

        GameObject buildings = new GameObject("buildings [parent for building spawns]");
        buildings.transform.SetParent(parent.transform);

        GameObject buildingexample = new GameObject("[example spawnpoint for building]");
        buildingexample.transform.SetParent(buildings.transform);
    }

    #region EditorFunctions
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
    public List<GameObject> trees;
    public List<GameObject> rocks;
    public List<GameObject> buildings;
    public List<GameObject> groundPanels;

    public bool hasTrees;
    public bool hasRocks;
    public bool hasBuildings;

    public bool treeSpawnAsRock;

    public WorldType() {
        name = "New Worldtype";
        trees = new List<GameObject>();
        trees.Add(null);
        rocks = new List<GameObject>();
        rocks.Add(null);
        buildings = new List<GameObject>();
        buildings.Add(null);
        groundPanels = new List<GameObject>();
        groundPanels.Add(null);
    }

    public GameObject GetGroundPanel() {
        if (groundPanels.Count > 0) {
            return groundPanels[Random.Range(0, groundPanels.Count)];
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
}

[System.Serializable]
public class GameMode {

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

    public virtual void SetupGame(WorldManager wm) {
        manager = wm;
    }

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
                if (timer >= tooLongTime) {
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

    protected void EndGame() {
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

    public void SetPanels(World w) {
        world = w;
    }

    public virtual void OnHitTaggedItem(string hittedTag) { }

    public bool isActive() { return active; }
}
