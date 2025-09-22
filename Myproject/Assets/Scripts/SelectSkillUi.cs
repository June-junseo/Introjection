using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkillUi : MonoBehaviour
{
    public GameObject uiPanel;
    public Button button1, button2, button3;
    public SkillManager skillManager;

    private List<(bool isActive, SkillData active, PassiveSkillData passive)> selectedForUi = new List<(bool, SkillData, PassiveSkillData)>();

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
        selectedForUi.Clear();

        List<SkillData> activeCandidates = new List<SkillData>();
        List<PassiveSkillData> passiveCandidates = new List<PassiveSkillData>();
        HashSet<string> usedSkillGroups = new HashSet<string>();
        HashSet<string> usedPassiveGroups = new HashSet<string>();

        foreach (var data in skillManager.skillDatas)
        {
            var current = skillManager.GetCurrentSkill(data.skillGroup);

            SkillData candidate = null;

            if (current != null)
            {
                candidate = skillManager.GetNextLevelSkill(current);
            }
            else if (data.level == 1)
            {
                candidate = data;
            }

            if (candidate != null && !usedSkillGroups.Contains(candidate.skillGroup))
            {
                activeCandidates.Add(candidate);
                usedSkillGroups.Add(candidate.skillGroup);
            }
        }

        foreach (var p in skillManager.PassiveSkills)
        {
            var next = skillManager.GetNextLevelPassive(p);
            if (next != null && !usedPassiveGroups.Contains(next.passiveGroup))
            {
                passiveCandidates.Add(next);
                usedPassiveGroups.Add(next.passiveGroup);
            }
        }

        foreach (var p in CSVLoader.LoadCSV<PassiveSkillData>(skillManager.passiveCSV))
        {
            if (skillManager.GetCurrentPassive(p.passiveGroup) == null
                && p.level == 1
                && !usedPassiveGroups.Contains(p.passiveGroup))
            {
                passiveCandidates.Add(p);
                usedPassiveGroups.Add(p.passiveGroup);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (activeCandidates.Count == 0 && passiveCandidates.Count == 0)
            {
                break;
            }

            bool chooseActive = (Random.value < 0.5f && activeCandidates.Count > 0) || passiveCandidates.Count == 0;

            if (chooseActive && activeCandidates.Count > 0)
            {
                int idx = Random.Range(0, activeCandidates.Count);
                selectedForUi.Add((true, activeCandidates[idx], null));
                activeCandidates.RemoveAt(idx);
            }
            else if (passiveCandidates.Count > 0)
            {
                int idx = Random.Range(0, passiveCandidates.Count);
                selectedForUi.Add((false, null, passiveCandidates[idx]));
                passiveCandidates.RemoveAt(idx);
            }
        }

        UpdateButton(button1, 0);
        UpdateButton(button2, 1);
        UpdateButton(button3, 2);
    }

    private void UpdateButton(Button btn, int index)
    {
        if (index >= selectedForUi.Count)
        {
            btn.gameObject.SetActive(false);
            return;
        }

        btn.gameObject.SetActive(true);
        var sel = selectedForUi[index];

        if (sel.isActive)
        {
            btn.GetComponentInChildren<TMP_Text>().text = $"{sel.active.skillName} Lv.{sel.active.level}";
        }
        else
        {
            btn.GetComponentInChildren<TMP_Text>().text = $"{sel.passive.skillName} Lv.{sel.passive.level}";
        }
    }

    private void OnSkillClicked(int index)
    {
        if (index >= selectedForUi.Count)
        {
            return;
        }

        var sel = selectedForUi[index];

        if (sel.isActive)
        {
            skillManager.AddSkill(sel.active);
        }
        else
        {
            skillManager.AddPassiveSkill(sel.passive);
        }

        CloseUi();
    }
}
