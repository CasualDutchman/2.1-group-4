using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    //variables to be able to fire
    public Transform muzzle = null;
    public GameObject bullet = null;

    void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            FireGun();
        }
    }

    public void FireGun()
    {
        //Fire ze Gun!
        GameObject FiredBullet = Instantiate(bullet);
        FiredBullet.transform.position = muzzle.position;
        FiredBullet.transform.rotation = muzzle.rotation;
        FiredBullet.GetComponent<Rigidbody>().velocity = muzzle.forward * 50;

    }
}
