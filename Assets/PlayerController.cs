using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction jumpAction;

    public float moveSpeed = 8f;
    public float jumpForce = 16f;
    public float jumpCutMultiplier = 0.5f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;


    public float playerHealth;
    public float enemyDamage;
    public Image healthSprite;

    public Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        horizontalInput = moveAction.ReadValue<float>();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && jumpAction.WasPressedThisFrame())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (jumpAction.WasReleasedThisFrame() && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        UpdateHealth();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if(horizontalInput > 0.1f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            animator.Play("walk");
        }
        else if(horizontalInput < -0.1f)
        {
            animator.Play("walk");

            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if(horizontalInput == 0f)
        {
            animator.Play("idle");
        }
    }


    public void UpdateHealth()
    {
        healthSprite.fillAmount = playerHealth / 3f;

        if (playerHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            playerHealth -= enemyDamage * Time.deltaTime;
            animator.Play("hurt");

        }
    }
}