using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawblade : MonoBehaviour
{
    [SerializeField] private float _distance;
    [SerializeField] private float _speed = 0.012f;

    private Transform block;
    private bool movingRight = true;
    private Vector3 startPos;

    private Transform player;
    private PlayerController playerCon;
    void Start()
    {
        block = GetComponent<Transform>();

        player = GameObject.Find("Player").GetComponent<Transform>();
        playerCon = GameObject.Find("Player").GetComponent<PlayerController>();

        startPos = block.position;
    }

    void Update()
    {
        Debug.Log("isRight:" + movingRight);
        transform.Rotate(0,0, 280f * Time.deltaTime);

        if (block.position.x >= startPos.x + _distance)
            movingRight = false;
        else if (block.position.x <= startPos.x)
            movingRight = true;

        if (movingRight)
            block.position = Vector3.MoveTowards(block.position, new Vector3(startPos.x + _distance, startPos.y, startPos.z), _speed);
        else
            block.position = Vector3.MoveTowards(block.position, new Vector3(startPos.x - _distance, startPos.y, startPos.z), _speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerCon.Damage(1);
    }
}
