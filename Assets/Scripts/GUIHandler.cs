using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIHandler : MonoBehaviour {

    public GameObject menu;
    public GameObject enterButton;
    private bool sceneLoaded = false;
    private bool gamePaused = false;
    private bool died = false;
    void Start() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
        showPauseMenu();
    }

    public void enterGame() {
        //loads game
        if(!sceneLoaded) {
            menu.SetActive(false);
            SceneManager.LoadScene("SampleScene");
            sceneLoaded = true;
        }
        else if (!died) {
            menu.SetActive(false);
            Time.timeScale = 1;
            gamePaused = false;
        }
        else {
            menu.SetActive(false);
            SceneManager.LoadScene("SampleScene");
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

    public void uponDeath() {
        died = true;
        menu.SetActive(true);
        enterButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retry";
        Time.timeScale = 0;
        gamePaused = true;
    }
}
