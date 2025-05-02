using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectlie : MonoBehaviour
{
    private Vector2 direction;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 5f;

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Damage(1);
            Destroy(gameObject);
        }
    }
}
