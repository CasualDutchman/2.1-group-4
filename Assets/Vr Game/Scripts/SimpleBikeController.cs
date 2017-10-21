using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBikeController : MonoBehaviour {

    float rotY;

    public WorldManager manager;

    public Transform steering;

    bool alive = true;
    float timer;

    //true when portal trigger is hit, false at default
    //this bool prevents a very annoying bug
    bool hitPortal;
    float hitPortalTimer;

	void Start () {
		
	}
	
	void Update () {
        rotY += Input.GetAxis("Horizontal");
        transform.eulerAngles = new Vector3(0, rotY, 0);

        steering.localEulerAngles = new Vector3(0, Input.GetAxis("Horizontal") * 20, 7);

        Ray ray = new Ray(transform.position + transform.up * 4 + (transform.forward * 1.3f), Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 11, LayerMask.GetMask("Ground"))) {
            if (hit.collider != null) {
                transform.GetChild(0).LookAt(hit.point);
            }
        }

        Ray ray2 = new Ray(transform.position + transform.up * 4, Vector3.down);
        RaycastHit hit2;
        if (Physics.Raycast(ray2, out hit2, 11, LayerMask.GetMask("Ground"))) {
            if (hit2.collider != null) {
                transform.position = hit2.point;
            }
        }

        if (Input.GetKey(KeyCode.Space)) {
            manager.speed = 20;
        } else {
            manager.speed = 10;
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

        if (hitPortal) {
            hitPortalTimer += Time.deltaTime;
            if (hitPortalTimer >= 2) {
                hitPortal = false;
                hitPortalTimer = 0;
            }
        }
	}

    void OnTriggerStay(Collider other) {
        alive = true;
    }

    void OnTriggerEnter(Collider other) {
        alive = true;
        if (other.tag == "Portal" && !hitPortal) {
            manager.ChangeWorlds();
            hitPortal = true;
        }
    }

    void OnTriggerExit(Collider other) {
        alive = false;
    }
}
