using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;

public class playerController : MonoBehaviour {
    public float moveSpeed;
    public float jumpForce;
    public float wallJumpShootDuration;

    private Rigidbody2D rb;
    private GameObject GUIObject;
    private GUIHandler menu;
    private float inputHorizontal;
    private bool grounded = false;
    private bool leftWalled = false;
    private bool rightWalled = false;
    private bool wallJump = false;
    private float wallJumpTimer = 0f;
    //these are to store what objects the player collides with, with no repeating objects
    private HashSet<Collider2D> groundsTouching = new HashSet<Collider2D>();
    private HashSet<Collider2D> leftWallsTouching = new HashSet<Collider2D>();
    private HashSet<Collider2D> rightWallsTouching = new HashSet<Collider2D>();


    void Start() {
        rb = GetComponent<Rigidbody2D>();
        GUIObject = GameObject.FindWithTag("GUI");
        if (GUIObject != null) {
            menu = GUIObject.GetComponent<GUIHandler>();
        }
        else {
            Debug.Log("gui missing");
        }
    }

    void Update() {
        movePlayer();
        jump();
    }

    private void movePlayer() {
        inputHorizontal = Input.GetAxisRaw("Horizontal");

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

        //this timer is so the default movement doesn't interfere with the wall jump
        if (wallJumpTimer > 0f) {
            wallJumpTimer -= Time.deltaTime;
        }

        //regular movement
        if (wallJumpTimer <= 0f) {
            flipPlayer(inputHorizontal);
            rb.linearVelocity = new Vector2(moveSpeed * inputHorizontal, rb.linearVelocity.y);
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
        if (!collision.gameObject.CompareTag("Injure")) {
            //this loops through each of the points of the object that the player collides with
            foreach (ContactPoint2D contact in collision.contacts) {
                //this essentially checks what angle the surface of the object is when the player lands on it
                //if it is around 1 that means it's an upward surface
                //this is so the player only regains their ability to jump when they touch the top surface of an object
                if (contact.normal.y > 0.98f) {
                    groundsTouching.Add(collision.collider);
                }
                if (contact.normal.x > 0.98f && !collision.gameObject.CompareTag("Terrain")) {
                    rightWallsTouching.Add(collision.collider);
                }
                if (contact.normal.x < -0.98f && !collision.gameObject.CompareTag("Terrain")) {
                    leftWallsTouching.Add(collision.collider);
                }
            }

            //only sets these to true if the hash sets contain any contacts
            grounded = groundsTouching.Count > 0;
            leftWalled = leftWallsTouching.Count > 0;
            rightWalled = rightWallsTouching.Count > 0;
        }

        if (collision.gameObject.CompareTag("Injure")) {
            menu.uponDeath();
        }

        if (collision.gameObject.CompareTag("Enemy")) {
            bool dead = true;
            //checks each point to make sure that the player jumped on top of the enemy
            foreach (ContactPoint2D contact in collision.contacts) {
                if (contact.normal.y > 0.5f) {
                    dead = false;
                    break;
                }
            }
            if (dead) {
                menu.uponDeath();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        //attempting to remove the colliders does nothing if they are not in the hash set
        //this is useful since there is no need to check beforehand
        groundsTouching.Remove(collision.collider);
        leftWallsTouching.Remove(collision.collider);
        rightWallsTouching.Remove(collision.collider);

        //only sets these to false if the hash sets are empty
        //this was to fix a bug where the player was unable to jump, most likely due to collisions not registering properly
        grounded = groundsTouching.Count > 0;
        leftWalled = leftWallsTouching.Count > 0;
        rightWalled = rightWallsTouching.Count > 0;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("BottomBorder")) {
            menu.uponDeath();
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("LeftBorder") && rb.position.x < collision.gameObject.transform.position.x) {
            menu.uponDeath();
        }
    }
}