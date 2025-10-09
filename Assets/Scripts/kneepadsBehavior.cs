using UnityEditor;
using UnityEngine;

public class kneepadsBehavior : MonoBehaviour {
    private playerController player;
    private bool applied = false;
    private int currentHitPoints;
    void Start() {
        player = GetComponentInParent<playerController>();
    }

    void Update() {
        if(!applied) {
            player.hitPoints++;
            currentHitPoints = player.hitPoints;
            GUIHandler.instance.updateKneepadCounter(1);
            applied = true;
        }
        if(applied && player.hitPoints < currentHitPoints) {
            GUIHandler.instance.updateKneepadCounter(-1);
            Destroy(gameObject);
        }
    }
}
