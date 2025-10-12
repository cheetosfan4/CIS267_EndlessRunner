using UnityEditor;
using UnityEngine;

public class objectMover : MonoBehaviour {
    private bool started = false;
    public float motionSpeed;
    public GameObject player;
    private Rigidbody2D rb;

    void Start() {
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (started && GUIHandler.instance.getCameraMoving()) {
            transform.Translate(Vector2.right * motionSpeed * Time.deltaTime);
        }
        else if (!started) {
            checkPlayerPosition();
        }
    }

    //this is so the game doesn't actually start until the player begins to move
    private void checkPlayerPosition() {
        if(rb.position.x != -9) {
            started = true;
            GUIHandler.instance.setDeletionStatus(true);
        }
    }
}
