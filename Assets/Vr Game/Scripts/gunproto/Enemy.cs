using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
        if (col.tag == "Bullet") {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
