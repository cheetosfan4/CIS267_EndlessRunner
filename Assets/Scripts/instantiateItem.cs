using System.Linq;
using UnityEngine;

public class instantiateItem : MonoBehaviour {
    public GameObject[] items;
    private GameObject itemToSpawn;
    private int randomItem;
    void Start() {
        randomItem = Random.Range(0, items.Length);
        itemToSpawn = Instantiate(items[randomItem]);
        itemToSpawn.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Destroy(gameObject);
    }
}
