using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PickupParent : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;

	void Awake ()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	void Update ()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);


        if(device.GetHairTriggerDown())
        {
            Debug.Log("Trigger was pulled");
        }
	}
}
