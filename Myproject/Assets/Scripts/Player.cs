using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Vector2 vec;
    public float playerSpeed = 5f;

    public Button levelUpButton;

    private Rigidbody2D rb;
    private SPUM_Prefabs spum;
    private Monster monster;
    private CharacterStats stats;

    [SerializeField]
    private VirtualJoystick joystick;

    private Vector2 lastNonZeroVec = Vector2.left;

    private Transform spumRoot;

    public event Action<int, int, int> onExpChanged;
    public event Action<float, float> onHpBarChanged;
    public event Action onPlayerDied;

    private int level = 1;
    private int currentExp = 0;
    private int expToLevel;
    public float maxHp = 50f;
    public float currentHp;
    private bool isDead = false;
    public int gold = 0;

    private bool levelUpUIOpen = false;

    public event Action<int, int> onGoldChanged;
    public SelectSkillUi skillUi;

    public void AddGold(int amount)
    {
        gold += amount;
        onGoldChanged?.Invoke(gold, amount);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spum = GetComponentInChildren<SPUM_Prefabs>();
        stats = GetComponent<CharacterStats>();
        spumRoot = spum.transform;

        level = 1;
        currentExp = 0;
        expToLevel = CalculateExpToNextLevel(level);
    }

    public void AddExp(int amount)
    {
        currentExp += amount;

        if (currentExp >= expToLevel)
        {
            currentExp -= expToLevel;
            LevelUp();
        }

        onExpChanged?.Invoke(currentExp, expToLevel, level);
    }
    public void Heal(int amount)
    {
        currentHp += amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        onHpBarChanged?.Invoke(currentHp, maxHp); 
    }

    private void LevelUp()
    {
        level++;
        currentExp = 0; 
        expToLevel = CalculateExpToNextLevel(level);

        Debug.Log($"current level: {level}");

        if (skillUi != null)
        {
            skillUi.OpenUi();
        }
    }

    public void CheatAddExp(int amount)
    {
        AddExp(amount);
        Debug.Log($"Cheat! Exp +{amount}");
    }

    private int CalculateExpToNextLevel(int lvl)
    {
        return Mathf.Max(1, Mathf.RoundToInt(3 + Mathf.Pow(lvl, 1.8f)));
    }

    private void Start()
    {
        currentHp = maxHp;
        isDead = false;
        spum.PopulateAnimationLists();
        spum.OverrideControllerInit();
        spum.PlayAnimation(PlayerState.IDLE, 0);
    }

    private void Update()
    {
        if (joystick != null)
        {
            vec = joystick.Input;
        }
        else
        {
            vec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        if (vec != Vector2.zero)
        {
            lastNonZeroVec = vec.normalized;
        }
    }

    public Vector2 GetFacingDirection()
    {
        if (vec != Vector2.zero)
        {
            return vec.normalized;
        }
        else
        {
            return lastNonZeroVec;
        }
    }

    private void FixedUpdate()
    {
        float finalSpeed = stats.GetFinalMoveSpeed(); 
        Vector2 nextVec = vec.normalized * finalSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
    }

    private void LateUpdate()
    {
        if (vec != Vector2.zero)
        {
            spum.PlayAnimation(PlayerState.MOVE, 0);

            if (vec.x > 0)
            {
                spumRoot.localScale = new Vector3(-1, 1, 1);
            }
            else if (vec.x < 0)
            {
                spumRoot.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            spum.PlayAnimation(PlayerState.IDLE, 0);
        }
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = stats.ApplyDefense(damage);

        currentHp -= finalDamage;
        Debug.Log($"플레이어 데미지 입음 {finalDamage}");

        spum.PlayAnimation(PlayerState.DAMAGED, 0);
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        onHpBarChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void OpenLevelUpUI()
    {
        levelUpUIOpen = true;
        Time.timeScale = 0f;
    }

    public void CloseLevelUpUI()
    {
        levelUpUIOpen = false;
        if (!isDead)
        {
            Time.timeScale = 1f;
        }
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;

        spum.PlayAnimation(PlayerState.DEATH, 0);
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        AnimatorStateInfo stateInfo = spum._anim.GetCurrentAnimatorStateInfo(0);
        float animLength = stateInfo.length;

        yield return new WaitForSeconds(animLength);
        if (!levelUpUIOpen)
        {
            Time.timeScale = 0f;
        }

        onPlayerDied?.Invoke();

    }
}
