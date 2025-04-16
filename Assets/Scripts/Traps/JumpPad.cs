using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private Sprite pressedState;
    [SerializeField] private Sprite unpressedState;
    private SpriteRenderer jumppadSprite;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        jumppadSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.Jump(10f);
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        jumppadSprite.transform.position = new Vector2(jumppadSprite.transform.position.x, jumppadSprite.transform.position.y - 0.09f);
        jumppadSprite.sprite = pressedState;

        yield return new WaitForSeconds(0.5f);

        jumppadSprite.transform.position = new Vector2(jumppadSprite.transform.position.x, jumppadSprite.transform.position.y + 0.09f);
        jumppadSprite.sprite = unpressedState;

    }
}
