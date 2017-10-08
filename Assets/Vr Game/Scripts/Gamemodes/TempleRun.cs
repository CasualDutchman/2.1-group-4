using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleRun : GameMode {

    float timer;

    bool didSpawn;

    GameObject lastSpawned;

    public override void SetupGame(WorldManager wm) {
        base.SetupGame(wm);
        Debug.Log("Temple Run");
    }

    public override void UpdateGame() {
        if (active) {
            timer += Time.deltaTime;
            if (!started && timer >= 2) {
                started = true;
                timer = 0;
            }

            if (started) {
                Debug.Log("I am playing: Temple Run");
                if (!didSpawn) {
                    for (int i = Mathf.FloorToInt(manager.amountOfPanels / 2); i < worldsactivepanels.Count; i++) {
                        SpawnObjects(worldsactivepanels[i]);
                        lastSpawned = worldsactivepanels[i];
                    }
                    didSpawn = true;
                }

                if(lastSpawned != worldsactivepanels[worldsactivepanels.Count - 1]) {
                    Debug.Log("new");
                    SpawnObjects(worldsactivepanels[worldsactivepanels.Count - 1]);
                    lastSpawned = worldsactivepanels[worldsactivepanels.Count - 1];
                }

                if (timer >= 7) {
                    active = false;
                }
            }
        }
    }

    void SpawnObjects(GameObject go) {
        BoxCollider boxcol = go.transform.GetChild(0).GetComponent<BoxCollider>();

        for (int i = 0; i < 10; i++) {
            Vector3 randominbounds = new Vector3(Random.Range(-boxcol.bounds.extents.x, boxcol.bounds.extents.x), 0, Random.Range(-boxcol.bounds.extents.z, boxcol.bounds.extents.z));

            Ray ray = new Ray(boxcol.bounds.center + randominbounds + Vector3.up * 5, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 30, LayerMask.GetMask("Ground"))) {
                GameObject lol = GameObject.Instantiate(world.worldType.GetRock(), go.transform);
                lol.transform.position = hit.point;
                lol.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetTexture("_MainTex", world.texturemap);
            }
        }
    }
}
