using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleRun : GameMode {

    bool didSpawn;

    GameObject lastSpawned;

    bool onEnd;

    float score;



    public override void SetupGame(WorldManager wm) {
        base.SetupGame(wm);
        Debug.Log("Temple Run");

        started = true;
        startTime = 10;
        endTime = 10;

        tooLongTime = 5;
    }

    public override void OnStart() {
        base.OnStart();
    }

    public override void OnPlay() {
        base.OnPlay();
        if (!didSpawn) {
            for (int i = world.activepanels.Count - 3; i < world.activepanels.Count; i++) {
                SpawnObjects(world.activepanels[i]);
                lastSpawned = world.activepanels[i];
            }
            didSpawn = true;
        }

        if (lastSpawned != world.activepanels[world.activepanels.Count - 1]) {
            SpawnObjects(world.activepanels[world.activepanels.Count - 1]);
            lastSpawned = world.activepanels[world.activepanels.Count - 1];
        }
    }

    public override void OnEnd() {
        base.OnEnd();
    }

    void SpawnObjects(GameObject go) {
        BoxCollider boxcol = go.transform.GetChild(go.transform.childCount - 1).GetComponent<BoxCollider>();
        int random = Random.Range(0, 7);

        Transform parent = go.transform.Find("rocks");

        for (int j = 0; j < 2; j++) {
            for (int i = 0; i < 7; i++) {
                if (i != random) {
                    Vector3 rayorigin = go.transform.TransformPoint(boxcol.center + (new Vector3(-6 + (i * 2), 0, -5 + (j * 15))));
                    Ray ray = new Ray(rayorigin + Vector3.up * 5, Vector3.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 30, LayerMask.GetMask("Ground"))) {
                        GameObject lol = GameObject.Instantiate(world.worldType.GetRock(), parent);
                        lol.transform.position = hit.point;
                        lol.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                        lol.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetTexture("_MainTex", world.texturemap);
                    }
                }
            }
        }
    }
}
