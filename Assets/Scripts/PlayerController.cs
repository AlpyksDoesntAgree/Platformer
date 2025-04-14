using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public string nameScene;
    //UI
    [SerializeField] private GameObject heart;
    [SerializeField] private Transform playerUI;
    private int _health = 3;
    private CameraController cam;

    //PlayerControl
    private Rigidbody2D rb;
    private float speed = 5.0f;
    [SerializeField] private Animator animator;
    private bool isGrounded;
    [SerializeField]private LayerMask groundLayer;
    private float jumpForse = 6.0f;
    [HideInInspector] public bool isMovingEnabled = true;

    //DoubleJump
    [HideInInspector] public bool doubleJump;
    private bool jumped = false;

    void Start()
    {
        updateUI();
        nameScene = PlayerPrefs.GetString("NameScene", "StarterLvl");
        doubleJump = PlayerPrefs.GetInt("HasDoubleJump", 0) == 1;
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //Moving
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        if (isMovingEnabled)
        {
            if (move > 0) { transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); cam.offset = new Vector3(-0.1f, 0, 0); }
            if (move < 0) { transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); cam.offset = new Vector3(0.1f, 0, 0); }
        }

        //isGrounded
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.4f, groundLayer);

        //Animation
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", move != 0);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", true);
        }

        //Jump and double jump
        if (isGrounded)  { jumped = false; }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForse);
        }
        if (Input.GetKeyDown(KeyCode.W) && !isGrounded && doubleJump && !jumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForse);
            jumped = true;
        }

        //Death
        if(_health <= 0)
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(nameScene);
        }
    }

    public void Damage(int i)
    {
        _health -= i;
        updateUI();
    }
    private void updateUI()
    {
        foreach (Transform child in playerUI.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _health; i++)
        {
            Instantiate(heart, new Vector3(playerUI.position.x + 1.15f * i, playerUI.position.y), Quaternion.identity, playerUI);
        }
    }
}
