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
        scoreText.text = "Score " + score.ToString();
        livesText.text = "Lives remaining " + lives.ToString();
        levelText.text = "Level " + level.ToString();
    }

    private void NewGame()
    {
        score = 0;
        lives = 3;

        LoadLevel(1);
    }

    private void LoadLevel(int level)
    {
        this.level = level;

        if (level > NUM_LEVELS)
        {
            // Start over again at level 1 once you have beaten all the levels
            // You can also load a "Win" scene instead
            LoadLevel(1);
            return;
        }

        SceneManager.LoadScene("Level" + level);
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        ball = FindObjectOfType<Ball>();
        paddle = FindObjectOfType<Paddle>();
        bricks = FindObjectsOfType<Brick>();
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

        // Resetting the bricks is optional
        // for (int i = 0; i < bricks.Length; i++) {
        //     bricks[i].ResetBrick();
        // }
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
