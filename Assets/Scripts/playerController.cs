using UnityEngine;

public class playerController : MonoBehaviour {
    Rigidbody2D rb;
    private float inputHorizontal;
    public float moveSpeed;
    public float jumpForce;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        movePlayer();
        jump();
    }

    private void movePlayer() {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        flipPlayer(inputHorizontal);
        rb.linearVelocity = new Vector2(moveSpeed * inputHorizontal, rb.linearVelocity.y);
    }

    private void flipPlayer(float inputHorizontal) {
        if (inputHorizontal > 0) {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (inputHorizontal < 0) {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void jump() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}
