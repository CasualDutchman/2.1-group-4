using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Race : GameMode {

    float timer;

    public override void SetupGame(WorldManager wm) {
        base.SetupGame(wm);
        Debug.Log("Race");
    }

    public override void UpdateGame() {
        if (active) {
            timer += Time.deltaTime;
            if (!started && timer >= 2) {
                started = true;
                timer = 0;
            }

            if (started) {
                Debug.Log("I am playing: Race");

                if (timer >= 7) {
                    active = false;
                }
            }
        }
    }
}
