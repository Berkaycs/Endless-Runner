using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] GameObject newHighScoreText;
    [SerializeField] GameObject gameOverPanel;

    public int score;
    private int highScore;
    public bool isGameOver = false;

    private void Awake()
    {
        Instance = this;
        gameOverPanel.SetActive(false);
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
        while (!isGameOver)
        {
            yield return new WaitForSeconds(1f);
            score++;
            scoreText.text = "SCORE: " + score.ToString();
        }
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
