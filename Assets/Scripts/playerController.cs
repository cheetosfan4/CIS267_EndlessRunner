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
    //this is to store what objects the player collides with, with no repeating objects
    private HashSet<Collider2D> groundsTouching = new HashSet<Collider2D>();


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

        //this timer is so the default movement doesn't interfere with the wall jump
        //without it, the player's movement is reset to 0 on the next frame
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
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Terrain")) {
            //this loops through each of the points of the object that the player collides with
            foreach (ContactPoint2D contact in collision.contacts) {
                //this essentially checks what angle the surface of the ground/terrain is when the player lands on it
                //if it is around 1 that means it's an upward surface
                //this is so the player only regains their ability to jump when they touch the top surface of a ground/terrain object
                if(contact.normal.y > 0.9f) {
                    groundsTouching.Add(collision.collider);
                    grounded = true;
                    break;
                }
            }
        }
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Terrain")) {
            foreach (ContactPoint2D contact in collision.contacts) {
                //same method as ground checking, but determines what side of a wall/terrain the player is on
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
        if (collision.gameObject.CompareTag("Injure")) {
            menu.uponDeath();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Terrain")) {
            //attempting to remove the collider does nothing if it is not in the hash set
            //this is useful since there is no need to check beforehand
            groundsTouching.Remove(collision.collider);
            //only sets grounded to false if the hash set is empty
            //this was to fix a bug where the player was unable to jump
            grounded = (groundsTouching.Count > 0);
        }
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Terrain")) {
            leftWalled = false;
            rightWalled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("BottomBorder")) {
            menu.uponDeath();
        }
    }
}