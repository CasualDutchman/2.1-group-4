using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class CameraMouseController : MonoBehaviour {

    public float mouseSpeed = 5;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    Transform parent;

    float speed;

    void Start () {
        if (VRSettings.enabled) {
            this.enabled = false;
        }else {
            GameObject go = new GameObject("CameraRig");
            if (transform.parent != null)
                go.transform.SetParent(transform.parent);
            parent = go.transform;
            parent.position = transform.position;
            transform.SetParent(parent);
        }
	}
	
	void Update () {
        if (parent != null) {
            yaw += Input.GetAxis("Mouse X") * mouseSpeed;
            pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * mouseSpeed, -90, 90);

            transform.localEulerAngles = new Vector3(pitch, 0, 0);
            parent.localEulerAngles = new Vector3(0, yaw, 0);
        }

        parent.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);

        if (Input.GetKey(KeyCode.W)) {
            speed = Mathf.Clamp(speed + (1 * Time.deltaTime), 0, 10);
        }else {
            speed = Mathf.Clamp(speed - (1 * Time.deltaTime), 0, 10);
        }
    }
}
