using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinHitter : GameMode {

    bool didSpawn;
    bool intro;

    GameObject penguinPrefab;

    GameObject lastSpawned;

    private AudioClip audioIntroduction;
    private AudioClip audioEndGood;
    private AudioClip audioEndBad;

    private AudioClip oNo;

    private bool goodEnding;

    private int hitCounter;
    private int hitLimit = 5;
    private int score;
    private int scoreMax = 20;

    public PenguinHitter() {
        audioIntroduction = Resources.Load<AudioClip>("penguingame/audio/introduction");
        audioEndGood = Resources.Load<AudioClip>("penguingame/audio/end-good");
        audioEndBad = Resources.Load<AudioClip>("penguingame/audio/end-bad");
        penguinPrefab = Resources.Load<GameObject>("penguingame/Penguin01");

        oNo = Resources.Load<AudioClip>("ono");
    }

    public override void SetupGame(WorldManager wm) {
        base.SetupGame(wm);
        started = true;

        startTime = 10.0f;
        endTime = 10;

        tooLongTime = 1 * 60; 
    }

    public override void OnStart() {
        base.OnStart();

        if (!intro) {
            StartSound(audioIntroduction);
            intro = true;
        } 
    }

    public override void OnPlay() {
        base.OnPlay();

        if (intro && timer >= 2 && !manager.gunObject.activeSelf) {
            manager.gunObject.SetActive(true);
        }

        if (!didSpawn) {
            for (int i = world.activepanels.Count - 3; i < world.activepanels.Count; i++) {
                SpawnMultiplePenguins(world.activepanels[i].transform, 2);
                lastSpawned = world.activepanels[i];
            }
            didSpawn = true;
        }

        if (lastSpawned != world.activepanels[world.activepanels.Count - 1]) {
            SpawnMultiplePenguins(world.activepanels[world.activepanels.Count - 1].transform, 4);
            lastSpawned = world.activepanels[world.activepanels.Count - 1];
        }
    }

    public override void OnEnd() {
        base.OnEnd();
        if (goodEnding) {
            StartSound(audioEndGood);
        }
        else {
            StartSound(audioEndBad);
        }
    }

    public override void OnHit(Collider other) {
        if (other.tag.Equals("Enemy") || other.tag.Equals("Rock")) {
            if (other.tag.Equals("Enemy"))
                GameObject.Destroy(other.gameObject);

            StartSound(oNo);
            hitCounter++;
            if(hitCounter >= hitLimit) {
                goodEnding = false;
                quickPortal = true;
                endTime = 0;
                EndGame();
            }
        }
    }

    public override void Score() {
        score++;
        if(score >= scoreMax) {
            endTime = 0;
            EndGame();
        }
    }


    void SpawnMultiplePenguins(Transform trans, int amount) {
        for (int i = 0; i < amount; i++) {
            SpawnPenguin(trans);
        }
    }

    void SpawnPenguin(Transform trans) {
        GameObject go = GameObject.Instantiate(penguinPrefab, manager.currentWorld.worldObject.transform);
        go.GetComponent<Penguin>().target = manager.currentWorld.bike;
        go.transform.position = trans.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    }
}
