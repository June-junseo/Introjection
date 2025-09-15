using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Vector2 vec;
    public float playerSpeed = 5f;

    private Rigidbody2D rb;
    private SPUM_Prefabs spum;

    [SerializeField]
    private VirtualJoystick joystick;

    private Transform spumRoot;

    private int level = 1;
    private int currentExp;
    private int expToLevel;
    public Slider slider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spum = GetComponentInChildren<SPUM_Prefabs>();
        spumRoot = spum.transform; 
    }

    private void Start()
    {
        spum.PopulateAnimationLists();
        spum.OverrideControllerInit();
        spum.PlayAnimation(PlayerState.IDLE, 0);
    }

    private void Update()
    {
        if (joystick != null)
            vec = joystick.Input;
        else
            vec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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
}
