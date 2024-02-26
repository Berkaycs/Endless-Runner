using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string IsDead = "IsDead";
    private const string HighScoreColor = "HighScoreColor";

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private GameObject _newHighScoreText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _countdownPanel;
    [SerializeField] private PlayerMovement _player;
    [SerializeField] private Animator _animator;

    private int _respawnCounter = 3;
    private int _highScore;
    public int Score;
    public bool IsGameOver = false;

    private void Awake()
    {
        Instance = this;
        _gameOverPanel.SetActive(false);
        _countdownPanel.SetActive(false);
    }

    void Start()
    {
        Score = 0;
        _scoreText.text = "SCORE: " + Score.ToString();

        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        _highScoreText.text = "HIGH SCORE: " + _highScore.ToString();

        string highScoreColorString = PlayerPrefs.GetString(HighScoreColor, ColorUtility.ToHtmlStringRGB(Color.white));
        Color highScoreColor;
        if (ColorUtility.TryParseHtmlString("#" + highScoreColorString, out highScoreColor))
        {
            _highScoreText.color = highScoreColor;
        }

        StartCoroutine(IncreaseScore());
    }

    IEnumerator IncreaseScore()
    {
        while (!IsGameOver && _player.IsAlive == true)
        {
            yield return new WaitForSeconds(0.2f);
            Score++;
            _scoreText.text = "SCORE: " + Score.ToString();
        }
    }
    
    public IEnumerator RespawnCounter()
    {
        _countdownPanel.SetActive(true);

        while (_respawnCounter > 0)
        {
            _countdownText.text = _respawnCounter.ToString();
            _respawnCounter--;
            yield return new WaitForSeconds(1f);
        }

        _countdownPanel.SetActive(false);
        _respawnCounter = 3;
        _player.IsAlive = true;
        _animator.SetBool(IsDead, false);
        StartCoroutine(IncreaseScore());
    }
    
    public void GameOver()
    {
        _gameOverPanel.SetActive(true);

        if (Score > _highScore)
        {
            _highScore = Score + 1;
            _highScoreText.text = "HIGH SCORE: " + _highScore.ToString();
            PlayerPrefs.SetInt("HighScore", _highScore);

            _highScoreText.color = Random.ColorHSV();
            string highScoreColorString = ColorUtility.ToHtmlStringRGB(_highScoreText.color);
            PlayerPrefs.SetString(HighScoreColor, highScoreColorString);
            _newHighScoreText.SetActive(true);         
        }

        else
        {
            _newHighScoreText.SetActive(false);
        }
    }
}
