using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    [SerializeField] private PoolManager pool;

    private void Start()
    {
        Init();
    }


    private void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                break;
        }
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = -150f;
                Arrange();
                break;
            default:
                break;
        }
    }

    public void Arrange()
    {
        float angleStep = 360f / count;   // 각 단검의 간격
        float radius = 1.5f;              // 플레이어로부터 거리

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;

            Transform dagger = pool.Get(prefabId).transform;
            dagger.SetParent(transform);  // 플레이어/SkillManager를 부모로 설정

            // 회전 및 위치 설정
            dagger.localRotation = Quaternion.Euler(0, 0, angle);
            dagger.localPosition = dagger.up * radius;

            // 데미지/관통 초기화
            dagger.GetComponent<Dagger>().Init(damage, -1);
        }
    }

}
