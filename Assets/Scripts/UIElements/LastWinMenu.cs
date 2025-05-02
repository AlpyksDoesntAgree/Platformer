using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastWinMenu : MonoBehaviour
{
    [SerializeField] private Text firstScoreText;
    [SerializeField] private Text secondScoreText;
    [SerializeField] private Text thirdScoreText;
    [SerializeField] private Text totalScoreText;
    [SerializeField] private Text StarsCollectedText;
    private Animator animator;
    private PlayerController playerController;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0f;
        firstScoreText.text = $"First Lvl:{PlayerPrefs.GetInt("firstScore",0)}";
        secondScoreText.text = $"Second Lvl:{PlayerPrefs.GetInt("secondScore",0)}";
        thirdScoreText.text = $"Third Lvl:{PlayerPrefs.GetInt("thirdScore", 0)}";
        StarsCollectedText.text = $": {PlayerPrefs.GetInt("ScoreCollected", 0)}";
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    public void WinGame()
    {
        AudioController.Instance.PlaySound(5);
        animator.speed = 1f;
        animator.Play("SlideDown", 0, 0f);
        playerController.isMovingEnabled = false;
        totalScoreText.text = $"Total:{PlayerPrefs.GetInt("totalScore")}";
    }
}
