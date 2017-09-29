using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRGun : MonoBehaviour
{
    public GameObject BulletPrefab;
    private Vector3 VRGunRotation;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        MoveGun();
        if(Input.GetButtonDown("trigger")
        {
            FireGun();
        }
    }

    //Move the gun with the VR Controller
    private void MoveGun()
    {
        transform.position = GameObject.Find("VRControllerRight").GetComponent<Transform>().position;
        transform.rotation = GameObject.Find("VRControllerRight").GetComponent<Transform>().rotation;
    }

    private void FireGun()
    {
        //Fire the gun
        Debug.Log("Gun Fired");
        GameObject Bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
    }
}