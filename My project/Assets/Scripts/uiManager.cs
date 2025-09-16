using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI levelUpText;
    public Player player;
    StringBuilder sb = new StringBuilder();

    private void Start()
    {
        
    }

    private void UpdateUi(float currentExp, float expToLevel, int level)
    {
        slider.value = currentExp / expToLevel;
        levelUpText.text = $"Level";
    }

}
