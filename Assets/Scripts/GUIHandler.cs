//====================================================================================================
//Author        :       Marc McLennan
//Date          :       10-12-2025
//Description   :       CIS267 Homework #1; Endless Runner Game
//====================================================================================================
using System.Data;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GUIHandler : MonoBehaviour {
    //code for singleton
    public static GUIHandler instance { get; private set; }
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
        GUIHandler.instance.scoreCounter.SetActive(false);
    }

    public GameObject enterButton;
    public GameObject menuButtons;
    public GameObject titleGUI;
    public GameObject guideGUI;

    public GameObject scoreCounter;
    private TextMeshProUGUI scoreText;
    public GameObject mushroomGUI;
    private TextMeshProUGUI mushroomText;
    public GameObject kneepadGUI;
    private TextMeshProUGUI kneepadText;
    public GameObject hourglassGUI;
    private TextMeshProUGUI hourglassText;
    public GameObject highscoreGUI;
    public TextMeshProUGUI highscoreGUIText;

    private bool cameraMoving = true;
    private float mushroomTimer = 0;
    private float hourglassTimer = 0;
    private bool startDeletion = false;
    private bool sceneLoaded = false;
    private bool gamePaused = false;
    private bool died = false;
    private int playerScore = 0;
    private int kneepadCounter = 0;


    void Start() {
        DontDestroyOnLoad(this.gameObject);
        scoreText = scoreCounter.GetComponentInChildren<TextMeshProUGUI>();
        mushroomText = mushroomGUI.GetComponentInChildren<TextMeshProUGUI>();
        kneepadText = kneepadGUI.GetComponentInChildren<TextMeshProUGUI>();
        hourglassText = hourglassGUI.GetComponentInChildren<TextMeshProUGUI>();
        mushroomGUI.SetActive(false);
        kneepadGUI.SetActive(false);
        hourglassGUI.SetActive(false);
        highscoreGUI.SetActive(false);
        guideGUI.SetActive(false);
    }

    void Update() {
        showPauseMenu();
    }

    //called when an instance is loaded directly from the gameplay scene
    public void debugMode() {
        menuButtons.SetActive(false);
        scoreCounter.SetActive(true);
        titleGUI.SetActive(false);
        sceneLoaded = true;
    }

    public void enterGame() {
        //loads game from main menu
        if(!sceneLoaded) {
            titleGUI.SetActive(false);
            menuButtons.SetActive(false);
            mushroomGUI.SetActive(false);
            kneepadGUI.SetActive(false);
            hourglassGUI.SetActive(false);
            scoreCounter.SetActive(true);
            SceneManager.LoadScene("SampleScene");
            sceneLoaded = true;
            updateScore(0);
            kneepadCounter = 0;
            cameraMoving = true;
            hourglassTimer = 0;
            startDeletion = false;
}
        //unpauses game when clicking resume
        else if (!died) {
            menuButtons.SetActive(false);
            Time.timeScale = 1;
            gamePaused = false;
        }
        //loads game when clicking retry
        else {
            menuButtons.SetActive(false);
            mushroomGUI.SetActive(false);
            kneepadGUI.SetActive(false);
            hourglassGUI.SetActive(false);
            SceneManager.LoadScene("SampleScene");
            Time.timeScale = 1;
            gamePaused = false;
            died = false;
            updateScore(0);
            kneepadCounter = 0;
            cameraMoving = true;
            hourglassTimer = 0;
            startDeletion = false;
        }
    }

    public void exitGame() {
        //only works on exported build
        Application.Quit();
    }

    public void loadScores() {
        int[] scores = readScores();
        highscoreGUI.SetActive(true);
        highscoreGUIText.text = "1. " + scores[0] + "\n2. " + scores[1] + "\n3. " + scores[2] + "\n4. " + scores[3] + "\n5. " + scores[4];
    }

    public void closeScores() {
        highscoreGUI.SetActive(false);
    }

    public void loadGuide() {
        guideGUI.SetActive(true);
    }

    public void closeGuide() {
        guideGUI.SetActive(false);
    }

    private void showPauseMenu() {
        if(Input.GetKeyDown(KeyCode.P) && sceneLoaded) {
            highscoreGUI.SetActive(false);
            guideGUI.SetActive(false);
            if (!gamePaused) {
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
        writeScore(playerScore);
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

    public void writeScore(int score) {
        int[] scores = readScores();
        int ranking = 5;

        //compares player's score to each of the high scores in the array
        for (int i = 0; i < scores.Length; i++) {
            if (scores[i] < score) {
                ranking--;
            }
        }

        //inserts new score into rankings
        if (ranking < 5) {
            for (int i = 4; i > ranking; i--) {
                //sets current rank to the score above it
                scores[i] = scores[i - 1];
            }

            scores[ranking] = score;

            for (int i = 0; i < scores.Length; i++) {
                PlayerPrefs.SetInt("Highscore" + (i + 1), scores[i]);
            }
        }
    }

    public int[] readScores() {
        int[] scores = new int[5];
        scores[0] = PlayerPrefs.GetInt("Highscore1", 0);
        scores[1] = PlayerPrefs.GetInt("Highscore2", 0);
        scores[2] = PlayerPrefs.GetInt("Highscore3", 0);
        scores[3] = PlayerPrefs.GetInt("Highscore4", 0);
        scores[4] = PlayerPrefs.GetInt("Highscore5", 0);
        return scores;
    } 

    public int getScore(int index) {
        int[] scores = readScores();
        return scores[index];
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
            kneepadText.text = "X " + kneepadCounter.ToString();
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

    //--getters----------------------------------------
    public bool getDeletionStatus() {
        return startDeletion;
    }
    public bool getCameraMoving() {
        return cameraMoving;
    }
    public float getMushroomTimer() {
        return mushroomTimer;
    }
    public float getHourglassTimer() {
        return hourglassTimer;
    }

    //--setters----------------------------------------
    public void setDeletionStatus(bool b) {
        startDeletion = b;
    }
    public void setCameraMoving(bool b) {
        cameraMoving = b;
    }
    public void setMushroomTimer(float f) {
        mushroomTimer = f;
    }
    public void setHourglassTimer(float f) {
        hourglassTimer = f;
    }
}