using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;

public class playerController : MonoBehaviour {
    Rigidbody2D rb;
    private float inputHorizontal;
    public float moveSpeed;
    public float jumpForce;
    private bool grounded = false;
    private bool leftWalled = false;
    private bool rightWalled = false;
    private bool wallJump = false;
    public float wallJumpShootDuration;
    private float wallJumpTimer = 0f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        movePlayer();
        jump();
    }

    private void movePlayer() {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        if(wallJumpTimer <= 0f) {
            flipPlayer(inputHorizontal);
            rb.linearVelocity = new Vector2(moveSpeed * inputHorizontal, rb.linearVelocity.y);
        }

        if (wallJump && rightWalled) {
            rb.linearVelocity = new Vector2(moveSpeed * 1.2f, jumpForce);
            flipPlayer(moveSpeed);
            wallJumpTimer = wallJumpShootDuration;
        }
        else if (wallJump && leftWalled) {
            rb.linearVelocity = new Vector2(-moveSpeed * 1.2f, jumpForce);
            flipPlayer(-moveSpeed);
            wallJumpTimer = wallJumpShootDuration;
        }
        wallJump = false;

        if(wallJumpTimer > 0f) {
            wallJumpTimer -= Time.deltaTime;
        }
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
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !grounded && (rightWalled || leftWalled)) {
            wallJump = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            //this loops through each of the points of the object that the player collides with
            foreach (ContactPoint2D contact in collision.contacts) {
                //this essentially checks what angle the surface of the ground is when the player lands on it
                //if it is around 1 that means it's an upward surface
                //this is so the player only regains their ability to jump when they touch the top surface of a ground object
                if(contact.normal.y > 0.9f) {
                    grounded = true;
                    break;
                }
            }
        }
        if(collision.gameObject.CompareTag("Wall")) {
            foreach (ContactPoint2D contact in collision.contacts) {
                //same method as ground checking, but determines what side of a wall the player is on
                if(contact.normal.x > 0.9f) {
                    rightWalled = true;
                    leftWalled = false;
                    break;
                }
                else if(contact.normal.x < -0.9f) {
                    leftWalled = true;
                    rightWalled = false;
                    break;
                }
            }
        }
        if (collision.gameObject.CompareTag("Terrain")) {
            foreach (ContactPoint2D contact in collision.contacts) {
                //same method as ground checking, but determines what side of a wall the player is on
                if (contact.normal.x > 0.9f) {
                    rightWalled = true;
                    leftWalled = false;
                    break;
                }
                else if (contact.normal.x < -0.9f) {
                    leftWalled = true;
                    rightWalled = false;
                    break;
                }
                else if (contact.normal.y > 0.9f) {
                    grounded = true;
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Ground")) {
            grounded = false;
        }
        if (collision.gameObject.CompareTag("Wall")) {
            leftWalled = false;
            rightWalled = false;
        }
        if(collision.gameObject.CompareTag("Terrain")) {
            grounded = false;
            leftWalled = false;
            rightWalled = false;
        }
    }
}