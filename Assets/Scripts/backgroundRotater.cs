using UnityEngine;

public class backgroundRotater : MonoBehaviour {
    public float rotationSpeed;
    private SpriteRenderer bg;
    void Start() {
        bg = GetComponent<SpriteRenderer>();
    }

    void Update() {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
