using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkillUi : MonoBehaviour
{
    public GameObject uiPanel;

    public Button button1;
    public Button button2;
    public Button button3;

    public SkillManager skillManager;

    private List<SkillData> currentSkills = new List<SkillData>();

    private void Start()
    {
        CloseUi();

        button1.onClick.AddListener(() => OnSkillCliked(0));
        button2.onClick.AddListener(() => OnSkillCliked(1));
        button3.onClick.AddListener(() => OnSkillCliked(2));
    }

    private void PickRandomSkills()
    {
        currentSkills.Clear();

        List<SkillData> pool = new List<SkillData>(skillManager.skillDatas);

        for(int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, pool.Count);
            SkillData seleted = pool[randomIndex];
            currentSkills.Add(seleted);
            pool.RemoveAt(randomIndex);
        }

        button1.GetComponentInChildren<TMP_Text>().text = currentSkills.Count > 0 ? currentSkills[0].skillName : "";
        button2.GetComponentInChildren<TMP_Text>().text = currentSkills.Count > 1 ? currentSkills[1].skillName : "";
        button3.GetComponentInChildren<TMP_Text>().text = currentSkills.Count > 2 ? currentSkills[2].skillName : "";


    }

    private void OnSkillCliked(int index)
    {
        skillManager.AddSkills(currentSkills[index]);
        CloseUi();
    }

    public void OpenUi()
    {
        uiPanel.SetActive(true);
        PickRandomSkills();
    }

    public void CloseUi()
    {
        uiPanel.SetActive(false);
    }
}
