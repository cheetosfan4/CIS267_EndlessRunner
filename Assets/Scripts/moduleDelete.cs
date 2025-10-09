using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class moduleDelete : MonoBehaviour {
    private bool inside = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Module")) {
            inside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Module") && inside) {
            Destroy(collision.gameObject);
            GUIHandler.instance.updateScore(50);
            inside = false;
        }
    }
}