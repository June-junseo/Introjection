using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 vec;
    public float playerSpeed;

    private Rigidbody2D rb;

    private Animator animator;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        vec.x = Input.GetAxisRaw("Horizontal");
        vec.y = Input.GetAxisRaw("Vertical");
        // maybe use GetAxisRaw ?? : 플레이어 움직임의 느낌에 따라 다를것

    }

    private void FixedUpdate()
    {
        Vector2 nextVec = vec.normalized * playerSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
    }

    private void LateUpdate()
    {

        animator.SetFloat("Horizontal", vec.x);
        animator.SetFloat("Vertical", vec.y);
        animator.SetBool("Run", vec != Vector2.zero);
    }
}
