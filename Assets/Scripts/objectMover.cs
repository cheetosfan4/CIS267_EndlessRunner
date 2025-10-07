using UnityEngine;

public class objectMover : MonoBehaviour {
    private bool moving = false;
    public float motionSpeed;
    public GameObject player;
    private Rigidbody2D rb;

    void Start() {
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (moving) {
            transform.Translate(Vector2.left * motionSpeed * Time.deltaTime);
        }
        else {
            checkPlayerPosition();
        }
    }

    private void checkPlayerPosition() {
        if(rb.position.x != -9) {
            moving = true;
        }
    }
}
