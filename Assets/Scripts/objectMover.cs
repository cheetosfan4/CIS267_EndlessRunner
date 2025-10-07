using UnityEngine;

public class objectMover : MonoBehaviour {
    private bool moving = true;
    private bool started = false;
    public float motionSpeed;
    public GameObject player;
    private Rigidbody2D rb;

    void Start() {
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update() {
        pauseAndResume();
        if (started && moving) {
            transform.Translate(Vector2.right * motionSpeed * Time.deltaTime);
        }
        else {
            checkPlayerPosition();
        }
    }

    //this is so the game doesn't truly start until the player begins to move
    private void checkPlayerPosition() {
        if(rb.position.x != -9) {
            started = true;
        }
    }
    private void pauseAndResume() {
        if(Input.GetKeyDown(KeyCode.T) && moving) {
            moving = false;
        }
        else if(Input.GetKeyDown(KeyCode.T) && !moving) {
            moving = true;
        }
    }
}
