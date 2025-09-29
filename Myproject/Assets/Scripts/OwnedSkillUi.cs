using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OwnedSkillUi : MonoBehaviour
{
    [System.Serializable]
    public class SkillButtonUi
    {
        public Image icon;
        public string skillGroup; 
    }

    public SkillManager skillManager;
    public SkillButtonUi[] activeSkillSlots;
    public SkillButtonUi[] passiveSkillSlots;

    public void RefreshOwnedSkills()
    {
        foreach (var slot in activeSkillSlots)
        {
            ClearSkillSlot(slot);
        }


        foreach (var skill in skillManager.GetOwnedActiveSkills())
        {

            var slot = activeSkillSlots.FirstOrDefault(s => s.skillGroup == skill.Data.skillGroup);

            if (slot == null)
            {
                slot = activeSkillSlots.FirstOrDefault(s => string.IsNullOrEmpty(s.skillGroup));
            }

            if (slot != null)
            {
                slot.skillGroup = skill.Data.skillGroup;
                UpdateSkillSlot(slot, skill.Data.iconPath);
            }
        }

        foreach (var slot in passiveSkillSlots)
        {
            ClearSkillSlot(slot);
        }

        foreach (var p in skillManager.PassiveSkills)
        {
            var slot = passiveSkillSlots.FirstOrDefault(s => s.skillGroup == p.passiveGroup);

            if (slot == null)
            {
                slot = passiveSkillSlots.FirstOrDefault(s => string.IsNullOrEmpty(s.skillGroup));
            }

            if (slot != null)
            {
                slot.skillGroup = p.passiveGroup;
                UpdateSkillSlot(slot, p.iconPath);
            }
        }
    }

    public void ReplaceSkillSlot(string oldGroup, string newGroup, string newIconPath)
    {
        var slot = activeSkillSlots.FirstOrDefault(s => s.skillGroup == oldGroup);

        if (slot != null)
        {
            slot.skillGroup = newGroup;
            Sprite icon = Resources.Load<Sprite>(newIconPath);
            slot.icon.sprite = icon != null ? icon : Resources.Load<Sprite>("Icons/DefaultIcon");
            slot.icon.gameObject.SetActive(true);
            slot.icon.SetAllDirty();
        }
        else
        {
            RefreshOwnedSkills();
        }
    }

    private void UpdateSkillSlot(SkillButtonUi slot, string iconPath)
    {
        Sprite icon = Resources.Load<Sprite>(iconPath);
        slot.icon.sprite = icon != null ? icon : Resources.Load<Sprite>("Icons/DefaultIcon");
        slot.icon.SetAllDirty();
        slot.icon.gameObject.SetActive(true);
    }

    private void ClearSkillSlot(SkillButtonUi slot)
    {
        slot.skillGroup = "";
        slot.icon.sprite = Resources.Load<Sprite>("Icons/DefaultIcon");
        slot.icon.gameObject.SetActive(false);
    }

}
