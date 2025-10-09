using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class moduleDelete : MonoBehaviour {
    private GameObject GUIObject;
    private GUIHandler menu;
    private bool inside = false;

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
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Module")) {
            inside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Module") && inside) {
            Destroy(collision.gameObject);
            menu.updateScore(50);
            inside = false;
        }
    }
}