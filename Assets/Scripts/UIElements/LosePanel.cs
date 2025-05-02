using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        animator.speed = 0f;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    public void LoseGame()
    {
        animator.enabled = true;
        AudioController.Instance.PlaySound(4);
        animator.speed = 1f;
        animator.Play("SlideDown");
        playerController.isMovingEnabled = false;
    }
}
