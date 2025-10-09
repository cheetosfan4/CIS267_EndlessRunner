using UnityEditor;
using UnityEngine;

public class objectMover : MonoBehaviour {
    //private bool moving = true;
    private bool started = false;
    private GameObject GUIObject;
    private GUIHandler menu;
    public float motionSpeed;
    public GameObject player;
    private Rigidbody2D rb;

    void Start() {
        rb = player.GetComponent<Rigidbody2D>();
        GUIObject = GameObject.FindWithTag("GUI");
        if (GUIObject != null) {
            menu = GUIObject.GetComponent<GUIHandler>();
        }
        else {
            Debug.Log("gui missing");
        }
    }

    void Update() {
        //pauseAndResume();
        if (started && menu.cameraMoving) {
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
}
