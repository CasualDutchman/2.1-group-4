using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class VRControllerScript : MonoBehaviour {

    public Vector3 HandPosition;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        HandPosition = InputTracking.GetLocalPosition(VRNode.RightHand);

    }
}
