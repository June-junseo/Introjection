using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float baseAttack = 10f;
    public float attackMultiplier = 1f;
    public float cooldownReduction = 0f;
    public float moveSpeed = 5f;

    public float critRate = 0f;        
    public float defenseRate = 0f;     
    public int projectileBonus = 0;    

    private float baseAttackMultiplier;
    private float baseCooldownReduction;
    private float baseMoveSpeed;
    private float baseCritRate;
    private float baseDefenseRate;
    private int baseProjectileBonus;

    private void Awake()
    {
        baseAttackMultiplier = attackMultiplier;
        baseCooldownReduction = cooldownReduction;
        baseMoveSpeed = moveSpeed;
        baseCritRate = critRate;
        baseDefenseRate = defenseRate;
        baseProjectileBonus = projectileBonus;
    }

    public void ResetStats()
    {
        attackMultiplier = baseAttackMultiplier;
        cooldownReduction = baseCooldownReduction;
        moveSpeed = baseMoveSpeed;
        critRate = baseCritRate;
        defenseRate = baseDefenseRate;
        projectileBonus = baseProjectileBonus;
    }

    public float CalculateDamage(float skillDamagePercent)
    {
        float damage = (baseAttack * attackMultiplier) * skillDamagePercent;
        if (Random.value < critRate) 
        {
            damage *= 1.5f;
        }
        return damage;
    }

    public float GetFinalMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetFinalCooldown(float skillBaseCooldown)
    {
        float totalCooldownReduction = Mathf.Clamp01(cooldownReduction);
        return skillBaseCooldown * (1f - totalCooldownReduction);
    }

    public float ApplyDefense(float incomingDamage)
    {
        if (Random.value < defenseRate)
        {
            return 0f;
        }
        return incomingDamage;
    }

    public int GetFinalProjectileCount(int skillProjectileCount)
    {
        return skillProjectileCount + projectileBonus;
    }
}
