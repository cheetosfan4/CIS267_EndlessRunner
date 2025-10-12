using Unity.VisualScripting;
using UnityEngine;

public class mushroomBehavior : MonoBehaviour {
    private playerController player;
    private bool applied = false;
    void Start() {
        player = GetComponentInParent<playerController>();
    }

    void Update() {
        if (!applied) {
            if (player.moveSpeed > 0) {
                player.moveSpeed = -player.moveSpeed;
                GUIHandler.instance.activateMushroomTimer();
                applied = true;
            }
            //if there is an already active mushroom, any new ones will just reset the timer
            else {
                GUIHandler.instance.setMushroomTimer(10);
                Destroy(gameObject);
            }
        }
        if (applied && GUIHandler.instance.getMushroomTimer() <= 0) {
            player.moveSpeed = Mathf.Abs(player.moveSpeed);
            Destroy(gameObject);
        }

        GUIHandler.instance.updateMushroomTimer();
    }
}
