using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 10;
    public float sideSpeed = 20;

    public float horizontalInput;
    public float horizontalMultiplier = 2;

    public float jumpForce = 6;
    public float doubleJumpForce = 6;

    public float Gravity = -20;

    public bool isOnGround = true;
    public bool doubleJump = false;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 forwardMove = transform.forward * forwardSpeed * Time.fixedDeltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput * sideSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove + horizontalMove);
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        Jump();
    }

    void Jump()
    {
        if (isOnGround && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("First jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            doubleJump = true;
        }

        else if (!isOnGround && doubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            doubleJump = false;
            Debug.Log("Second jump");
            rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Player collides with the ground");
            isOnGround = true;
            doubleJump = false;
        }
    }
}
