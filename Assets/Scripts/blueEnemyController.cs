using System.Collections.Generic;
using UnityEngine;

public class blueEnemyController : MonoBehaviour {
    public float jumpHeight;
    private int directionFacing;
    private Rigidbody2D rb;
    private bool grounded = false;
    private bool jumped = false;
    private HashSet<Collider2D> groundsTouching = new HashSet<Collider2D>();

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        jump();
    }

    private void jump() {
        if (!grounded) {
            jumped = false;
        }
        if (grounded && !jumped) {
            rb.linearVelocity = new Vector2(0, jumpHeight);
            jumped = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        foreach (ContactPoint2D contact in collision.contacts) {
            if (contact.normal.y > 0.75f) {
                groundsTouching.Add(collision.collider);
            }
            if (contact.normal.y < -0.5f && collision.gameObject.CompareTag("Player")) {
                Destroy(gameObject);
            }
        }

        grounded = groundsTouching.Count > 0;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        groundsTouching.Remove(collision.collider);
        grounded = groundsTouching.Count > 0;
    }
}
