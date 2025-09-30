using UnityEngine;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    private bool isOpened = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpened)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            if (player != null)
            {
                isOpened = true;
                OpenChest(player);
            }
        }
    }

    private void OpenChest(Player player)
    {
        Debug.Log("OpenChest 호출됨");

        var skillMgr = player.GetComponentInChildren<SkillManager>();
        var selectUI = player.skillUi;

        if (skillMgr == null || selectUI == null)
        {
            Debug.LogError("SkillManager 또는 SelectSkillUi 없음!");
            return;
        }

        List<SkillEvolutionData> evoOptions = new List<SkillEvolutionData>();

        foreach (var active in skillMgr.GetOwnedActiveSkills())
        {
            foreach (var passive in skillMgr.PassiveSkills)
            {
                if (skillMgr.evolutionSystem.TryGetEvolution(active.Data.id, passive.id, out var evoData))
                {
                    if (evoData.unlock_condition == 0)
                    {
                        evoOptions.Add(evoData);
                    }
                }
            }
        }

        if (evoOptions.Count > 0)
        {
            selectUI.OpenEvolutionUI(skillMgr.GetOwnedActiveSkills()[0].Data, evoOptions);
        }
        else
        {
            selectUI.OpenUi();
        }

        Destroy(gameObject, 0.1f);
    }
}
