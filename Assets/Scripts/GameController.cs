using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float gameDuration = 60f; 
    public int requiredKills = 2;  
    private float timer;
    private int currentKills;

    void Start()
    {
        timer = gameDuration;
        currentKills = 0;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            EndGame();
        }
    }

    public void AddKill()
    {
        currentKills++;

        if (currentKills >= requiredKills)
        {
            SceneManager.LoadScene("WinScene"); 
        }
    }

    void EndGame()
    {
        if (currentKills >= requiredKills)
        {
            SceneManager.LoadScene("WinScene"); 
        }
        else
        {
            SceneManager.LoadScene("LoseScene"); 
        }
    }
}
