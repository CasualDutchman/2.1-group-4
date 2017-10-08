using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBikeController : MonoBehaviour {

    float rotY;

    public WorldManager manager;

    bool alive = true;
    float timer;

	void Start () {
		
	}
	
	void Update () {
        rotY += Input.GetAxis("Horizontal");
        transform.eulerAngles = new Vector3(0, rotY, 0);

        Ray ray = new Ray(transform.position + transform.up * 4, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 11, LayerMask.GetMask("Ground"))) {
            if (hit.collider != null) {
                transform.position = hit.point;
            }
        }

        if (!alive) {
            timer += Time.deltaTime;
            if (timer >= 2) {
                //manager.PlayerDies();
                print("If this was a game, you would be dead!");
            }
        }else {
            timer = 0;
        }
	}

    void OnTriggerStay(Collider other) {
        alive = true;
    }

    void OnTriggerEnter(Collider other) {
        alive = true;
        if (other.tag == "Portal") {
            manager.ChangeWorlds();
        }
    }

    void OnTriggerExit(Collider other) {
        alive = false;
    }
}
