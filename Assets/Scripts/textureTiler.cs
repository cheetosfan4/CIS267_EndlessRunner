using UnityEngine;

public class textureTiler : MonoBehaviour {

    void Start() {
        var brick = GetComponentInChildren<SpriteRenderer>();
        var collider = GetComponent<BoxCollider2D>();

        float width = (this.transform.localScale.x);
        float height = (this.transform.localScale.y);

        //resets transformation on base object
        //then, scales the sprite renderer size and collider size based on the base object's size
        //this is so the ground objects can be scaled easily in the editor, and then this script makes sure they tile correctly
        this.transform.localScale = new Vector2(1, 1);
        brick.size = new Vector2(width, height);
        collider.size = new Vector2(width, height);
    }

    void Update() {
        
    }
}
