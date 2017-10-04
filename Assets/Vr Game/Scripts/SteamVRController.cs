﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class SteamVRController : MonoBehaviour {

    //Create variables for tracking the device and manipulation
    SteamVR_Controller.Device Device;
    SteamVR_TrackedObject trackedObj;

	void Awake ()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	void Update ()
    {
        //assign the device for input
        Device = SteamVR_Controller.Input((int)trackedObj.index);
    }

    private void OnTriggerStay(Collider Col)
    {
        PickUpObject(Col);

        //get the trigger being pulled
        if (Device.GetHairTriggerDown() && Col.name == "Gun")
        {
            FireGun();
        }
    }

    //Function for picking up any object
    private void PickUpObject(Collider Col)
    {
        //get the device input to pickup an object(using the trigger)
        if (Device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            //Get position and transformation of the object picked up
            Col.gameObject.transform.position = trackedObj.transform.position;
            Col.gameObject.transform.rotation = trackedObj.transform.rotation;

            //Make sure the object is kinematic
            Col.attachedRigidbody.isKinematic = true;

            //Set the parent of the object to the SteamVR controller to make transformation easier
            Col.gameObject.transform.SetParent(gameObject.transform);
        }

        //Get input from device to drop object (The grip button)
        if (Device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            //Set the parent of the object as the player
            Col.gameObject.transform.SetParent(gameObject.transform.parent);
        }
    }

    private void FireGun()
    {
        //TODO implement gun firing
        Debug.Log("Gun is fired");

        //does not work for some reason
        Debug.DrawRay(trackedObj.transform.position, trackedObj.transform.forward, Color.red, 1f);
        
        //variable to get hit info
        RaycastHit hit;

        //actually firing of the "gun"
        if(Physics.Raycast(trackedObj.transform.position, trackedObj.transform.forward, out hit) && hit.collider.tag != "Player")
        {
            Debug.Log("Something hit");
            Destroy(hit.transform.gameObject);
        }
        
    }
}
