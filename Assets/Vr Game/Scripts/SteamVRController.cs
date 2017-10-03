using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class SteamVRController : MonoBehaviour {

    SteamVR_Controller.Device Device;
    SteamVR_TrackedObject trackedObj;

	void Awake ()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	void Update ()
    {
        Device = SteamVR_Controller.Input((int)trackedObj.index);


        if(Device.GetHairTriggerDown())
        {
            Debug.Log("Trigger was pulled");
        }
	}
}
