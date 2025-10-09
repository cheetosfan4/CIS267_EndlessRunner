using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Progress;

public class playerController : MonoBehaviour {
    public float moveSpeed;
    public float jumpForce;
    public float wallJumpShootDuration;
    public int hitPoints;

    private Rigidbody2D rb;
    private float inputHorizontal;
    private bool grounded = false;
    private bool leftWalled = false;
    private bool rightWalled = false;
    private bool wallJump = false;
    private float wallJumpTimer = 0f;
    private bool dead = false;
    private itemData itemGrabbed;
    private GameObject accessoryToSpawn;
    //these are to store what objects the player collides with, with no repeating objects
    private HashSet<Collider2D> groundsTouching = new HashSet<Collider2D>();
    private HashSet<Collider2D> leftWallsTouching = new HashSet<Collider2D>();
    private HashSet<Collider2D> rightWallsTouching = new HashSet<Collider2D>();


    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        movePlayer();
        jump();
        death();
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

    private void death() {
        if (dead) {
            GUIHandler.instance.uponDeath();
            dead = false;
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
            hitPoints--;
            dead = hitPoints <= 0;
        }

        if (collision.gameObject.CompareTag("Enemy")) {
            bool hit = true;
            //checks each point to make sure that the player jumped on top of the enemy
            foreach (ContactPoint2D contact in collision.contacts) {
                if (contact.normal.y > 0.5f) {
                    hit = false;
                    GUIHandler.instance.updateScore(50);
                    break;
                }
            }
            if (hit) {
                hitPoints--;
                dead = hitPoints <= 0;
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
            GUIHandler.instance.uponDeath();
            dead = false;
        }
        if (collision.gameObject.CompareTag("Fruit")) {
            Destroy(collision.gameObject);
            GUIHandler.instance.updateScore(5);
        }
        if (collision.gameObject.CompareTag("Item")) {
            itemGrabbed = collision.gameObject.GetComponent<itemData>();
            accessoryToSpawn = Instantiate(itemGrabbed.accessory);
            accessoryToSpawn.transform.SetParent(this.gameObject.transform);
            accessoryToSpawn.transform.position = new Vector2(rb.position.x, rb.position.y);
            accessoryToSpawn.transform.eulerAngles = new Vector3(rb.transform.eulerAngles.x, rb.transform.eulerAngles.y, rb.transform.eulerAngles.z);
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("LeftBorder") && rb.position.x < collision.gameObject.transform.position.x) {
            GUIHandler.instance.uponDeath();
            dead = false;
        }
    }
}