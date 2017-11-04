using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class SteamVRController : MonoBehaviour {

    //Create variables for tracking the device and manipulation
    SteamVR_Controller.Device Device;
    SteamVR_TrackedObject trackedObj;

    public Transform Muzzle;
    public GameObject Bullet;

    float timer;

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
    }

    //Function for picking up any object
    private void PickUpObject(Collider Col)
    {
        //get the device input to pickup an object(using the trigger)
        if (Device.GetHairTriggerDown())
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


}
