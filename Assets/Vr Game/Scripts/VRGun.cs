using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRGun : MonoBehaviour {

    private Vector3 VRGunRotation;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = GameObject.Find("VRControllerRight").GetComponent<Transform>().position;
        transform.rotation = GameObject.Find("VRControllerRight").GetComponent<Transform>().rotation;
    }
}
