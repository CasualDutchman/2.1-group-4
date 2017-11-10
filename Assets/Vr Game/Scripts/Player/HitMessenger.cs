using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMessenger : MonoBehaviour {

    public WorldManager wm;

    //add a callback for the Gamemode
	void OnTriggerEnter(Collider col) {
        wm.OnHitCallback(col);
    }
}
