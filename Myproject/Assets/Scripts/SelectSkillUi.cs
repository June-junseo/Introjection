using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkillUi : MonoBehaviour
{
    public GameObject uiPanel;
    public Button button1, button2, button3;
    public SkillManager skillManager;

    private List<SkillData> availableActive = new List<SkillData>();
    private List<PassiveSkillData> availablePassive = new List<PassiveSkillData>();
    private List<(bool isActive, int idx)> selectedForUi = new List<(bool, int)>();

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
        availableActive.Clear();
        availablePassive.Clear();
        selectedForUi.Clear();

        foreach (var data in skillManager.skillDatas)
        {
            var current = skillManager.GetCurrentSkill(data.skillGroup);
            if (current != null)
            {
                var next = skillManager.GetNextLevelSkill(current);
                if (next != null) availableActive.Add(next);
            }
            else if (data.level == 1)
            {
                availableActive.Add(data);
            }
        }

        foreach (var p in skillManager.PassiveSkills)
        {
            var current = skillManager.GetCurrentPassive(p.passiveGroup);
            if (current != null)
            {
                var next = skillManager.GetNextLevelPassive(current);
                if (next != null) availablePassive.Add(next);
            }
        }

        foreach (var p in CSVLoader.LoadCSV<PassiveSkillData>(skillManager.passiveCSV))
        {
            if (skillManager.GetCurrentPassive(p.passiveGroup) == null && p.level == 1)
                availablePassive.Add(p);
        }

        for (int i = 0; i < 3; i++)
        {
            bool chooseActive = (Random.value < 0.5f && availableActive.Count > 0) || availablePassive.Count == 0;
            if (chooseActive && availableActive.Count > 0)
            {
                int idx = Random.Range(0, availableActive.Count);
                selectedForUi.Add((true, idx));
            }
            else if (availablePassive.Count > 0)
            {
                int idx = Random.Range(0, availablePassive.Count);
                selectedForUi.Add((false, idx));
            }
        }

        UpdateButton(button1, 0);
        UpdateButton(button2, 1);
        UpdateButton(button3, 2);
    }

    private void UpdateButton(Button btn, int buttonIndex)
    {
        if (buttonIndex >= selectedForUi.Count)
        {
            btn.gameObject.SetActive(false);
            return;
        }

        btn.gameObject.SetActive(true);
        var sel = selectedForUi[buttonIndex];

        if (sel.isActive)
        {
            var skill = availableActive[sel.idx];
            btn.GetComponentInChildren<TMP_Text>().text = $"{skill.skillName} Lv.{skill.level}";
        }
        else
        {
            var passive = availablePassive[sel.idx];
            btn.GetComponentInChildren<TMP_Text>().text = $"{passive.skillName} Lv.{passive.level}";
        }
    }

    private void OnSkillClicked(int buttonIndex)
    {
        if (buttonIndex >= selectedForUi.Count)
        {
            return;
        }

        var sel = selectedForUi[buttonIndex];

        if (sel.isActive)
        {
            skillManager.AddSkill(availableActive[sel.idx]);
        }
        else
        {
            skillManager.AddPassiveSkill(availablePassive[sel.idx]);
        }

        CloseUi();
    }
}
