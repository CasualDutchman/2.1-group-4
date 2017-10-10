using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusController : MonoBehaviour {

    public Transform Muzzle;
    public GameObject Bullet;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        OVRInput.Update();
        if(OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            FireGun();
        }
    }

    private void FixedUpdate()
    {
        OVRInput.FixedUpdate();
    }

    private void FireGun()
    {
        //Fire ze Gun!
        GameObject FiredBullet = Instantiate(Bullet);
        FiredBullet.transform.position = Muzzle.position;
        FiredBullet.transform.rotation = Muzzle.rotation;
        FiredBullet.GetComponent<Rigidbody>().velocity = Muzzle.forward * 50;

    }
}
