using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

    public Transform portalMesh;

    [Header("Main Camera Objects")]
    public Skybox currentSkybox;
    public Material currentSkyboxMaterial;
    public Transform oldWorld;

    [Header("Next Objects")]
    public Skybox nextSkybox;
    public Material nextSkyboxMaterial;
    public Transform nextWorld;

    void Start () {
        nextSkybox.material = nextSkyboxMaterial;
	}
	
	void Update () {
		
	}

    void OnTriggerEnter(Collider col) {
        if (col != null && col.tag == "PortalFrame") {
            currentSkybox.material = nextSkyboxMaterial;
            nextSkybox.material = currentSkyboxMaterial;

            portalMesh.eulerAngles = new Vector3(-90, 0, 180);

            for (int i = 0; i < nextWorld.childCount; i++) {
                ChangeLayersRecursively(nextWorld, 0);
            }
            for (int i = 0; i < oldWorld.childCount; i++) {
                if (i < oldWorld.childCount) {
                    ChangeLayersRecursively(oldWorld, LayerMask.NameToLayer("Portal"));
                    if (oldWorld.GetChild(i).gameObject.tag == "Deletable") {
                        Destroy(oldWorld.GetChild(i).gameObject);
                    }
                }
            }
        }

        //child.gameObject.layer = mask;
    }

    void ChangeLayersRecursively(Transform trans, int mask) {
        foreach (Transform child in trans) {
            child.gameObject.layer = mask;
            ChangeLayersRecursively(child, mask);
        }
    }
}
