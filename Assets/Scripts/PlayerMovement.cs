using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 10;
    public float sideSpeed = 20;

    public float horizontalInput;
    public float horizontalMultiplier = 2;

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
    }
}
