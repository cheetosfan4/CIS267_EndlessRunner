using UnityEngine;

public class textureTiler : MonoBehaviour {
    private float height;
    private float width;
    private SpriteRenderer display;
    private BoxCollider2D boxCollider;

    void Start() {
        display = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        float width = (this.transform.localScale.x);
        float height = (this.transform.localScale.y);

        //resets transformation on base object
        //then, scales the sprite renderer size and collider size based on the base object's initial size
        //this is so any objects with tiled sprites can be scaled easily in the editor, and then this script makes sure they tile correctly
        this.transform.localScale = new Vector2(1, 1);
        display.size = new Vector2(width, height);
        boxCollider.size = new Vector2(width, height);
    }
}
