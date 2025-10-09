using Unity.VisualScripting;
using UnityEngine;

public class mushroomBehavior : MonoBehaviour {
    private playerController player;
    private GameObject GUIObject;
    private GUIHandler menu;
    private bool applied = false;
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
        if (!applied) {
            if (player.moveSpeed > 0) {
                player.moveSpeed = -player.moveSpeed;
                menu.activateMushroomTimer();
                applied = true;
            }
            //if there is an already active mushroom, any new ones will just reset the timer
            else {
                menu.mushroomTimer = 10;
                Destroy(gameObject);
            }
        }
        if (applied && menu.mushroomTimer <= 0) {
            player.moveSpeed = Mathf.Abs(player.moveSpeed);
            Destroy(gameObject);
        }

        menu.updateMushroomTimer();
    }
}
