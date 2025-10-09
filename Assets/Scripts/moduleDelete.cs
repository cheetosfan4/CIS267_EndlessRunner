using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class moduleDelete : MonoBehaviour {
    private void OnTriggerExit2D(Collider2D collision) {
        //checks to make sure the player has loaded in and started moving before triggering function
        //this was to fix a bug where the player was given 100 points just by pressing retry
        if (collision.gameObject.CompareTag("Module") && GUIHandler.instance.getDeletionStatus()) {
            Destroy(collision.gameObject);
            GUIHandler.instance.updateScore(100);
        }
        if (collision.gameObject.CompareTag("House") && GUIHandler.instance.getDeletionStatus()) {
            Destroy(collision.gameObject);
        }
    }
}