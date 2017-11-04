using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
