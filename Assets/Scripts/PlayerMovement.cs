using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //private const string IS_IDLE = "IsIdle";
    private const string IS_SLIP = "IsSlipping";
    private const string IS_DEAD = "IsDead";
    private const string IS_RUNNING = "IsRunning";
    //private const string IS_RESPAWNING = "IsRespawning";

    [SerializeField] private TextMeshProUGUI healthText;
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

    public int health;
    public int showHealth;

    public bool isOnGround = true;
    public bool doubleJump = false;
    public bool isAlive = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //playerRenderer = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        Physics.gravity *= gravityModifier;
        health = 2;
        showHealth = health + 1;
        healthText.text = "HEALTH: " + showHealth.ToString();
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
            isOnGround = true;
            doubleJump = false;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isAlive = false;
            showHealth--;
            animator.SetBool(IS_DEAD, true);

            if (health == 0)
            {
                healthText.text = "HEALTH: " + showHealth.ToString();
                GameManager.Instance.isGameOver = true;
                GameManager.Instance.GameOver();
            }

            else if (health > 0)
            {
                transform.position = respawnPoint.transform.position;
                health--;
                healthText.text = "HEALTH: " + showHealth.ToString();
                Respawn();
            }
        }
    }
    
    private void Respawn()
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.RespawnCounter()); 
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
