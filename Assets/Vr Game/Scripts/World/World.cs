using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class World {

    WorldManager manager;

    public GameObject worldObject;
    public GameObject portalObject;

    //all active panels
    public List<GameObject> activepanels = new List<GameObject>();
    //spawnpoint for new panels
    public GameObject nextSpawnLoc;
    //get the length of all active panels (divided by 2 is roughly the middle)
    float activelength;

    Transform bike;

    public string name;
    public Material skyboxMaterial;
    public Color sunColor;
    public Texture2D texturemap;
    public WorldType worldType;
    public List<string> availableGamemodes = new List<string>();
    public int typeID;

    public LayerMask portalMask;
    public int portalindex;

    float timer, timer2;

    int needsGenerationIndex;
    Vector3 changedPos, changedRot;

    public World() {
        name = "New World";
    }

    public void SetupWorld(WorldManager wm, LayerMask mask, Vector3 pos, Vector3 euler) {
        manager = wm;
        portalMask = mask;
        changedPos = pos;
        changedRot = euler;

        bike = GameObject.Find("bike").transform;

        worldObject = new GameObject(name + manager.worldIndex);
        worldObject.transform.SetParent(manager.transform);

        nextSpawnLoc = new GameObject("next spawn");
        nextSpawnLoc.transform.SetParent(worldObject.transform);

        //set the skybox and sun color to the color specified by the world
        if (portalMask == LayerMask.NameToLayer("Portal")) {
            manager.portalCamera.GetComponent<Skybox>().material = skyboxMaterial;
            manager.otherSkybox.GetComponent<MeshRenderer>().material.SetColor("_Color", sunColor);
        } else {
            manager.normalCamera.GetComponent<Skybox>().material = skyboxMaterial;
            manager.currentSkybox.GetComponent<MeshRenderer>().material.SetColor("_Color", sunColor);
        }
    }

    public void UpdateWorld() {
        //move all planes based on the bike's rotation
        worldObject.transform.Translate(-bike.forward * Time.deltaTime * manager.speed, Space.World);

        if(portalObject != null)
            portalObject.transform.Translate(-bike.forward * Time.deltaTime * manager.speed, Space.World);

        if (needsGenerationIndex < manager.amountOfPanels) {
            SpawnPanel();
            if (portalMask == LayerMask.NameToLayer("Portal") && needsGenerationIndex < Mathf.FloorToInt(manager.amountOfPanels / 2))
                activepanels[needsGenerationIndex].SetActive(false);

            if (portalMask == LayerMask.NameToLayer("Portal") && needsGenerationIndex == Mathf.FloorToInt(manager.amountOfPanels / 2))
                foreach (Transform child in activepanels[needsGenerationIndex].transform) {
                    child.gameObject.SetActive(false);
                }

            if (needsGenerationIndex == Mathf.FloorToInt(manager.amountOfPanels / 2)) {

                Vector3 difeuler = changedRot - nextSpawnLoc.transform.eulerAngles;
                worldObject.transform.eulerAngles += difeuler;

                Vector3 difpos = changedPos - nextSpawnLoc.transform.position;
                worldObject.transform.position += difpos;

                if (changedRot != null) {
                    bike.transform.eulerAngles = changedRot;
                }
            }
            needsGenerationIndex++;
        }

        //destroy last panel and spawn new one
        if (this != manager.prevWorld && needsGenerationIndex >= manager.amountOfPanels) {
            if (Vector3.Distance(bike.position, activepanels[0].transform.position) - 20 >= Vector3.Distance(bike.position, activepanels[manager.amountOfPanels - 1].transform.position) + 20) {
                GameObject temp = activepanels[0];
                activelength -= float.Parse(temp.transform.GetChild(temp.transform.childCount - 1).name);
                activepanels.RemoveAt(0);
                GameObject.Destroy(temp);
                SpawnPanel();

                if (portalObject != null)
                    portalindex--;
            }

            if (portalObject != null && Vector3.Distance(bike.position, portalObject.transform.position) <= 100) {
                timer2 += Time.deltaTime;
                if(timer2 < 4)
                    portalObject.transform.GetChild(0).localScale = new Vector3(Mathf.Clamp(timer2 / 2, 0, 2), 1, 1);
            }
        } else {
            timer += Time.deltaTime;

            if(timer >= 3) {
                portalObject.transform.GetChild(0).localScale = new Vector3(Mathf.Clamp(5 - timer, 0, 2), 1, 1);

                if (timer >= 6) {
                    GameObject.Destroy(portalObject);
                    GameObject.Destroy(worldObject);
                    manager.hasPortal = false;
                    manager.prevWorld = null;
                }
            }
        }

        if(this == manager.currentWorld)
            manager.SetGameModespanels(this);
    }

    public void SpawnPortal() {
        portalObject = GameObject.Instantiate(manager.portalObject, manager.transform);
        portalObject.transform.position = nextSpawnLoc.transform.position;
        portalObject.transform.rotation = nextSpawnLoc.transform.rotation;

        portalindex = manager.amountOfPanels;

        manager.SetupNewWorld(portalObject.transform.position, portalObject.transform.eulerAngles);
    }

    void SpawnPanel() {
        //spawn panel at the end of the previous panel
        GameObject go = GameObject.Instantiate(worldType.GetGroundPanel(), worldObject.transform);
        go.layer = portalMask;
        if (activepanels.Count == 0) {
            go.transform.position = new Vector3(0, 0, 0);
            go.transform.eulerAngles = Vector3.zero;
        } else {
            go.transform.position = nextSpawnLoc.transform.position;
            go.transform.eulerAngles = nextSpawnLoc.transform.eulerAngles;
        }

        //go.transform.localScale = Vector3.one * 3f;

        float thislength = go.transform.Find("next").localPosition.z;
        activelength += thislength;

        //place object at position where next panel needs to be placed
        nextSpawnLoc.transform.position = go.transform.Find("next").position;
        nextSpawnLoc.transform.eulerAngles = go.transform.Find("next").eulerAngles;
        activepanels.Add(go);

        //create meshcollider from the groundplane for raycasting to stay on the floor (SimpleBikeController)
        MeshCollider meshcol = go.AddComponent<MeshCollider>();
        meshcol.sharedMesh = go.transform.Find("mesh").GetComponent<MeshFilter>().sharedMesh;

        //List used to combine meshes
        List<CombineInstance> combine = new List<CombineInstance>();

        //add groundplane to the list for combined meshes
        CombineInstance comb = new CombineInstance();
        comb.mesh = go.transform.Find("mesh").GetComponent<MeshFilter>().sharedMesh;
        comb.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        combine.Add(comb);

        GameObject.Destroy(go.transform.Find("mesh").gameObject);

        //add trees to spawnlocation specified by the prefab groundpanel
        //this is the same for the rocks and buildings, so no comments there
        if (go.transform.Find("trees") != null && (worldType.hasTrees || worldType.treeSpawnAsRock)) {

            List<int> randomList = new List<int>();
            float tempCount = go.transform.Find("trees").childCount;
            int afterPercentage = Mathf.FloorToInt(tempCount * manager.percentageSpawn);

            int i = 0;
            foreach (Transform child in go.transform.Find("trees")) {
                randomList.Add(i);
                i++;
            }

            for(int k = 0; k < tempCount - afterPercentage; k++) {
                randomList.RemoveAt(Random.Range(0, randomList.Count - 1));
            }

            i = 0;
            foreach (Transform child in go.transform.Find("trees")) {
                if (randomList.Contains(i)) {
                    //old system, performed badly because of instatiation
                    
                    //add tree to the world;
                    GameObject childgo = GameObject.Instantiate(worldType.hasTrees ? worldType.GetTree() : worldType.GetRock(), go.transform.Find("trees"));
                    childgo.transform.position = child.position;
                    childgo.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);

                    childgo.transform.GetChild(0).GetComponent<MeshRenderer>().material = portalMask == LayerMask.NameToLayer("Portal") ? manager.portalMaterial : manager.defaultMaterial;
                    childgo.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texturemap);
                    //copy mesh to the combine list
                    //Mesh mesh = childgo.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
                    //if (Random.Range(0, 2) == 1)
                    //    mesh.uv = mesh.uv2;

                    //comb.mesh = mesh;
                    //comb.transform = Matrix4x4.TRS(child.transform.localPosition, childgo.transform.rotation, childgo.transform.localScale);
                    //comb.transform = Matrix4x4.TRS(child.transform.localPosition, Quaternion.Euler(0, Random.Range(0, 360), 0), Vector3.one);
                    //combine.Add(comb);

                    GameObject.Destroy(child.gameObject);

                    //destroy the individual mesh
                    //GameObject.Destroy(childgo);
                    
                    /*
                    GameObject prefabToSpawn = worldType.hasTrees ? worldType.GetTree() : worldType.GetRock();

                    //copy mesh to the combine list
                    Mesh mesh = prefabToSpawn.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;

                    if (Random.Range(0, 2) == 1) //change uvmap for randomness
                        mesh.uv = mesh.uv2;

                    comb.mesh = mesh;
                    comb.transform = Matrix4x4.TRS(child.transform.localPosition, Quaternion.Euler(0, Random.Range(0, 360), 0), Vector3.one);
                    combine.Add(comb);
                    */
                }
                i++;
            }
        }

        if (go.transform.Find("rocks") != null && worldType.hasRocks) {
            foreach (Transform child in go.transform.Find("rocks")) {
                GameObject childgo = GameObject.Instantiate(worldType.GetRock(), child);
                childgo.transform.position = child.position;
                childgo.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);

                Mesh mesh = childgo.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
                if (Random.Range(0, 2) == 1)
                    mesh.uv = mesh.uv2;

                comb.mesh = childgo.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
                comb.transform = Matrix4x4.TRS(child.transform.localPosition, childgo.transform.rotation, childgo.transform.localScale);
                combine.Add(comb);
                GameObject.Destroy(childgo);
            }
        }

        if (go.transform.Find("buildings") != null && worldType.hasBuildings) {
            foreach (Transform child in go.transform.Find("buildings")) {
                GameObject childgo = GameObject.Instantiate(worldType.GetBuilding(), child);
                childgo.transform.position = child.position;
                childgo.transform.eulerAngles = child.eulerAngles;

                Mesh mesh = childgo.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
                if (Random.Range(0, 2) == 1)
                    mesh.uv = mesh.uv2;

                comb.mesh = childgo.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
                comb.transform = Matrix4x4.TRS(child.transform.localPosition, childgo.transform.rotation, childgo.transform.localScale);
                combine.Add(comb);
                GameObject.Destroy(childgo);
            }
        }

        //information for the road boundaries
        Vector3 c = Vector3.one, s = Vector3.one;
        bool hasbounds = false;

        if (go.transform.Find("bounds")) {
            c = go.transform.Find("bounds").GetComponent<BoxCollider>().center;
            s = go.transform.Find("bounds").GetComponent<BoxCollider>().size;
            hasbounds = true;

            GameObject.Destroy(go.transform.Find("bounds").gameObject);
        }

        //delete all childs to get rid of unused memory
        int childcount = go.transform.childCount;
        for (int i = 0; i < childcount; i++) {
            //GameObject.Destroy(go.transform.GetChild(i).gameObject);
        }

        //create object to hold the road boundaries
        if (hasbounds) {
            GameObject checkgo = new GameObject(thislength.ToString());
            checkgo.transform.SetParent(go.transform);
            checkgo.transform.localPosition = Vector3.zero;
            checkgo.transform.localEulerAngles = Vector3.zero;
            BoxCollider box = checkgo.AddComponent<BoxCollider>();
            box.center = c;
            box.size = s;
        }

        //create meshfilter and meshrenderer to hold the combined mesh, this includes all trees, rocks and buildings, if a spawnpoint was present
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();

        mf.mesh = new Mesh();
        mf.mesh.name = "Mesh";
        mf.mesh.CombineMeshes(combine.ToArray());

        mr.material = portalMask == LayerMask.NameToLayer("Portal") ? manager.portalMaterial : manager.defaultMaterial;
        mr.material.SetTexture("_MainTex", texturemap);
    }

    public World Copy() {
        World newWorld = new World();
        newWorld.name = name;
        newWorld.skyboxMaterial = skyboxMaterial;
        newWorld.sunColor = sunColor;
        newWorld.texturemap = texturemap;
        newWorld.worldType = worldType;
        newWorld.availableGamemodes = availableGamemodes;
        newWorld.typeID = typeID;

        return newWorld;
}
}
