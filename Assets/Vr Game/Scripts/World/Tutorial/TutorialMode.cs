using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMode : GameMode {

    private bool TutorialDone;

    private bool intro;
    private bool explainedSteering;
    private bool explainedAvoiding;
    private bool explainedShooting;
    private bool end;

    private bool avoidingSpawned;
    private bool shootingSpawned;

    private AudioClip audioIntroduction;
    private AudioClip audioExplainSteering;
    private AudioClip audioExplainAvoiding;
    private AudioClip audioExplainShooting;
    private AudioClip audioEnd;

    private AudioClip oNo;

    private GameObject avoidObstacle;
    private GameObject shootObstacle;

    GameObject avoidObstacleSpawned;
    GameObject shootObstacleSpawned;

    float audiotimer;

    float steertimer;
    float avoidtimer;
    float gunSpawntimer;

    int i;

    TutorialShoot shooterTutorial;

    public TutorialMode() {
        avoidObstacle = Resources.Load<GameObject>("tutorial/objects/avoid");
        shootObstacle = Resources.Load<GameObject>("tutorial/objects/targets");

        audioIntroduction = Resources.Load<AudioClip>("tutorial/audio/introduction");
        audioExplainSteering = Resources.Load<AudioClip>("tutorial/audio/explainsteer");
        audioExplainAvoiding = Resources.Load<AudioClip>("tutorial/audio/explainavoid");
        audioExplainShooting = Resources.Load<AudioClip>("tutorial/audio/explainshoot");
        audioEnd = Resources.Load<AudioClip>("tutorial/audio/end");

        oNo = Resources.Load<AudioClip>("ono");
    }

    public override void SetupGame(WorldManager wm) {
        base.SetupGame(wm);

        started = true;
        startTime = 14f;
        endTime = 8.6f;

        tooLongTime = 0;
    }

    public override void OnStart() {
        base.OnStart();

        if (Input.GetKeyDown(KeyCode.P)) {
            EndGame();
        }

        if (!intro) {
            ForceStartSound(audioIntroduction);
            manager.speed = 1;
            intro = true;
        }
    }

    public override void OnPlay() {
        base.OnPlay();

        if (Input.GetKeyDown(KeyCode.P)) {
            EndGame();
        }

        if (!explainedSteering) {
            ForceStartSound(audioExplainSteering);

            audiotimer += Time.deltaTime;
            if (audiotimer >= audioExplainSteering.length - 1) {
                explainedSteering = true;
                audiotimer = 0;
            }
        } 

        if (explainedSteering && !explainedAvoiding) {
            steertimer += Time.deltaTime * Mathf.Abs(Input.GetAxis("Horizontal2"));
        }

        if (steertimer > 2 && !explainedAvoiding) {
            ForceStartSound(audioExplainAvoiding);

            audiotimer += Time.deltaTime;
            if (audiotimer >= audioExplainAvoiding.length - 1) {
                explainedAvoiding = true;
                audiotimer = 0;

                manager.speed = 5;

                avoidObstacleSpawned = GameObject.Instantiate(avoidObstacle, manager.currentWorld.worldObject.transform);
                avoidObstacleSpawned.transform.position = manager.currentWorld.nextSpawnLoc.transform.position;
                avoidObstacleSpawned.transform.eulerAngles = manager.currentWorld.bike.transform.eulerAngles;
            }
        }

        if (explainedAvoiding && !avoidingSpawned) {
            avoidtimer += Time.deltaTime;
            if (avoidtimer > 10) {
                GameObject.Destroy(avoidObstacleSpawned);
                avoidingSpawned = true;
            }
        }

        if (!explainedShooting && avoidingSpawned) {
            ForceStartSound(audioExplainShooting);

            audiotimer += Time.deltaTime;
            if(audiotimer >= 2 && !manager.gunObject.activeSelf) {
                manager.gunObject.SetActive(true);
            }

            if (audiotimer >= audioExplainShooting.length - 1) {
                explainedShooting = true;
                audiotimer = 0;

                shootObstacleSpawned = GameObject.Instantiate(shootObstacle, manager.currentWorld.worldObject.transform);
                shootObstacleSpawned.transform.position = manager.currentWorld.nextSpawnLoc.transform.position;
                shootObstacleSpawned.transform.eulerAngles = manager.currentWorld.bike.transform.eulerAngles;

                shooterTutorial = shootObstacleSpawned.GetComponent<TutorialShoot>();

                shootingSpawned = true;
            }
        }

        if (shootingSpawned) {
            if (Input.GetKeyDown(KeyCode.Space)) {                  //TODO -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
                GameObject.Destroy(shooterTutorial.targets[i]);
                i++;
            }

            if (shooterTutorial.AllTargetsDead()) {
                EndGame();
                manager.controllerObject.GetComponent<SteamVRController>().ForceDrop();
                manager.gunObject.SetActive(false);
            }
        }

        if (TutorialDone) {
            EndGame();
        }
    }

    public override void OnHit(Collider other) {
        if (other.tag.Equals("Enemy") || other.tag.Equals("Rock"))
            StartSound(oNo);
    }

    public override void OnEnd() {
        if (!end) {
            ForceStartSound(audioEnd);
            end = true;
        }
        manager.speed = 10;
        base.OnEnd();
    }
}
