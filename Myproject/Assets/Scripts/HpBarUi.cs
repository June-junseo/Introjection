using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUi : MonoBehaviour
{
    public Slider hpSlider;
    public Player player;

    private void Start()
    {
        hpSlider.maxValue = player.maxHp;
        hpSlider.value = player.maxHp;

        player.onHpBarChanged += UpdateHpUi;
    }

    private void UpdateHpUi(float currentHp, float maxHp)
    {
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
    }
}
