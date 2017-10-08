using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotation : MonoBehaviour {

    public Transform copyTransform;

	void Update () {
        if(copyTransform != null)
            transform.localEulerAngles = copyTransform.localEulerAngles;
	}
}
