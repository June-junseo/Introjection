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
    StringBuilder sb = new StringBuilder();

    private void Start()
    {
        player.onExpChanged += UpdateUi;
    }

    private void UpdateUi(int currentExp, int expToLevel, int level)
    {
        slider.value = (float)currentExp / expToLevel;

        sb.Clear();
        sb.Append("Lv ");
        sb.Append(level);
        levelUpText.text = sb.ToString();
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


}
