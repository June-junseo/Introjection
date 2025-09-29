using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float maxTime = 15f * 60f; 
    private float currentTime = 0f;
    private bool gameEnded = false;

    public TMP_Text timerText; 
   
    public SkillManager skillManager;
    public OwnedSkillUi ownedSkillUi;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        skillManager.InitSkillManager();
        ownedSkillUi.RefreshOwnedSkills();
    }

    private void Update()
    {
        if (gameEnded)
        {
            return;
        }

        currentTime += Time.deltaTime;
        UpdateTimerUI();

        if (currentTime >= maxTime)
        {
            EndGame();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText == null)
        {
            return;
        }

        float remaining = Mathf.Max(0, maxTime - currentTime);
        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void EndGame()
    {
        gameEnded = true;
        Debug.Log("15분 종료 → 게임 오버 처리");

        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Time.timeScale = 1f;
    }
}
