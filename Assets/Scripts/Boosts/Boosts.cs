using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boosts : MonoBehaviour
{
    int currentCollected;
    private PlayerController player;
    private BirdController birdController;
    public enum boostType
    {
        doubleJump,
        score,
        bird,
        heal,
    }
    [SerializeField] private boostType type;
    [SerializeField] private GameObject UIElement;
    void Start()
    {
        birdController = GameObject.Find("BirdSpawner").GetComponent<BirdController>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            GetBoost(type);
    }

    public void GetBoost(boostType boost)
    {
        switch (boost)
        {
            case boostType.doubleJump:
                player.doubleJump = true;
                PlayerPrefs.SetInt("HasDoubleJump", 1);
                break;
            case boostType.score:
                ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
                scoreManager.AddScore(50);
                currentCollected = PlayerPrefs.GetInt("ScoreCollected", 0);
                currentCollected++;
                PlayerPrefs.SetInt("ScoreCollected", currentCollected);
                break;
            case boostType.bird:
                UIElement.SetActive(true);
                birdController.canThrowBird = true; 
                PlayerPrefs.SetInt("HasBird", 1);
                break;
            case boostType.heal:
                PlayerPrefs.SetInt("HasHeal", 1);
                break;
        }
        PlayerPrefs.Save();
        Destroy(gameObject);
    }
}
