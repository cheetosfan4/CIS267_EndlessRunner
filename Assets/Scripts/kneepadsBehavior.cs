using UnityEditor;
using UnityEngine;

public class kneepadsBehavior : MonoBehaviour {
    private playerController player;
    private GameObject GUIObject;
    private GUIHandler menu;
    private bool applied = false;
    private int currentHitPoints;
    void Start() {
        player = GetComponentInParent<playerController>();
        GUIObject = GameObject.FindWithTag("GUI");
        if (GUIObject != null) {
            menu = GUIObject.GetComponent<GUIHandler>();
        }
        else {
            Debug.Log("gui missing");
        }
    }

    void Update() {
        if(!applied) {
            player.hitPoints++;
            currentHitPoints = player.hitPoints;
            menu.updateKneepadCounter(1);
            applied = true;
        }
        if(applied && player.hitPoints < currentHitPoints) {
            menu.updateKneepadCounter(-1);
            Destroy(gameObject);
        }
    }
}
