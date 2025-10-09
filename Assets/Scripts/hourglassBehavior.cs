using UnityEditor;
using UnityEngine;

public class hourglassBehavior : MonoBehaviour {
    private GameObject GUIObject;
    private GUIHandler menu;
    private bool applied = false;
    void Start() {
        GUIObject = GameObject.FindWithTag("GUI");
        if (GUIObject != null) {
            menu = GUIObject.GetComponent<GUIHandler>();
        }
        else {
            Debug.Log("gui missing");
        }
    }

    void Update() {
        if (menu.hourglassTimer > 0 && !applied) {
            menu.updateHourglassTimer(1);
            applied = true;
            Destroy(gameObject);
        }
        else if (!applied) {
            menu.updateHourglassTimer(1);
            applied = true;
        }
        else if (!menu.cameraMoving) {
            menu.updateHourglassTimer();
        }
        checkPress();
        if (menu.hourglassTimer <= 0) {
            Destroy(gameObject);
        }
    }

    private void checkPress() {
        if (Input.GetKeyDown(KeyCode.H) && menu.cameraMoving && menu.hourglassTimer > 0) {
            menu.cameraMoving = false;

        }
        else if (Input.GetKeyDown(KeyCode.H) && !menu.cameraMoving) {
            menu.cameraMoving = true;
        }
    }
}
