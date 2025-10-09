using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GUIHandler : MonoBehaviour {

    //public GameObject menu;
    public GameObject enterButton;
    public GameObject menuButtons;
    public GameObject scoreCounter;
    public GameObject mushroomGUI;
    public GameObject kneepadGUI;
    private TextMeshProUGUI kneepadText;
    private TextMeshProUGUI mushroomText;
    private TextMeshProUGUI scoreText;
    private bool sceneLoaded = false;
    private bool gamePaused = false;
    private bool died = false;
    private int playerScore = 0;
    private int kneepadCounter = 0;
    public float mushroomTimer = 0;
    void Start() {
        DontDestroyOnLoad(this.gameObject);
        scoreText = scoreCounter.GetComponentInChildren<TextMeshProUGUI>();
        mushroomText = mushroomGUI.GetComponentInChildren<TextMeshProUGUI>();
        kneepadText = kneepadGUI.GetComponentInChildren<TextMeshProUGUI>();
        scoreCounter.SetActive(false);
        mushroomGUI.SetActive(false);
        kneepadGUI.SetActive(false);
    }

    void Update() {
        showPauseMenu();
    }

    public void enterGame() {
        //loads game
        if(!sceneLoaded) {
            menuButtons.SetActive(false);
            mushroomGUI.SetActive(false);
            kneepadGUI.SetActive(false);
            scoreCounter.SetActive(true);
            SceneManager.LoadScene("SampleScene");
            sceneLoaded = true;
            Debug.Log("scene initially loaded");
        }
        else if (!died) {
            menuButtons.SetActive(false);
            Time.timeScale = 1;
            gamePaused = false;
            Debug.Log("game unpaused");
        }
        else {
            menuButtons.SetActive(false);
            mushroomGUI.SetActive(false);
            kneepadGUI.SetActive(false);
            SceneManager.LoadScene("SampleScene");
            Time.timeScale = 1;
            gamePaused = false;
            died = false;
            Debug.Log("game reloaded after death");
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
    public void activateMushroomTimer() {
        mushroomGUI.SetActive(true);
        mushroomTimer = 10;
    }

    public void updateMushroomTimer() {
        if (mushroomTimer > 0) {
            mushroomTimer -= Time.deltaTime;
            mushroomText.text = ": " + ((int)(mushroomTimer)).ToString();
        }
        else {
            mushroomGUI.SetActive(false);
        }
    }
    public void updateKneepadCounter(int x) {
        kneepadCounter += x;
        if (kneepadCounter > 0) {
            kneepadGUI.SetActive(true);
            kneepadText.text = "X " + kneepadCounter.ToString();
        }
        else {
            kneepadGUI.SetActive(false);
        }
    }
}