using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinHitter : GameMode {

    float travelled;

    public override void SetupGame(WorldManager wm) {
        base.SetupGame(wm);
        Debug.Log("Race");
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

        travelled += Time.deltaTime * manager.speed;

        if (travelled >= 100) {
            if (timer < 101) {
                Debug.Log("You won the race!");
                EndGame();
            }
        }
    }

    public override void OnEnd() {
        base.OnEnd();
        if (travelled < 100) {
            Debug.Log("You lost the race!");
        }
    }

    public override void OnHit(Collider other) {
        Debug.Log(other.name);
    }
}
