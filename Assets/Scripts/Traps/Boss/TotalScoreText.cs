using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalScoreText : MonoBehaviour
{
    private int finalScore;
    private Text scoreText;
    void Start()
    {
        finalScore = PlayerPrefs.GetInt("totalScore");
        scoreText = GetComponent<Text>();
        scoreText.text = $"Total: {finalScore}";
    }

    public void updateTotalScore()
    {
        finalScore += 500;
        PlayerPrefs.SetInt("totalScore", finalScore);
        PlayerPrefs.Save();
        scoreText.text = $"Total: {finalScore}";
    }
}
