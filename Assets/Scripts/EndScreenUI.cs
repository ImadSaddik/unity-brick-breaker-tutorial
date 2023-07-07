using System.Collections;
using UnityEngine;
using TMPro;

public class EndScreenUI : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI livesRemainingText;
    public TextMeshProUGUI levelReachedText;
    public TextMeshProUGUI restartText;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        StartCoroutine(BlinkText());
    }

    private IEnumerator BlinkText()
    {
        while (true)
        {
            restartText.enabled = false;
            yield return new WaitForSeconds(.5f);

            restartText.enabled = true;
            yield return new WaitForSeconds(.5f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameManager.NewGame();
        }
    }
}
