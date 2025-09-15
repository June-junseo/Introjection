using UnityEngine;

public class Staff : MonoBehaviour
{
    private float speed;
    private Transform target;
    private Vector3 direction;
    private float damage;
    private float maxDistance;
    private Vector3 startPos;

    public void Init(Transform target, float damage, float speed, float maxDistance)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
        this.maxDistance = maxDistance;

        startPos = transform.position;
        gameObject.SetActive(true);
    }

    public void Init(Vector3 direction, float damage, float speed, float maxDistance)
    {
        this.target = null;
        this.direction = direction.normalized;
        this.damage = damage;
        this.speed = speed;
        this.maxDistance = maxDistance;

        startPos = transform.position;
        gameObject.SetActive(true);

        float angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Update()
    {
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Monster monster = collision.GetComponent<Monster>();
        if (monster != null)
        {
             Vector2 knockbackDir = transform.right;
            float knockbackDistance = 1.2f; 
            float knockbackSpeed = 6f;

            monster.TakeDamage(damage, knockbackDir, knockbackDistance, knockbackSpeed);
            gameObject.SetActive(false);
        }
    }
}
