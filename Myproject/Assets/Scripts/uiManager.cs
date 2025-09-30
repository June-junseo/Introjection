using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI levelUpText;
    public Player player;
    public GameManager gameManager;
    public TextMeshProUGUI goldText;
    public GameObject gameOverUI;

    StringBuilder sb = new StringBuilder();
    StringBuilder sb2 = new StringBuilder();

    private void Start()
    {
        player.onExpChanged += UpdateUi;
        player.onGoldChanged += UpdateGoldUI;
        player.onPlayerDied += ShowGameOverUI;

        UpdateGoldUI(player.gold, 0);
    }

    private void UpdateUi(int currentExp, int expToLevel, int level)
    {
        slider.value = (float)currentExp / expToLevel;

        sb.Clear();
        sb.Append("Lv ");
        sb.Append(level);
        levelUpText.text = sb.ToString();
    }

    private void UpdateGoldUI(int currentGold, int addedGold)
    {
        sb2.Clear();
        sb2.Append(currentGold);

        goldText.text = sb2.ToString();
    }


    public void OnClickRestart()
    {
        if (gameManager != null)
        {
            gameManager.RestartGame();
        }
    }

    public void OnClickAddExpCheat()
    {
        player.CheatAddExp(100);
    }

    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }
}
