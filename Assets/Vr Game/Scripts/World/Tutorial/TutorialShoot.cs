using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShoot : MonoBehaviour {

    public List<GameObject> targets = new List<GameObject>();

    public bool AllTargetsDead() {
        string temp = "";

        foreach (GameObject go in targets) {
            if (go == null) {
                temp += "j";
            }
        }

        return temp.Length >= targets.Count;
    }
}
