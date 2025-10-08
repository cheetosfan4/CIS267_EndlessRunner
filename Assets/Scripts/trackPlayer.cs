using UnityEngine;

public class trackPlayer : MonoBehaviour {
    public GameObject player;
    private Rigidbody2D rb;
    private bool stop = false;
    void Start() {
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (rb.position.y > 0f) {
            if(stop) {
                stop = false;
            }
            this.transform.position = new Vector3(this.transform.position.x, rb.position.y, -10);
        }
        else {
            //the stop variable is so that the camera's position isn't constantly getting set when it doesn't need to be
            if (!stop) {
                this.transform.position = new Vector3(this.transform.position.x, 0, -10);
                stop = true;
            }
        }

    }
}
