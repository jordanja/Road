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

    float currentScore;

    string defaultScoreText = "Score: ";

    void Awake() {
        instance = this;
        currentScore = 0;
        updateScoreText();
    }

    public void IncreaseScore() {
        currentScore += 1;
        updateScoreText();
    }

    private void updateScoreText() {
        ScoreText.text = defaultScoreText + currentScore.ToString();
    }

    public void RestartGame() {
        SceneManager.LoadScene(0);
    }

}
