using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkillUi : MonoBehaviour
{
    public GameObject uiPanel;
    public Button button1, button2, button3;
    public SkillManager skillManager;

    private List<SkillData> currentSkills = new List<SkillData>();

    private void Start()
    {
        CloseUi();
        button1.onClick.AddListener(() => OnSkillClicked(0));
        button2.onClick.AddListener(() => OnSkillClicked(1));
        button3.onClick.AddListener(() => OnSkillClicked(2));
    }

    public void OpenUi()
    {
        uiPanel.SetActive(true);
        Time.timeScale = 0f;
        PickSkillsForLevelUp();
    }

    public void CloseUi()
    {
        uiPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void PickSkillsForLevelUp()
    {
        currentSkills.Clear();
        List<SkillData> pool = new List<SkillData>();

        foreach (var data in skillManager.skillDatas)
        {
            SkillData current = skillManager.GetCurrentSkill(data.skillName);

            if (current != null)
            {
                SkillData next = skillManager.GetNextLevelSkill(current);
                if (next != null)
                {
                    pool.Add(next);
                }
            }
            else if (data.level == 1)
            {
                pool.Add(data);
            }
        }

        int count = Mathf.Min(3, pool.Count);
        for (int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, pool.Count);
            SkillData picked = pool[idx];

            currentSkills.Add(picked);
            pool.RemoveAt(idx);   
        }

        UpdateButton(button1, 0);
        UpdateButton(button2, 1);
        UpdateButton(button3, 2);
    }

    private void UpdateButton(Button btn, int idx)
    {
        if (idx < currentSkills.Count)
        {
            btn.gameObject.SetActive(true);
            var skill = currentSkills[idx];
            btn.GetComponentInChildren<TMP_Text>().text = $"{skill.skillName} Lv.{skill.level}";
        }
        else
        {
            btn.gameObject.SetActive(false);
        }
    }

    private void OnSkillClicked(int index)
    {
        if (index >= currentSkills.Count)
        {
            return;
        }

        skillManager.AddSkill(currentSkills[index]); 
        CloseUi();
    }
}
