using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleRun : GameMode {

    float timer;

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

                if (timer >= 7) {
                    active = false;
                }
            }
        }
    }
}
