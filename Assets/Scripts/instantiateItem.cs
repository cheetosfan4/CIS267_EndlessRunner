using System.Linq;
using UnityEngine;

public class instantiateItem : MonoBehaviour {
    public GameObject[] items;
    public GameObject fruit;
    private GameObject objectToSpawn;
    private int randomItem;
    private int fruitChance;
    void Start() {
        randomItem = Random.Range(0, items.Length);
        fruitChance = Random.Range(0, 10);
        //30% chance to spawn a fruit instead of an item, to make items rarer
        if (fruitChance <= 6) {
            objectToSpawn = Instantiate(items[randomItem]);
            objectToSpawn.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            Destroy(gameObject);
        }
        else if (fruitChance >= 7) {
            objectToSpawn = Instantiate(fruit);
            objectToSpawn.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            Destroy(gameObject);
        }

    }
}
