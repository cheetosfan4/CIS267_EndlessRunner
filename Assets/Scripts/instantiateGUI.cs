using UnityEngine;

public class instantiateGUI : MonoBehaviour {
    public GameObject GUI;
    private void Awake() {
        //creates GUI when running gameplay scene in editor, for testing
        if(GUIHandler.instance == null) {
            Instantiate(GUI);
            GUIHandler.instance.debugMode();
        }
        Destroy(gameObject);
    }
}
