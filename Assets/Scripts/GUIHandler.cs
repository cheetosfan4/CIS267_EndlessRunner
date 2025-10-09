using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIHandler : MonoBehaviour {

    //public GameObject menu;
    public GameObject enterButton;
    public GameObject menuButtons;
    public GameObject scoreCounter;
    private TextMeshProUGUI scoreText;
    private bool sceneLoaded = false;
    private bool gamePaused = false;
    private bool died = false;
    private int playerScore = 0;
    void Start() {
        DontDestroyOnLoad(this.gameObject);
        scoreCounter.SetActive(false);
        scoreText = scoreCounter.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update() {
        showPauseMenu();
    }

    public void enterGame() {
        //loads game
        if(!sceneLoaded) {
            menuButtons.SetActive(false);
            scoreCounter.SetActive(true);
            SceneManager.LoadScene("SampleScene");
            sceneLoaded = true;
        }
        else if (!died) {
            menuButtons.SetActive(false);
            Time.timeScale = 1;
            gamePaused = false;
        }
        else {
            menuButtons.SetActive(false);
            SceneManager.LoadScene("SampleScene");
            Time.timeScale = 1;
            gamePaused = false;
            died = false;
        }
        updateScore(0);
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
                menuButtons.SetActive(true);
                enterButton.GetComponentInChildren<TextMeshProUGUI>().text = "Resume";
                Time.timeScale = 0;
                gamePaused = true;
            }
            else {
                menuButtons.SetActive(false);
                Time.timeScale = 1;
                gamePaused = false;
            }
        }
    }

    public void uponDeath() {
        died = true;
        menuButtons.SetActive(true);
        enterButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retry";
        Time.timeScale = 0;
        gamePaused = true;
    }

    public void updateScore(int score) {
        if(score == 0) {
            playerScore = 0;
        }
        else if (!died) {
            playerScore += score;
        }
        scoreText.text = ("SCORE: " + playerScore.ToString());
    }
}
