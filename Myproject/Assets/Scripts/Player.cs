using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    public Vector2 vec;
    public float playerSpeed = 5f;

    public Button levelUpButton;

    private Rigidbody2D rb;
    private SPUM_Prefabs spum;
    private Monster monster;

    [SerializeField]
    private VirtualJoystick joystick;

    private Transform spumRoot;

    public event System.Action<int, int, int> onExpChanged;
    public event System.Action<float, float> onHpBarChanged;

    private int level = 1;
    private int currentExp = 0;
    private int expToLevel;
    public float maxHp = 50f;
    public float currentHp;
    private bool isDead = false;

    public SelectSkillUi skillUi;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spum = GetComponentInChildren<SPUM_Prefabs>();
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
        return Mathf.Max(1, Mathf.RoundToInt(3 + Mathf.Pow(lvl, 1.4f)));
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
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = vec.normalized * playerSpeed * Time.fixedDeltaTime;
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
        currentHp -= damage;
        Debug.Log($"플레이어 데미지 입음 {damage}");
        spum.PlayAnimation(PlayerState.DAMAGED, 0);

        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        onHpBarChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        spum.PlayAnimation(PlayerState.DEATH, 0);
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        AnimatorStateInfo stateInfo = spum._anim.GetCurrentAnimatorStateInfo(0);
        float animLength = stateInfo.length;

        yield return new WaitForSeconds(animLength);

    
        Time.timeScale = 0f;
        //게임 오버 처리 여기
    }
}
