using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIHandler : MonoBehaviour {

    public GameObject menu;
    public GameObject enterButton;
    private bool sceneLoaded = false;
    private bool gamePaused = false;
    void Start() {

    }

    void Update() {
        showPauseMenu();
    }

    public void enterGame() {
        //loads game
        if(!sceneLoaded) {
            DontDestroyOnLoad(this.gameObject);
            menu.SetActive(false);
            SceneManager.LoadScene("SampleScene");
            sceneLoaded = true;
        }
        else {
            menu.SetActive(false);
            Time.timeScale = 1;
            gamePaused = false;
        }
    }

    public void exitGame() {
        //only works on exported build
        Application.Quit();
    }

    public void loadScores() {

    }

    private void showPauseMenu() {
        if(Input.GetKeyDown(KeyCode.P) && sceneLoaded) {
            if(!gamePaused) {
                menu.SetActive(true);
                enterButton.GetComponentInChildren<TextMeshProUGUI>().text = "Resume";
                Time.timeScale = 0;
                gamePaused = true;
            }
            else {
                menu.SetActive(false);
                Time.timeScale = 1;
                gamePaused = false;
            }
        }
    }
}
