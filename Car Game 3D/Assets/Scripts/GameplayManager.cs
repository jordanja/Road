using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour {

    public static GameplayManager instance;

    [SerializeField]
    TextMeshProUGUI ScoreText;

    [SerializeField]
    TextMeshProUGUI HighScoreText;

    int currentScore;

    string defaultScoreText = "Score: ";
    string defaultHighScoreText = "Highscore: ";

    string highScoreKey = "HighScore";

    void Awake() {
        instance = this;
        currentScore = 0;
        updateScoreText();
        updateHightScoreText();

    }

    public void IncreaseScore() {
        currentScore += 1;
        updateScoreText();
    }

    private void updateHightScoreText() {
        HighScoreText.text = defaultHighScoreText + PlayerPrefs.GetInt(highScoreKey, 0).ToString();
    }

    private void updateScoreText() {
        ScoreText.text = defaultScoreText + currentScore.ToString();
    }

    public void RestartGame() {
        if (currentScore > PlayerPrefs.GetInt(highScoreKey, 0)) {
            PlayerPrefs.SetInt(highScoreKey,currentScore);
            updateHightScoreText();
            
        }
        SceneManager.LoadScene(0);
    }

}
