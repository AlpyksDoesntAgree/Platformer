using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    private CameraController cam;
    private Transform playerTransform;
    private PlayerController playerController;
    private Transform birdTransform;
    private BirdController birdController;

    private Boss boss;

    private Vector2 direction;

    private float flyDuration = 2f;
    private float timer;

    void Start()
    {
        birdController = GameObject.Find("BirdSpawner").GetComponent<BirdController>();
        direction = GameObject.Find("BirdSpawner").GetComponent<BirdController>().birdDirection;
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        birdTransform = transform;

        if (GameObject.Find("BossDragon"))
            boss = GameObject.Find("BossDragon").GetComponent<Boss>();

        playerController.isMovingEnabled = false;
        cam.target = birdTransform;
        timer = flyDuration;

        if (playerTransform.localScale.x > 0)
            birdTransform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else 
            birdTransform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            birdController.StartCooldown();
        }

        birdTransform.Translate(direction * 5f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && collision.gameObject.name != "DangerArea")
        {
            if (collision.CompareTag("Boss"))
            {
                boss.TakeDamage(1);
            }

            birdController.StartCooldown();
        }
    }

    public void ReturnControl()
    {
        playerController.isMovingEnabled = true;
        if(gameObject != null)
            Destroy(gameObject);
    }
}
