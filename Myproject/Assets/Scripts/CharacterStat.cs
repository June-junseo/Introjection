using UnityEngine;
using System;

[Serializable]
public class CharacterStats : MonoBehaviour
{
    public float baseAttack = 10f;
    public float attackMultiplier = 1f; 
    public float cooldownReduction = 0f; 
    public float moveSpeed = 5f;

    public float GetFinalAttack()
    {
        return baseAttack * attackMultiplier;
    }

    public float GetFinalCooldown(float skillBaseCooldown)
    {
        float totalCooldownReduction = cooldownReduction;
        totalCooldownReduction = Mathf.Clamp01(totalCooldownReduction);

        return skillBaseCooldown * (1f - totalCooldownReduction);
    }

}
