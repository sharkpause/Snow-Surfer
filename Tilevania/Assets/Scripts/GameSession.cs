using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerScore = 0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    private void Awake()
    {
        int numberOfGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;

        if(numberOfGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            StartCoroutine(ResetGameSession());
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        playerScore += pointsToAdd;
        scoreText.text = playerScore.ToString();
    }

    void TakeLife()
    {
        playerLives--;
        livesText.text = playerLives.ToString();
        StartCoroutine(DeathWithDelay());
    }

    IEnumerator DeathWithDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator ResetGameSession()
    {
        playerLives--;
        livesText.text = playerLives.ToString();
        yield return new WaitForSeconds(2f);
        
        FindFirstObjectByType<ScenePersist>().ResetScenePersist();

        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
    }

    void Update()
    {
        
    }
}
