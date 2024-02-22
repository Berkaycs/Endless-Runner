using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const string IS_SLIP = "IsSlip";
    private const string IS_DEAD = "IsDead";
    private const string IS_RUNNING = "StartRunAnim";
    private const string IS_RESPAWNING = "IsRespawn";

    [SerializeField] private Animator animator;
    [SerializeField] private Transform respawnPoint;
    //private Renderer playerRenderer;
    private Rigidbody rb;

    public float forwardSpeed = 50;
    public float sideSpeed = 10;
    public float maxSpeed;

    public float middleRight = 13.5f;
    public float middleLeft = -13.5f;

    public float jumpForce = 20;
    public float doubleJumpForce = 20;

    public float gravityModifier;

    public float respawnDelay = 3f;
    public float blinkInterval = 0.5f;
    public Color blinkColor = Color.white;

    public float numberOfDeaths = 0;

    public bool isOnGround = true;
    public bool doubleJump = false;
    public bool isAlive = true;
    public bool canDie = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //playerRenderer = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        Physics.gravity *= gravityModifier;
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        
        IncreaseSpeedBySeconds();
        HandleMovement();
        Jump();
        Slip();
    }

    void HandleMovement()
    {
        transform.Translate(0, 0, forwardSpeed * Time.deltaTime);

        Vector3 toLeft = new Vector3(middleLeft, transform.position.y, transform.position.z);
        Vector3 toRight = new Vector3(middleRight, transform.position.y, transform.position.z);

        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("lerping to left");
            transform.position = Vector3.Lerp(transform.position, toLeft, sideSpeed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("lerping to right");
            transform.position = Vector3.Lerp(transform.position, toRight, sideSpeed * Time.deltaTime);
        }

        else
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }

    void Jump()
    {
        if (isOnGround && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool(IS_RUNNING, false);
            Debug.Log("First jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            doubleJump = true;
        }

        else if (!isOnGround && doubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool(IS_RUNNING, false);
            doubleJump = false;
            Debug.Log("Second jump");
            rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
        }
    }

    void Slip()
    {
        if (isOnGround && Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetBool(IS_SLIP, true);
        }

        else
        {
            animator.SetBool(IS_SLIP, false);
        }
    }

    void IncreaseSpeedBySeconds()
    {
        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.2f * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool(IS_RUNNING, true);
            isOnGround = true;
            doubleJump = false;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isAlive = false;

            if (numberOfDeaths < 3)
            {
                numberOfDeaths++;
                animator.SetBool(IS_DEAD, true);
                Respawn();
            }

            if (numberOfDeaths > 3)
            {
                GameManager.Instance.isGameOver = true;
                GameManager.Instance.GameOver();
            }
        }
    }

    private void Respawn()
    {
        transform.position = respawnPoint.transform.position;
        animator.SetBool(IS_RESPAWNING, true); 
        isAlive = true;
        animator.SetBool(IS_RUNNING, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SilverCoin"))
        {
            GameManager.Instance.score += 3;
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("GoldCoin"))
        {
            GameManager.Instance.score += 5;
            other.gameObject.SetActive(false);
        }
    }
}
