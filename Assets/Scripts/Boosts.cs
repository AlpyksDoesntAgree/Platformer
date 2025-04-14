using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boosts : MonoBehaviour
{
    private PlayerController player;
    public enum boostType
    {
        doubleJump,
        score,
    }
    [SerializeField] private boostType type;
    void Start()
    {
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
                PlayerPrefs.Save();
                Destroy(gameObject);
                break;
            case boostType.score:
                ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
                scoreManager.AddScore(50);
                Destroy(gameObject);
                break;
        }
    }
}
