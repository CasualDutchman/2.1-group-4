using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawners : MonoBehaviour {

    public GameObject enemyPreset;

    List<Transform> spawners = new List<Transform>();

    float timer;

	void Start () {
        foreach (Transform child in transform) {
            spawners.Add(child);
        }
	}
	
	void Update () {
        timer += Time.deltaTime;

        if (timer >= 1) {
            timer -= 1;
            GameObject go = Instantiate(enemyPreset);
            go.transform.position = spawners[Random.Range(0, spawners.Count)].position;
        }
	}
}
