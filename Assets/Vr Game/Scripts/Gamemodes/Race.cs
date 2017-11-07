using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Race : GameMode {

    private AudioClip audioIntroduction;
    private AudioClip audioEnd;

    private AudioClip music;
    float musicTimer;

    public Race() {
        audioIntroduction = Resources.Load<AudioClip>("relax/audio/intro");
        audioEnd = Resources.Load<AudioClip>("relax/audio/end");

        music = Resources.Load<AudioClip>("Forest");
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
        ForceStartSound(audioIntroduction);
    }

    public override void OnPlay() {
        base.OnPlay();

        if (musicTimer < 2.1f)
            musicTimer += Time.deltaTime;

        if (musicTimer >= 2f) {
            manager.musicSource.clip = music;
            if (!manager.musicSource.isPlaying)
                manager.musicSource.Play();
        }
    }

    public override void OnEnd() {
        base.OnEnd();
        ForceStartSound(audioEnd);
    }
}
