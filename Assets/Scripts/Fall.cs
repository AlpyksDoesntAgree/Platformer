using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fall : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D playerRb;
    private Collider2D playerCollider;
    private PlayerController playerController;
    private bool damaged = false;
    [SerializeField] private Vector3 checkPoint;
    [SerializeField] private int minY;
    [SerializeField] private float _speed;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        playerCollider = GameObject.Find("Player").GetComponent<Collider2D>();
    }
    void Update()
    {
        if (player.position.y <= minY)
        {
            if (!damaged)
            { playerController.Damage(1); damaged = !damaged;  _speed = 2f; playerController.isMovingEnabled = false; StartCoroutine(Falling()); }
        }
    }

    IEnumerator Falling()
    {
        playerRb.bodyType = RigidbodyType2D.Kinematic;
        playerRb.velocity = Vector3.zero;
        playerCollider.enabled = false;
        playerController.isMovingEnabled=false;

        while (Vector3.Distance(player.position, checkPoint) > 1f)
        {
            player.position = Vector3.Lerp(player.position, checkPoint, _speed * Time.deltaTime);
            yield return null;
        }
        player.position = checkPoint;

        if(player.position == checkPoint)
        {
            playerController.isMovingEnabled = true;
            playerRb.bodyType = RigidbodyType2D.Dynamic;

            yield return new WaitForSeconds(0.5f);
            playerCollider.enabled = true;
            damaged = !damaged;
        }
    }
}
