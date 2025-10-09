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
    public GameObject hourglassGUI;
    public float mushroomTimer = 0;
    private TextMeshProUGUI hourglassText;
    private TextMeshProUGUI kneepadText;
    private TextMeshProUGUI mushroomText;
    private TextMeshProUGUI scoreText;
    private bool sceneLoaded = false;
    private bool gamePaused = false;
    private bool died = false;
    private int playerScore = 0;
    private int kneepadCounter = 0;
    public bool cameraMoving = true;
    public float hourglassTimer = 0;

    void Start() {
        DontDestroyOnLoad(this.gameObject);
        scoreText = scoreCounter.GetComponentInChildren<TextMeshProUGUI>();
        mushroomText = mushroomGUI.GetComponentInChildren<TextMeshProUGUI>();
        kneepadText = kneepadGUI.GetComponentInChildren<TextMeshProUGUI>();
        hourglassText = hourglassGUI.GetComponentInChildren<TextMeshProUGUI>();
        scoreCounter.SetActive(false);
        mushroomGUI.SetActive(false);
        kneepadGUI.SetActive(false);
        hourglassGUI.SetActive(false);
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
            hourglassGUI.SetActive(false);
            scoreCounter.SetActive(true);
            SceneManager.LoadScene("SampleScene");
            sceneLoaded = true;
        }
        else if (!died) {
            menuButtons.SetActive(false);
            Time.timeScale = 1;
            gamePaused = false;
            Debug.Log("Test");
        }
        else {
            menuButtons.SetActive(false);
            mushroomGUI.SetActive(false);
            kneepadGUI.SetActive(false);
            hourglassGUI.SetActive(false);
            SceneManager.LoadScene("SampleScene");
            Time.timeScale = 1;
            gamePaused = false;
            died = false;
        }
        updateScore(0);
        kneepadCounter = 0;
        cameraMoving = true;
        hourglassTimer = 0;
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
            //math & rounding so it only shows two decimal places
            mushroomText.text = ": " + (Mathf.Round(mushroomTimer * 100)/100).ToString();
        }
        else {
            mushroomGUI.SetActive(false);
        }
    }
    public void updateKneepadCounter(int x) {
        kneepadCounter += x;
        if (kneepadCounter > 0) {
            kneepadGUI.SetActive(true);
            kneepadText.text = "X " + (Mathf.Round(hourglassTimer * 100) / 100).ToString();
        }
        else {
            kneepadGUI.SetActive(false);
        }
    }

    public void updateHourglassTimer(int x) {
        hourglassTimer += x;
        hourglassGUI.SetActive(true);
        hourglassText.text = ": " + hourglassTimer.ToString();
    }

    public void updateHourglassTimer() {
        hourglassTimer -= Time.deltaTime;
        if (hourglassTimer <= 0) {
            hourglassGUI.SetActive(false);
            cameraMoving = true;
        }
        else {
            hourglassGUI.SetActive(true);
        }

        hourglassText.text = ": " + (Mathf.Round(hourglassTimer * 100) / 100).ToString();
    }
}