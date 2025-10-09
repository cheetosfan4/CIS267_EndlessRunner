using UnityEditor;
using UnityEngine;

public class enemyController : MonoBehaviour {
    public float enemySpeed;
    public GameObject leftBoundary;
    public GameObject rightBoundary;
    private bool arrived = false;
    private int directionFacing = 0;
    private float chosenPosition;
    private float leftBoundaryPos;
    private float rightBoundaryPos;
    private float timer;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        leftBoundaryPos = leftBoundary.transform.position.x;
        rightBoundaryPos = rightBoundary.transform.position.x;
        chosenPosition = Random.Range(leftBoundaryPos, rightBoundaryPos);
        flipEnemy();
    }

    void Update() {
        move();
    }

    private void move() {
        timer -= Time.deltaTime;

        //moves the enemy towards chosen position
        //doesn't move if the enemy is heading past their chosen position from either side, or if they've already arrived at their position
        if (!(rb.position.x >= chosenPosition && directionFacing == 1) && !(rb.position.x <= chosenPosition && directionFacing == -1) && !arrived) {
            rb.linearVelocity = new Vector2(directionFacing * enemySpeed, rb.linearVelocityY);
        }
        //if the enemy is past their chosen position, they have arrived
        //a timer is set so the enemy stands in place momentarily
        else if (!arrived) {
            arrived = true;
            timer = 0.6f;
        }

        //once the timer is up, a new position is chosen and the enemy faces towards it
        if (timer <= 0 && arrived) {
            chosenPosition = Random.Range(leftBoundaryPos, rightBoundaryPos);
            flipEnemy();
            arrived = false;
        }
    }

    private void flipEnemy() {
        if (chosenPosition < rb.position.x) {
            directionFacing = -1;
        }
        else {
            directionFacing = 1;
        }

        if (directionFacing > 0) {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (directionFacing < 0) {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            foreach (ContactPoint2D contact in collision.contacts) {
                //this checks to make sure that the player jumps on top of the enemy
                if (contact.normal.y < -0.5f) {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("BottomBorder")) {
            Destroy(gameObject);
        }
    }
}
