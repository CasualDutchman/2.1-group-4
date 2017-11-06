using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinHitter : GameMode {

    bool didSpawn;

    GameObject penguinPrefab;

    GameObject lastSpawned;

    public override void SetupGame(WorldManager wm) {
        base.SetupGame(wm);
        Debug.Log("SnowWorld");
        started = true;

        startTime = 4;
        endTime = 10;

        tooLongTime = 5;

        penguinPrefab = Resources.Load<GameObject>("penguingame/Penguin01");
    }

    public override void OnStart() {
        base.OnStart();
    }

    public override void OnPlay() {
        base.OnPlay();

        if (!didSpawn) {
            for (int i = world.activepanels.Count - 3; i < world.activepanels.Count; i++) {
                SpawnPenguin(world.activepanels[i].transform);
                lastSpawned = world.activepanels[i];
            }
            didSpawn = true;
        }

        if (lastSpawned != world.activepanels[world.activepanels.Count - 1]) {
            SpawnPenguin(world.activepanels[world.activepanels.Count - 1].transform);
            lastSpawned = world.activepanels[world.activepanels.Count - 1];
        }
    }

    public override void OnEnd() {
        base.OnEnd();
        
    }

    public override void OnHit(Collider other) {
        Debug.Log(other.name);
    }

    void SpawnPenguin(Transform trans) {
        GameObject go = GameObject.Instantiate(penguinPrefab, manager.currentWorld.worldObject.transform);
        go.GetComponent<Penguin>().target = manager.currentWorld.bike;
        go.transform.position = trans.position;
    }
}
