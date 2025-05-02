using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdController : MonoBehaviour
{
    //Bird
    [HideInInspector] public bool canThrowBird;
    [SerializeField] private GameObject bird;
    private BirdMovement _birdMovement;
    private GameObject currentBird;

    //Transforms&Vector
    [HideInInspector] public Vector2 birdDirection = Vector2.right;
    private Transform playerPos;
    private Transform spawnPoint;

    //Bools
    private bool birdThrown = false;
    [HideInInspector] public bool moveToBird = false;
    private bool hasUIElement = false;

    [SerializeField] private GameObject UIElement;

    //Time
    [SerializeField] private Text cdText;
    private float cooldownTime = 5f;
    private float cooldownTimer = 0f;
    private bool isCooldown = false;

    void Start()
    {
        _birdMovement = bird.GetComponent<BirdMovement>();
        spawnPoint = GetComponent<Transform>();
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        cdText.gameObject.SetActive(false);
        canThrowBird = PlayerPrefs.GetInt("HasBird", 0) == 1;
        hasUIElement = PlayerPrefs.GetInt("HasBird", 0) == 1;
        if (hasUIElement)
            UIElement.SetActive(true);
        else
            UIElement.SetActive(false);
    }
    void Update()
    {
        if (canThrowBird)
        {
            if (!isCooldown && Input.GetKeyDown(KeyCode.E))
            {
                if (!birdThrown)
                {
                    birdDirection = playerPos.localScale.x > 0 ? Vector2.right : Vector2.left;
                    currentBird = Instantiate(bird, spawnPoint.position, Quaternion.identity);
                    birdThrown = true;
                }
                else if(currentBird != null)
                {
                    playerPos.position = currentBird.transform.position;
                    Destroy(currentBird);
                    birdThrown = false;
                    StartCooldown();
                }
                else
                {
                    birdThrown = false;
                    StartCooldown();
                }
            }
            if (isCooldown)
            {
                Cooldown();
            }
        }
    }

    public void StartCooldown()
    {
        isCooldown = true;
        birdThrown = false;
        moveToBird = false;
        cooldownTimer = cooldownTime;
        cdText.gameObject.SetActive(true);

        if (currentBird != null)
        {
            BirdMovement birdScript = currentBird.GetComponent<BirdMovement>();
            if (birdScript != null)
                birdScript.ReturnControl();
        }

        Camera.main.GetComponent<CameraController>().target = playerPos;
    }

    private void Cooldown()
    {
        cooldownTimer -= Time.deltaTime;
        cdText.text = Mathf.CeilToInt(cooldownTimer).ToString();

        if (cooldownTimer <= 0)
        {
            isCooldown = false;
            cdText.gameObject.SetActive(false);
        }
    }
}
