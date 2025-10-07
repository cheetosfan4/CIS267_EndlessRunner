using Unity.VisualScripting;
using UnityEngine;

public class moduleDelete : MonoBehaviour {

    void Start() {

    }

    void Update() {

    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Module")) {
            Destroy(collision.gameObject);
        }
    }
}
