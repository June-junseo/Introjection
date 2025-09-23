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
                UpdateSkillSlot(activeSkillSlots[i], skill.Data.iconPath);
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
                UpdateSkillSlot(passiveSkillSlots[i], p.iconPath);
            }
            else
            {
                ClearSkillSlot(passiveSkillSlots[i]);
            }
        }
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
