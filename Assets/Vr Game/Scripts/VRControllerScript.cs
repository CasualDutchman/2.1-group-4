﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class VRControllerScript : MonoBehaviour {

    Vector3 HandPosition;
    public string Hand = null;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if(Hand == "Right")
        {
            HandPosition = InputTracking.GetLocalPosition(VRNode.RightHand);
            transform.position = HandPosition;
        }

        if (Hand == "Left")
        {
            HandPosition = InputTracking.GetLocalPosition(VRNode.LeftHand);
            transform.position = HandPosition;
        }
    }
}
