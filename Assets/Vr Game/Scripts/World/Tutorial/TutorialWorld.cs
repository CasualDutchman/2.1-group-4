using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWorld : World {

    float timer, timer2;

    public TutorialWorld(WorldManager wm, LayerMask mask, Vector3 pos, Vector3 euler) {
        name = "Tutorial World";

        SetupWorld(wm, mask, pos, euler);
        this.sunColor = Color.white;
        this.availableGamemodes.Add("TempleRun");
    }

	public override void SetupWorld(WorldManager wm, LayerMask mask, Vector3 pos, Vector3 euler) {
        Debug.Log("Setup First World");
        manager = wm;
        portalMask = mask;
        changedPos = pos;
        changedRot = euler;

        bike = GameObject.Find("bike").transform;

        worldObject = new GameObject(name + manager.worldIndex);
        worldObject.transform.SetParent(manager.transform);

        nextSpawnLoc = new GameObject("next spawn");
        nextSpawnLoc.transform.SetParent(worldObject.transform);
        nextSpawnLoc.transform.position = new Vector3(0, 0, 100);

        //----------

        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);
        go.transform.SetParent(worldObject.transform);
        go.transform.localScale = Vector3.one * 200;
    }

    public override void SpawnPortal() {
        portalObject = GameObject.Instantiate(manager.portalObject, manager.transform);
        portalObject.transform.position = nextSpawnLoc.transform.position + bike.forward * 70;
        portalObject.transform.eulerAngles = bike.forward;

        portalindex = manager.amountOfPanels;

        manager.SetupNewWorld(portalObject.transform.position, bike.forward);
    }

    public override void UpdateWorld() {

        if (nextSpawnLoc != null && bike != null) {
            nextSpawnLoc.transform.position = bike.position + bike.forward * 20;
        }

        if (this != manager.prevWorld) {
            if (portalObject != null && Vector3.Distance(bike.position, portalObject.transform.position) <= 100) {
                timer2 += Time.deltaTime;
                if (timer2 < 4)
                    portalObject.transform.GetChild(0).localScale = new Vector3(Mathf.Clamp(timer2 / 2, 0, 2), 1, 1);
            }
        } else {
            Debug.Log("go");

            timer += Time.deltaTime;

            if (timer >= 3) {
                portalObject.transform.GetChild(0).localScale = new Vector3(Mathf.Clamp(5 - timer, 0, 2), 1, 1);

                if (timer >= 6) {
                    GameObject.Destroy(portalObject);
                    GameObject.Destroy(worldObject);
                    manager.hasPortal = false;
                    manager.prevWorld = null;
                }
            }
        }
    }
}
