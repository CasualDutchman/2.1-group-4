using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class SteamVRController : MonoBehaviour {

    //Create variables for tracking the device and manipulation
    SteamVR_Controller.Device Device;
    SteamVR_TrackedObject trackedObj;

    GameObject holdedObject;
    GameObject highlightedObject;

    public Transform defaultParent;

    //bool to check if an object is allready being held
    bool holding;

    bool inHighlightedArea;

	void Awake ()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	void Update ()
    {
        //assign the device for input
        Device = SteamVR_Controller.Input((int)trackedObj.index);

        if (holding)
        {
            if (Device.GetHairTriggerDown())
            {
                PrimaryObjectAction();
            }

            if (Device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                DropObject();
            }
        }
        else
        {
            if (Device.GetHairTriggerDown() && inHighlightedArea)
            {
                PickupObject(highlightedObject);
            }
        }
    }

    private void OnTriggerStay(Collider Col)
    {
        inHighlightedArea = true;
        highlightedObject = Col.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        inHighlightedArea = false;
        highlightedObject = null;
    }

    //When pulling the trigger, fire!
    private void PrimaryObjectAction()
    {
        //fire the gun
        if (holdedObject.tag == "Weapon")
        {
            holdedObject.GetComponent<GunController>().FireGun();
        }

    }

    private void PickupObject(GameObject go)
    {
        holdedObject = go;

        //Get position and transformation of the object picked up
        holdedObject.transform.position = trackedObj.transform.position;
        holdedObject.transform.rotation = trackedObj.transform.rotation;

        //Set the parent of the object to the SteamVR controller to make transformation easier
        holdedObject.transform.SetParent(gameObject.transform);

        holding = true;
        
    }

    private void DropObject()
    {
        resetPosition();

        holdedObject = null;
        holding = false;
    }

    void resetPosition() {
        if (holding || holdedObject != null) {
            holdedObject.transform.SetParent(defaultParent);
            holdedObject.transform.localPosition = new Vector3(-0.06f, 0.55f, -0.07f);
            holdedObject.transform.localEulerAngles = new Vector3(0, 0, -50);
        }
    }

    public void ForceDrop() {
        DropObject();
    }
}
