using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 5.0f;
    [SerializeField] private Animator animator;
    private bool isGrounded;
    [SerializeField]private LayerMask groundLayer;
    private float jumpForse = 7.0f;

    private CameraController cam;

    private int _health = 3;
    private bool doubleJump;
    private bool jumped = false;

    void Start()
    {
        doubleJump = PlayerPrefs.GetInt("HasDoubleJump", 1) == 1;
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //Moving
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        if (move > 0) { transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); cam.offset = new Vector3(-0.1f, 0, 0); }
        if (move < 0) { transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); cam.offset = new Vector3(0.1f, 0, 0); }

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
    }

    public void Damage(int i)
    {
        _health -= 0;
    }
}
