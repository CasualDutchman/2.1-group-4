using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSkybox : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
        transform.localEulerAngles = -transform.parent.eulerAngles;
	}
}
