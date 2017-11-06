using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour {

    public Transform target;

    public AnimationCurve wobble;

    float timer;

	void Start () {
		
	}
	
	void Update () {
        timer += Time.deltaTime * 3.0f;
        if (timer >= 1)
            timer -= 1;


        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

        Vector3 euler = transform.eulerAngles;
        euler.z = wobble.Evaluate(timer) * 10.0f;
        transform.eulerAngles = euler;

        Ray ray = new Ray(transform.position + Vector3.up * 3, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10, LayerMask.NameToLayer("Ground"))) {
            if (hit.collider != null) {
                transform.position = hit.point;
            }
        }

        transform.Translate(-transform.forward * Time.deltaTime);
    }
}
