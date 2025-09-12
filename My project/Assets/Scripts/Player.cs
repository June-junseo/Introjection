using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 vec;
    public float playerSpeed = 5f;

    private Rigidbody2D rb;
    private SPUM_Prefabs spum;

    [SerializeField]
    private VirtualJoystick joystick;

    private Transform spumRoot; // 실제 flip을 줄 자식 SPUM 프리팹 transform

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spum = GetComponentInChildren<SPUM_Prefabs>();
        spumRoot = spum.transform; //
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
                spumRoot.localScale = new Vector3(-1, 1, 1);
            else if (vec.x < 0)
                spumRoot.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            spum.PlayAnimation(PlayerState.IDLE, 0);
        }
    }
}
