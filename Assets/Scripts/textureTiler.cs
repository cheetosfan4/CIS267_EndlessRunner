using UnityEngine;

public class textureTiler : MonoBehaviour {
    private float height;
    private float width;
    //public float scaleMultiplier;
    private SpriteRenderer display;
    private BoxCollider2D boxCollider;

    void Start() {
        display = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        float width = (this.transform.localScale.x /** scaleMultiplier*/);
        float height = (this.transform.localScale.y /** scaleMultiplier*/);

        //resets transformation on base object
        //then, scales the sprite renderer size and collider size based on the base object's size
        //this is so the ground objects can be scaled easily in the editor, and then this script makes sure they tile correctly
        this.transform.localScale = new Vector2(1, 1);
        display.size = new Vector2(width, height);
        boxCollider.size = new Vector2(width, height);
    }

    void Update() {
        
    }
}
