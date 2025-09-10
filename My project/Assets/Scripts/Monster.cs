using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;

    bool isLive;

    private Animator animator;

    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
       
        Vector2 dicVec = target.position - rb.position;
        Vector2 nextVec = dicVec.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
    }

    private void LateUpdate()
    {
        Vector2 dirVec = (target.position - rb.position).normalized;
        animator.SetFloat("Horizontal", dirVec.x);
        animator.SetFloat("Vertical", dirVec.y);
        animator.SetBool("Moving", dirVec != Vector2.zero);
    }
}
