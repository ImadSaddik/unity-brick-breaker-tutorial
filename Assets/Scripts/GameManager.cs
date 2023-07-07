using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    const int NUM_LEVELS = 3;
    public Ball ball { get; private set; }
    public Paddle paddle { get; private set; }
    public Brick[] bricks { get; private set; }
    public GameObject ui;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI levelText;
    private EndScreenUI endScreenUI;

    public int level = 1;
    public int score = 0;
    public int lives = 3;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(ui);

        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        updateUIElements();
    }

    private void updateUIElements()
    {
        scoreText.text = "Score " + score.ToString();
        livesText.text = "Lives remaining " + lives.ToString();
        levelText.text = "Level " + level.ToString();
    }

    public void NewGame()
    {
        score = 0;
        lives = 3;
        ui.SetActive(true);
        
        LoadLevel(1);
    }

    private void LoadLevel(int level)
    {
        this.level = level;

        if (level > NUM_LEVELS)
        {
            SceneManager.LoadScene("EndScene");
            return;
        }

        SceneManager.LoadScene("Level" + level);
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (level <= NUM_LEVELS) {
            instantiateBallPaddleBricks();
        } else {
            endScreenUI = FindObjectOfType<EndScreenUI>();
            sendDataToEndScreen();
            ui.SetActive(false);
        }

    }

    private void instantiateBallPaddleBricks()
    {
        ball = FindObjectOfType<Ball>();
        paddle = FindObjectOfType<Paddle>();
        bricks = FindObjectsOfType<Brick>();
    }

    private void sendDataToEndScreen()
    {
        endScreenUI.bestScoreText.text = "Best Score: " + score.ToString();
        endScreenUI.livesRemainingText.text = "Lives Remaining: " + lives.ToString();

        if (level <= NUM_LEVELS)
            endScreenUI.levelReachedText.text = "Level Reached: " + level.ToString();
    }

    public void Miss()
    {
        lives--;

        if (lives > 0) {
            ResetLevel();
        } else {
            GameOver();
        }
    }

    private void ResetLevel()
    {
        paddle.ResetPaddle();
        ball.ResetBall();
    }

    private void GameOver()
    {
        // Start a new game immediately
        // You can also load a "GameOver" scene instead
        NewGame();
    }

    public void Hit(Brick brick)
    {
        score += brick.points;

        if (Cleared()) {
            LoadLevel(level + 1);
        }
    }

    private bool Cleared()
    {
        for (int i = 0; i < bricks.Length; i++)
        {
            if (bricks[i].gameObject.activeInHierarchy && !bricks[i].unbreakable) {
                return false;
            }
        }

        return true;
    }
}
