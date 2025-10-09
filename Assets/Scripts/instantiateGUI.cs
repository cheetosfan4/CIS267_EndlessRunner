using UnityEngine;

public class instantiateGUI : MonoBehaviour {
    public GameObject GUI;
    private void Awake() {
        if(GUIHandler.instance == null) {
            Instantiate(GUI);
            GUIHandler.instance.debugMode();
        }
        Destroy(gameObject);
    }
}
