using UnityEngine;
using TMPro;

public class GameManagerAI : MonoBehaviour
{
    public Ball ball { get; private set; }
    public PaddleAgent agent { get; private set; }
    public BrickAI[] bricks { get; private set; }
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI rewardText;

    public int score = 0;
    public int lives = 3;

    private void Awake()
    {
        instantiateBallPaddleBricks();
    }

    private void instantiateBallPaddleBricks()
    {
        ball = FindObjectOfType<Ball>();
        agent = FindObjectOfType<PaddleAgent>();
        bricks = FindObjectsOfType<BrickAI>();
    }

    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        score = 0;
        lives = 3;
        activateBricks();
        ResetLevel();
    }

    private void activateBricks()
    {
        for (int i = 0; i < bricks.Length; i++)
        {
            bricks[i].ResetBrick();
        }
    }

    private void Update()
    {
        updateUIElements();
    }

    private void updateUIElements()
    {
        scoreText.text = "Score " + score.ToString();
        livesText.text = "Lives remaining " + lives.ToString();
        rewardText.text = "Reward " + agent.GetCumulativeReward().ToString("0.00");
    }

    public void Miss()
    {
        lives--;

        if (lives > 0) {
            agent.AddReward(-0.5f);
            ResetLevel();
        } else {
            GameOver();
        }
    }

    private void ResetLevel()
    {
        agent.ResetAgent();
        ball.ResetBall();
    }

    private void GameOver()
    {
        agent.AddReward(-1f);
        agent.EndEpisode();
        NewGame();
    }

    public void Hit(BrickAI brick)
    {
        score += brick.points;
        agent.AddReward(0.01f);

        if (Cleared()) {
            agent.AddReward(1f);
            agent.EndEpisode();
            NewGame();
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
