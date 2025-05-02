using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    //UI
    private TotalScoreText TotalScoreText;
    private LastWinMenu LastWinMenu;
    // Player
    private Transform playerPos;
    private PlayerController playerCon;

    // Projectile
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootInterval = 3f;
    private Transform projectileSpawner;

    // Boss
    [HideInInspector] public int _health = 1;
    [HideInInspector] public bool playerInZone = false;
    private float shootTimer = 0f;

    void Start()
    {
        playerCon = GameObject.Find("Player").GetComponent<PlayerController>();
        playerPos = GameObject.Find("Player").transform;
        projectileSpawner = GameObject.Find("ProjectlieSpawner").transform;
        TotalScoreText = GameObject.Find("TotalScore").GetComponent<TotalScoreText>();
        LastWinMenu = GameObject.Find("WinPanel").GetComponent<LastWinMenu>();
    }

    void Update()
    {
        if (_health <= 0)
        {
            TotalScoreText.updateTotalScore();
            LastWinMenu.WinGame();
            Destroy(gameObject);
        }

        if (playerInZone)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                SpawnProjectile();
                shootTimer = shootInterval;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerCon.Damage(3);
    }

    public void SpawnProjectile()
    {
        if (projectilePrefab != null && projectileSpawner != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawner.position, Quaternion.identity);

            Vector2 direction = (playerPos.position - projectileSpawner.position).normalized;

            Projectlie projectileScript = projectile.GetComponent<Projectlie>();
            if (projectileScript != null)
                projectileScript.SetDirection(direction);
        }
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
    }
}
