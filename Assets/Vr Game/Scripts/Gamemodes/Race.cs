using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Race : GameMode {

    private AudioClip audioIntroduction;
    private AudioClip audioEnd;

    public Race() {
        audioIntroduction = Resources.Load<AudioClip>("relax/audio/intro");
        audioEnd = Resources.Load<AudioClip>("relax/audio/end");
    }

    public override void SetupGame(WorldManager wm) {
        base.SetupGame(wm);
        started = true;

        startTime = 5;
        endTime = 0;

        tooLongTime = 30;
    }

    public override void OnStart() {
        base.OnStart();
        Debug.Log(1);
        StartSound(audioIntroduction);
    }

    public override void OnPlay() {
        base.OnPlay();

    }

    public override void OnEnd() {
        base.OnEnd();
        StartSound(audioEnd);
    }
}
