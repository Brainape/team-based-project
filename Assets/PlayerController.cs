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
    public PointSystem pointSystem;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        // Initialize player health, makes sure rigidbody doesn't rotate when colliding with objects
    }
    private void Start()
    {
        pointSystem = FindAnyObjectByType<PointSystem>();
    }
    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        //makes sure the input actions are enabled when the script is enabled
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        //makes sure the input actions are disabled when the script is disabled
    }

    private void Update()
    {
        horizontalInput = moveAction.ReadValue<float>();
        //reads the horizontal input from the player
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        //checks if the player is grounded by creating a small circle at the groundCheck position

        if (isGrounded && jumpAction.WasPressedThisFrame())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            //if the player is grounded and the jump button is pressed, the player jumps
        }

        if (jumpAction.WasReleasedThisFrame() && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            //if the jump button is released and the player is moving upwards, the player's upward velocity is reduced
        }

        UpdateHealth();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        //sets the player's horizontal velocity based on the horizontal input and move speed
        if (horizontalInput > 0.1f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            animator.Play("walk");
            //if the player is moving right, the player's sprite is flipped to face right and the walk animation is played
        }
        else if(horizontalInput < -0.1f)
        {
            animator.Play("walk");

            transform.localScale = new Vector3(1f, 1f, 1f);
            //if the player is moving left, the player's sprite is flipped to face left and the walk animation is played
        }

        if (horizontalInput == 0f)
        {
            animator.Play("idle");
            //if the player is not moving, the idle animation is played
        }
    }


    public void UpdateHealth()
    {
        healthSprite.fillAmount = playerHealth / 3f;
        //updates the health bar based on the player's current health
        if (playerHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //if the player's health is 0 or less, reloads the current scene
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            playerHealth -= enemyDamage * Time.deltaTime;
            animator.Play("hurt");
            //if the player is colliding with an enemy, the player's health is reduced by the enemy's damage over time and the hurt animation is played

        }

    }

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("badpizza"))
        {


            animator.Play("hurt");
            playerHealth -= enemyDamage;
            pointSystem.points += 1;
            Destroy(collision.gameObject);

        }

        if (collision.gameObject.CompareTag("good pizza"))
        {

            pointSystem.points += 1;
            playerHealth += 5;
            Destroy(collision.gameObject);


        }
    }
}