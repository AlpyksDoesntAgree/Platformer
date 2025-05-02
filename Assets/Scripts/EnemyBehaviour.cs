using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    //Player
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundMask;
    private PlayerController playerController;
    private Transform playerPos;

    //Enemy
    [SerializeField] private Animator anim;
    private Transform enemyPos;
    private float _radius = 7.5f;
    private bool isPlayerUpside;
    private bool isGrounded;

    //Raycast
    [SerializeField] private Transform upsideRayCast;
    [SerializeField] private Transform isGroundRayCast;
    void Start()
    {
        enemyPos = GetComponent<Transform>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        float directionToPlayerX = playerPos.position.x - enemyPos.position.x;

        //Sprite Direction
        if (playerPos.position.x <= enemyPos.position.x) { transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); }
        else { transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); }

        //isPlayerUpside
        isPlayerUpside = Physics2D.Raycast(upsideRayCast.position, Vector2.up, 0.15f, playerLayer);
        if (isPlayerUpside) 
        { 
            Destroy(gameObject);
            if (PlayerPrefs.GetInt("HasHeal", 0) == 1 && playerController._health < 3)
            {
                playerController._health++;
                playerController.updateUI();
            }
        }

        //isGrounded
        isGrounded = Physics2D.Raycast(isGroundRayCast.position, Vector2.down, Mathf.Infinity, groundMask);

        if (!isGrounded)
            return;


        //Walking
        if (playerPos.position.x >= enemyPos.position.x - _radius
            && playerPos.position.x <= enemyPos.position.x + _radius)
        {
            enemyPos.position = Vector3.MoveTowards(enemyPos.position, new Vector3(playerPos.position.x,enemyPos.position.y), 2f * Time.deltaTime);
            anim.SetBool("isWalking", true);
        }
        else { anim.SetBool("isWalking", false); }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            playerController.Damage(1);
    }
}
