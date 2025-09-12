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
        float angleStep = 360f / count;   // �� �ܰ��� ����
        float radius = 1.5f;              // �÷��̾�κ��� �Ÿ�

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;

            Transform dagger = pool.Get(prefabId).transform;
            dagger.SetParent(transform);  // �÷��̾�/SkillManager�� �θ�� ����

            // ȸ�� �� ��ġ ����
            dagger.localRotation = Quaternion.Euler(0, 0, angle);
            dagger.localPosition = dagger.up * radius;

            // ������/���� �ʱ�ȭ
            dagger.GetComponent<Dagger>().Init(damage, -1);
        }
    }

}
