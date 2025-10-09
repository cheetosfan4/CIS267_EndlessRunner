using UnityEditor;
using UnityEngine;

public class hourglassBehavior : MonoBehaviour {
    private bool applied = false;
    void Start() {
    }

    void Update() {
        if (GUIHandler.instance.hourglassTimer > 0 && !applied) {
            GUIHandler.instance.updateHourglassTimer(1);
            applied = true;
            Destroy(gameObject);
        }
        else if (!applied) {
            GUIHandler.instance.updateHourglassTimer(1);
            applied = true;
        }
        else if (!GUIHandler.instance.cameraMoving) {
            GUIHandler.instance.updateHourglassTimer();
        }
        checkPress();
        if (GUIHandler.instance.hourglassTimer <= 0) {
            Destroy(gameObject);
        }
    }

    private void checkPress() {
        if (Input.GetKeyDown(KeyCode.H) && GUIHandler.instance.cameraMoving && GUIHandler.instance.hourglassTimer > 0) {
            GUIHandler.instance.cameraMoving = false;

        }
        else if (Input.GetKeyDown(KeyCode.H) && !GUIHandler.instance.cameraMoving) {
            GUIHandler.instance.cameraMoving = true;
        }
    }
}
