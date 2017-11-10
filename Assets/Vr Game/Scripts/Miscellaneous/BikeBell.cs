using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeBell : MonoBehaviour {

    public AudioClip ring;
    public WorldManager manager;

    AudioSource source;

    void Start() {
        source = GetComponent<AudioSource>();
        source.clip = ring;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.B)) {
            RingRing();
        }
    }

    public void RingRing() {
        source.Play();
        if (!manager.started)
            manager.StartWholeGame();
    }
}
