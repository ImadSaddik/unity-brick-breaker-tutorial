using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ResetZoneAI : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManagerAI gameManager = FindObjectOfType<GameManagerAI>();
        Ball ball = other.gameObject.GetComponent<Ball>();

        if (gameManager != null) {
            gameManager.Miss();
        } else if (ball != null) {
            ball.ResetBall();
        }
    }
}
