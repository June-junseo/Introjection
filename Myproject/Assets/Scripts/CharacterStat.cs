using UnityEngine;

// Ä³¸¯ÅÍ ½ºÅÈ
public class CharacterStats : MonoBehaviour
{
    public float baseAttack = 10f;
    public float attackMultiplier = 1f;
    public float cooldownReduction = 0f;
    public float moveSpeed = 5f;

    private float baseAttackMultiplier = 1f;
    private float baseCooldownReduction = 0f;
    private float baseMoveSpeed = 5f;

    private void Awake()
    {
        baseAttackMultiplier = attackMultiplier;
        baseCooldownReduction = cooldownReduction;
        baseMoveSpeed = moveSpeed;
    }

    public void ResetStats()
    {
        attackMultiplier = baseAttackMultiplier;
        cooldownReduction = baseCooldownReduction;
        moveSpeed = baseMoveSpeed;
    }

    public float GetFinalAttack()
    {
        return baseAttack * attackMultiplier;
    }

    public float GetFinalCooldown(float skillBaseCooldown)
    {
        float totalCooldownReduction = Mathf.Clamp01(cooldownReduction);
        return skillBaseCooldown * (1f - totalCooldownReduction);
    }
}
