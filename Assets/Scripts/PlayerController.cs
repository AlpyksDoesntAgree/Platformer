using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public string nameScene;
    //UI
    [SerializeField] private GameObject heart;
    [SerializeField] private Transform playerUI;
    [HideInInspector] public int _health = 3;
    private LosePanel LosePanel;
    private CameraController cam;

    //PlayerControl
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float speed = 5.0f;
    [SerializeField] private Animator animator;

    //Jumps
    private bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private float groundCheckDistance = 0.1f;
    private float jumpForse = 6.0f;

    [HideInInspector] public bool isMovingEnabled = true;

    //DoubleJump
    [HideInInspector] public bool doubleJump;
    private bool jumped = false;

    //Damage
    [HideInInspector] public bool damaged = false;

    private bool isStepped = false;

    void Start()
    {
        LosePanel = GameObject.Find("LosePanel").GetComponent<LosePanel>();
        _health = PlayerPrefs.GetInt("Health", 3);
        nameScene = PlayerPrefs.GetString("NameScene", "StarterLvl");
        doubleJump = PlayerPrefs.GetInt("HasDoubleJump", 0) == 1;
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        updateUI();
        AudioController.Instance.PlayMusic();
    }
    void Update()
    {
        //Moving
        float move = Input.GetAxisRaw("Horizontal");
        if (isMovingEnabled)
            rb.velocity = new Vector2(move * speed, rb.velocity.y);

        if (move > 0) { transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); cam.offset = new Vector3(-0.1f, 0, 0); }
        if (move < 0) { transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); cam.offset = new Vector3(0.1f, 0, 0); }


        //isGrounded
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        //Animation
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", move != 0);
            if(!isStepped && move != 0)
            StartCoroutine(PlayStepWithPause());
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", true);
        }

        //Jump and double jump
        if (isGrounded) { jumped = false; }

        if (isMovingEnabled)
        {
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                Jump(jumpForse);
            }
            if (Input.GetKeyDown(KeyCode.W) && !isGrounded && doubleJump && !jumped)
            {
                Jump(jumpForse);
                jumped = true;
            }
        }
        //Death
        if (_health <= 0)
        {
            PlayerPrefs.DeleteAll();
            LosePanel.LoseGame();
            damaged = true;
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
    }
    public void Jump(float jumpForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        AudioController.Instance.PlaySound(1);
    }
    public void FallDamage(int i)
    {
        _health -= i;
        AudioController.Instance.PlaySound(3);
        updateUI();
    }
    public void Damage(int i)
    {
        if (!damaged)
        {
            _health -= i;
            AudioController.Instance.PlaySound(3);
            updateUI();
            StartCoroutine(WaitDamaged());
        }
    }
    public void updateUI()
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
    IEnumerator WaitDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        damaged = !damaged;
        yield return new WaitForSeconds(1.5f);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        damaged = !damaged;
    }
    IEnumerator PlayStepWithPause()
    {
        isStepped = true;
        AudioController.Instance.PlaySound(2);
        animator.SetBool("isRunning", true);
        yield return new WaitForSeconds(0.5f);
        isStepped = false;
    }
}