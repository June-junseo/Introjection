using UnityEngine;
using UnityEngine.UI;

public class OwnedSkillUi : MonoBehaviour
{
    [System.Serializable]
    public class SkillButtonUi
    {
        public Image icon;
    }

    public SkillManager skillManager;

    public SkillButtonUi[] activeSkillSlots; 
    public SkillButtonUi[] passiveSkillSlots;

    private void Start()
    {
        RefreshOwnedSkills();
    }

    public void RefreshOwnedSkills()
    {
        var ownedActive = skillManager.GetOwnedActiveSkills();

        for (int i = 0; i < activeSkillSlots.Length; i++)
        {
            if (i < ownedActive.Count)
            {
                var skill = ownedActive[i];
                string iconPath = GetBaseIconPath(skill.Data.skillGroup, true);
                UpdateSkillSlot(activeSkillSlots[i], iconPath);
            }
            else
            {
                ClearSkillSlot(activeSkillSlots[i]);
            }
        }

        var ownedPassive = skillManager.PassiveSkills;

        for (int i = 0; i < passiveSkillSlots.Length; i++)
        {
            if (i < ownedPassive.Count)
            {
                var p = ownedPassive[i];
                string iconPath = GetBaseIconPath(p.passiveGroup, false);
                UpdateSkillSlot(passiveSkillSlots[i], iconPath);
            }
            else
            {
                ClearSkillSlot(passiveSkillSlots[i]);
            }
        }
    }

    private string GetBaseIconPath(string group, bool isActive)
    {
        if (isActive)
        {
            var level1 = skillManager.skillDatas.Find(s => s.skillGroup == group && s.level == 1);
            if (level1 != null && !string.IsNullOrEmpty(level1.iconPath))
            {
                return level1.iconPath;
            }
        }
        else
        {
            var allPassive = CSVLoader.LoadCSV<PassiveSkillData>(skillManager.passiveCSV);

            var level1 = allPassive.Find(p => p.passiveGroup == group && p.level == 1);

            if (level1 != null && !string.IsNullOrEmpty(level1.iconPath))
            {
                return level1.iconPath;
            }
        }

        return "Icons/DefaultIcon"; 
    }

    private void UpdateSkillSlot(SkillButtonUi slot, string iconPath)
    {
        Sprite icon = Resources.Load<Sprite>(iconPath);
        slot.icon.sprite = icon != null ? icon : Resources.Load<Sprite>("Icons/DefaultIcon");
        slot.icon.gameObject.SetActive(true);
    }

    private void ClearSkillSlot(SkillButtonUi slot)
    {
        slot.icon.sprite = Resources.Load<Sprite>("Icons/DefaultIcon");
        slot.icon.gameObject.SetActive(false);
    }
}
