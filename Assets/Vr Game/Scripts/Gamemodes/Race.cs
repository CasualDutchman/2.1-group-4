using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Race : GameMode {

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
        if(travelled < 100) {
            Debug.Log("You lost the race!");
        }
    }

    /*
    public override void UpdateGame() {
        if (active) {
            timer += Time.deltaTime;
            if (!started && timer >= 2) {
                started = true;
                timer = 0;
            }

            if (started) {
                Debug.Log("I am playing: Race");

                travelled += Time.deltaTime * manager.speed;

                if (travelled >= 100) {
                    if (timer < 101) {
                        Debug.Log("You Won the race!");
                        active = false;
                    }
                }
                if(timer >= 101 && travelled < 100) {
                    Debug.Log("You lost the race!");
                    active = false;
                }
            }
        }
    }
    */
}
