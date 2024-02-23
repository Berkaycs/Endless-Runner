using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] GameObject newHighScoreText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject countdownPanel;
    [SerializeField] PlayerMovement player;
    [SerializeField] Animator animator;

    public int score;
    private int highScore;
    public int respawnCounter = 3;
    public bool isGameOver = false;

    private void Awake()
    {
        Instance = this;
        gameOverPanel.SetActive(false);
        countdownPanel.SetActive(false);
    }

    void Start()
    {
        score = 0;
        scoreText.text = "SCORE: " + score.ToString();

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
        StartCoroutine(IncreaseScore());
    }

    IEnumerator IncreaseScore()
    {
        while (!isGameOver && player.isAlive == true)
        {
            yield return new WaitForSeconds(1f);
            score++;
            scoreText.text = "SCORE: " + score.ToString();
        }
    }
    
    public IEnumerator RespawnCounter()
    {
        countdownPanel.SetActive(true);
        player.isAlive = false;

        while (respawnCounter > 0)
        {
            countdownText.text = respawnCounter.ToString();
            respawnCounter--;
            yield return new WaitForSeconds(1f);
        }

        countdownPanel.SetActive(false);
        respawnCounter = 3;
        player.isAlive = true;
        animator.SetBool("IsDead", false);
    }
    
    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "HIGH SCORE: " + highScore.ToString();
            highScoreText.color = Random.ColorHSV();
            PlayerPrefs.SetInt("HighScore", highScore);
            newHighScoreText.SetActive(true);
        }

        else
        {
            newHighScoreText.SetActive(false);
        }
    }
}
