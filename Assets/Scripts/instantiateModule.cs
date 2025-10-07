using UnityEngine;

public class instantiateModule : MonoBehaviour {
    //creates array of modules in the editor so the prefabs can just be dragged in
    public GameObject[] Modules;

    private int randomNum;
    private GameObject moduleToSpawn;
    private BoxCollider2D modulePos;

    void Start() {

    }

    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("ModuleExit")) {
            //picks a random value based on the array's size to determine which module to spawn
            randomNum = Random.Range(0, Modules.Length);
            moduleToSpawn = Instantiate(Modules[randomNum].gameObject);
            modulePos = moduleToSpawn.GetComponent<BoxCollider2D>();
            //makes sure that the new module spawns right after the current module, based on its size
            moduleToSpawn.transform.position = new Vector2((this.transform.position.x + (modulePos.size.x * 0.5f)), 0);
        }
    }
}