using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class VRControllerScript : MonoBehaviour {

    //variables for the position and rotation of the controllers
    Vector3 PlayerPosition;
    Vector3 HandPosition;
    Quaternion HandRotation;

    public string Hand = null;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        PlayerPosition = gameObject.GetComponentInParent<Transform>().position;


        if (Hand == "Right")
        {
            HandPosition = InputTracking.GetLocalPosition(VRNode.RightHand);
            transform.position = HandPosition;// + PlayerPosition;

            HandRotation = InputTracking.GetLocalRotation(VRNode.RightHand);
            transform.rotation = HandRotation;
        }

        if (Hand == "Left")
        {
            HandPosition = InputTracking.GetLocalPosition(VRNode.LeftHand);
            transform.position = HandPosition;// + PlayerPosition;

            HandRotation = InputTracking.GetLocalRotation(VRNode.LeftHand);
            transform.rotation = HandRotation;
        }
        Debug.Log(transform.rotation);
        
    }
}
