using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //Distaces
    private Transform player;
    private Transform startPoint;
    [SerializeField] private Transform endPoint;
    private int lastScoreDistance = 0;
    private float distanceTravelled;

    //Scores & Texts
    [SerializeField] private string currentScene;
    private Text currentScoreText;
    [SerializeField] private Text totalScoreText;

    //Scores
    private int firstScore;
    private int secondScore;
    private int thirdScore;
    private int totalScore;

    private string curScore;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        startPoint = new GameObject("StartPoint").transform;
        startPoint.position = player.position;
        currentScoreText = GetComponent<Text>();
        GetScene(currentScene);
        totalScore = PlayerPrefs.GetInt("totalScore", 0);
    }

    void Update()
    {
        distanceTravelled = player.position.x - startPoint.position.x;

        if (distanceTravelled > 0)
        {
            int intDistance = Mathf.FloorToInt(distanceTravelled);

            if (intDistance > lastScoreDistance && player.position.x <= endPoint.position.x)
            {
                int delta = intDistance - lastScoreDistance;
                lastScoreDistance = intDistance;
                AddScore(delta * 2);
            }
        }
    }

    public void AddScore(int score)
    {
        switch(curScore)
        {
            case "firstScore":
                firstScore += score;
                currentScoreText.text = "Score: " + firstScore.ToString();
                totalScore += score;
                totalScoreText.text = "Total: " + totalScore.ToString();
                PlayerPrefs.SetInt("firstScore", firstScore);
                PlayerPrefs.SetInt("totalScore", totalScore);
                PlayerPrefs.Save();
                break;
            case "secondScore":
                secondScore += score;
                currentScoreText.text = "Score: " + secondScore.ToString();
                totalScore += score;
                totalScoreText.text = "Total: " + totalScore.ToString();
                PlayerPrefs.SetInt("secondScore", secondScore);
                PlayerPrefs.SetInt("totalScore", totalScore);
                PlayerPrefs.Save();
                break;
            case "thirdScore":
                thirdScore += score;
                currentScoreText.text = "Score: " + thirdScore.ToString();
                totalScore += score;
                totalScoreText.text = "Total: " + totalScore.ToString();
                PlayerPrefs.SetInt("thirdScore", thirdScore);
                PlayerPrefs.SetInt("totalScore", totalScore);
                PlayerPrefs.Save();
                break;
        }
    }
    
    private string GetScene(string currentScene)
    {
        switch(currentScene)
        {
            case "StarterLvl":
                firstScore = PlayerPrefs.GetInt("firstScore", 0);
                curScore = "firstScore";
                break;
            case "Underground":
                secondScore = PlayerPrefs.GetInt("secondScore", 0);
                curScore = "secondScore";
                break;
            case "ThirdLevel":
                secondScore = PlayerPrefs.GetInt("secondScore", 0);
                curScore = "thirdScore";
                break;
        }
        return curScore;
    }
}
