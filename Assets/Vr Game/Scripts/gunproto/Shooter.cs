using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public Transform bulletExit;
    public GameObject shellPreset;

    float timer;

	void Update () {

        if (Input.GetKey(KeyCode.Space)) {
            timer += Time.deltaTime;

            if (timer >= 1) {
                timer -= 1;

                GameObject go = Instantiate(shellPreset);
                go.transform.position = bulletExit.position;
                go.transform.rotation = bulletExit.rotation;
                go.GetComponent<Rigidbody>().velocity = bulletExit.forward * 50;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space)) {
            timer = 0;
        }
    }
}
