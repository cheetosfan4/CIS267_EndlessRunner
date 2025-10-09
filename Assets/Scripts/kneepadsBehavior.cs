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
            applied = true;
        }
        if(applied && player.hitPoints < currentHitPoints) {
            Destroy(gameObject);
        }
    }
}
