using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const string IsLeaning = "IsLeaning";
    private const string IsDead = "IsDead";

    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _respawnPoint;
    private Rigidbody _rb;

    private float _forwardSpeed = 70;
    private float _sideSpeed = 100;
    private float _maxSpeed = 120;

    private float _middleRight = 13.5f;
    private float _middleLeft = -13.5f;

    private float _jumpForce = 50;
    private float _doubleJumpForce = 50;

    private float _gravityModifier = 2f;

    private int _health;

    private bool _isOnGround = true;
    private bool _canDoubleJump = false;

    public bool IsAlive = true;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Physics.gravity *= _gravityModifier;
        _health = 3;
        _healthText.text = "HEALTH: " + _health.ToString();
    }

    void Update()
    {
        if (!IsAlive)
        {
            return;
        }

        IncreaseSpeedBySeconds();
        HandleMovement();
        Jump();
        Lean();
    }

    void HandleMovement()
    {
        transform.Translate(0, 0, _forwardSpeed * Time.deltaTime);

        Vector3 toLeft = new Vector3(_middleLeft, transform.position.y, transform.position.z);
        Vector3 toRight = new Vector3(_middleRight, transform.position.y, transform.position.z);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = Vector3.Lerp(transform.position, toLeft, _sideSpeed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = Vector3.Lerp(transform.position, toRight, _sideSpeed * Time.deltaTime);
        }

        else
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }

    void Jump()
    {
        if (_isOnGround && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("First jump");
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _isOnGround = false;
            _canDoubleJump = true;
        }

        else if (!_isOnGround && _canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            _canDoubleJump = false;
            Debug.Log("Second jump");
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            _rb.AddForce(Vector3.up * _doubleJumpForce, ForceMode.Impulse);
        }
    }

    void Lean()
    {
        if (_isOnGround && Input.GetKeyDown(KeyCode.LeftControl))
        {
            _animator.SetBool(IsLeaning, true);
        }
        
        else
        {
            _animator.SetBool(IsLeaning, false);
        }
    }

    void IncreaseSpeedBySeconds()
    {
        if (_forwardSpeed < _maxSpeed)
        {
            _forwardSpeed += 0.3f * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = true;
            _canDoubleJump = false;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AudioManager.instance.PlayInDeath();
            HealthHandler();
        }

        if (collision.gameObject.CompareTag("Rock"))
        {
            AudioManager.instance.PlayInDeath();
            collision.gameObject.SetActive(false);
            HealthHandler();
        }
    }
    
    private void Respawn()
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.RespawnCounter()); 
    }

    private void HealthHandler()
    {
        IsAlive = false;
        _health--;
        _animator.SetBool(IsDead, true);

        if (_health == 0)
        {
            _healthText.text = "HEALTH: " + 0.ToString();
            GameManager.Instance.IsGameOver = true;
            GameManager.Instance.GameOver();
        }

        else if (_health > 0)
        {
            transform.position = _respawnPoint.transform.position;
            _healthText.text = "HEALTH: " + _health.ToString();
            Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SilverCoin"))
        {
            AudioManager.instance.PlayPickupCoin();
            GameManager.Instance.Score += 3;
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("GoldCoin"))
        {
            AudioManager.instance.PlayPickupCoin();
            GameManager.Instance.Score += 5;
            other.gameObject.SetActive(false);
        }
    }
}
