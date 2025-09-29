using LayerLab.CasualGame;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SelectSkillUi : MonoBehaviour
{
    public GameObject uiPanel;
    public Button button1, button2, button3;
    public SkillManager skillManager;
    public OwnedSkillUi ownedSkillUi;
    public GameObject fadePanel;
    public Player player;

    [System.Serializable]
    public class SkillButtonUi
    {
        public Button button;
        public Image icon;
        public TMP_Text text;
        public TMP_Text DecText;
    }
    private List<(bool isActive, SkillData active, PassiveSkillData passive)> selectedForUi
        = new List<(bool, SkillData, PassiveSkillData)>();

    public SkillButtonUi[] skillButtons;

    private void Start()
    {
        CloseUi();
        
        for(int i = 0; i < skillButtons.Length; i++)
        {
            int idx = i;
            skillButtons[i].button.onClick.AddListener(() => OnSkillClicked(idx));
        }
    }

    public void OpenUi()
    {
        uiPanel.SetActive(true);
        fadePanel.SetActive(true);
        player.OpenLevelUpUI(); 
        PickSkillsForLevelUp();
    }

    public void CloseUi()
    {
        uiPanel.SetActive(false);
        fadePanel.SetActive(false);
        player.CloseLevelUpUI();
    }


    private void PickSkillsForLevelUp()
    {
        selectedForUi.Clear();
        List<SkillData> activeCandidates = new List<SkillData>();
        List<PassiveSkillData> passiveCandidates = new List<PassiveSkillData>();
        HashSet<string> usedSkillGroups = new HashSet<string>();
        HashSet<string> usedPassiveGroups = new HashSet<string>();

        int ownedActiveCount = skillManager.GetOwnedActiveSkills().Count;
        int ownedPassiveCount = skillManager.PassiveSkills.Count;
        int maxActive = 3;
        int maxPassive = 5;

        HashSet<string> excludedActiveGroups = new HashSet<string>();

        foreach (var s in skillManager.GetOwnedActiveSkills())
        {
            if (s.Data.IsConsumedForEvolution)
            {
                excludedActiveGroups.Add(s.Data.skillGroup);
            }
        }

        foreach (var data in skillManager.skillDatas)
        {
            if (data.IsEvolutionSkill)
            {
                continue;
            }

            if (skillManager.evolvedSkillGroups.Contains(data.skillGroup))
            {
                continue;
            }

            if (data.IsConsumedForEvolution)
            {
                continue;
            }

            var current = skillManager.GetCurrentSkill(data.skillGroup);
            SkillData candidate = null;

            if (current != null)
            {
                candidate = skillManager.GetNextLevelSkill(current);
            }
            else if (ownedActiveCount < maxActive && data.level == 1)
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

        if (ownedPassiveCount < maxPassive)
        {
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

        for (int i = 0; i < skillButtons.Length; i++)
        {
            UpdateButton(skillButtons[i], i);
        }
    }




    private void UpdateButton(SkillButtonUi btnUi, int index)
    {
        if (index >= selectedForUi.Count)
        {
            btnUi.button.gameObject.SetActive(false);
            return;
        }

        btnUi.button.gameObject.SetActive(true);
        var sel = selectedForUi[index];

        string iconPath = "Icons/DefaultIcon";
        string text = "";
        string dec = "";

        if (sel.isActive)
        {
            if (!string.IsNullOrEmpty(sel.active.iconPath))
            {
                iconPath = sel.active.iconPath;
            }

            text = sel.active.skillName;
            dec = sel.active.description;
        }
        else
        {
            text = sel.passive.skillName;
            dec = sel.passive.flavorText;
            if (!string.IsNullOrEmpty(sel.passive.iconPath))
            {
                iconPath = sel.passive.iconPath;
            }
        }

        btnUi.text.text = text;
        btnUi.DecText.text = dec;

        Sprite icon = Resources.Load<Sprite>(iconPath);
        btnUi.icon.sprite = icon != null ? icon : Resources.Load<Sprite>("Icons/DefaultIcon");
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
            skillManager.ReplaceOrAddSkillByGroup(sel.active.skillGroup, sel.active);

            if (sel.active.IsEvolutionSkill)
            {
                skillManager.MarkEvolved(sel.active.skillGroup);
            }
        }
        else
        {
            skillManager.AddPassiveSkill(sel.passive);
        }

        ownedSkillUi.RefreshOwnedSkills();
        CloseUi();
    }



    public void OpenEvolutionUI(SkillData activeSkill, PassiveSkillData passive, SkillEvolutionData evoData)
    {
        var evoSkillData = skillManager.skillDatas.Find(s => s.id == evoData.evo_skill_id);
        if (evoSkillData == null)
        {
            return;
        }

        selectedForUi.Clear();
        selectedForUi.Add((true, evoSkillData, null));
        skillButtons[0].button.gameObject.SetActive(true);
        UpdateButton(skillButtons[0], 0);

        for (int i = 1; i < skillButtons.Length; i++)
        {
            skillButtons[i].button.gameObject.SetActive(false);
        }

        uiPanel.SetActive(true);
        fadePanel.SetActive(true);
        player?.OpenLevelUpUI();
    }



}
