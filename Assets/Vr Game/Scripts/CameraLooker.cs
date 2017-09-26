using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraLooker : MonoBehaviour {

    public Transform looker;
    public Transform portalTransform;

    bool portal;
    bool hasPortal;

    float timer;

    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update () {
        
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Hitter"))) {
            if (hit.collider != null) {
                looker.position = hit.point;
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = new Vector2(Screen.width / 2, Screen.height / 2);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0) {
                //WorldUI is my layer name
                if (results[0].gameObject.layer == LayerMask.NameToLayer("UI")) {
                    if (results[results.Count - 1].gameObject.GetComponent<Button>()) {
                        results[results.Count - 1].gameObject.GetComponent<Button>().onClick.Invoke();
                    }
                    results.Clear();
                }
            }
        }

        if(!hasPortal && portal) {
            timer += Time.deltaTime;

            portalTransform.localScale = new Vector3(timer * 1.0f, 1, 1);

            if(timer >= 1) {
                hasPortal = true;
            }
        }
    }

    public void PressPlay() {
        portal = true;
    }
}
