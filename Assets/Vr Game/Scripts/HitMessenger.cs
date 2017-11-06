using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMessenger : MonoBehaviour {

    public WorldManager wm;

	void OnTriggerEnter(Collider col) {
        wm.OnHitCallback(col);
    }
}
