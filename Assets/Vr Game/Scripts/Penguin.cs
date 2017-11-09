using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour {

    public Transform target;

    public AnimationCurve wobble;

    float timer;

    Rigidbody rb;

    bool alive = true;

	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
        if (alive) {
            timer += Time.deltaTime * 3.0f; // timer needed for the AnimationCurve wobble
            if (timer >= 1)
                timer -= 1;

            //rotaes the object towards the player
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

            //aply wobble, when walking
            Vector3 euler = transform.eulerAngles;
            euler.z = wobble.Evaluate(timer) * 10.0f;
            transform.eulerAngles = euler;

            //keep it on the ground (no flying or digging in ground)
            Ray ray = new Ray(transform.position + Vector3.up * 8, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 20, LayerMask.GetMask("Ground"))) {
                if (hit.collider != null) {
                    transform.position = hit.point;
                }
            }

            //move forward
            transform.Translate(transform.forward * Time.deltaTime, Space.World);
        } 
        else {
            timer += Time.deltaTime;
            if (timer >= 5) {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("Bullet")) { // when hit, alive=false, apply force
            rb.isKinematic = false;
            rb.AddForce(collision.relativeVelocity * 100);

            alive = false;
            timer = 0;

            GameObject.FindObjectOfType<WorldManager>().AddScore();

            Destroy(collision.gameObject);
        }
    }
}
