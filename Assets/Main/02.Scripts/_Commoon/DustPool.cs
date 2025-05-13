using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DustPool : MonoBehaviour
{
    [SerializeField] GameObject _dustPrefab;
    [SerializeField] GameObject _useCharacter;     // �÷��̾� ĳ���� �Ǵ� ��� ������Ʈ
    public int PoolSize = 10;
    private Queue<GameObject> _pool = new Queue<GameObject>();

    public Queue<GameObject> Pool() => _pool;

    // delay �Ŀ� ������Ʈ�� �ٽ� ��ȯ�� �ð� (��)
    public float DeactivationDelay = 0.5f;
    // ĳ������ �ӵ��� ���� ���� ���� ���� ����
    private Rigidbody2D _targetRigidbody2D;           // �÷��̾� ĳ������ Rigidbody2D
    private NavMeshAgent _navMeshAgent;

    // �ִ� �� �ּ� ���� ���� (�ӵ��� ������ min, ������ max)
    public float MinSpawnInterval = 0.05f;    // �ִ� �ӵ��� �� ���� ���� ����
    public float MaxSpawnInterval = 0.5f;    // ���� ������ �� ���� ���� ����
    // �÷��̾� ĳ������ �ִ� �ӵ� (���� ���ӿ��� ����Ǵ� �ִ� �ӵ�)
    public float CharacterMaxSpeed = 10f;

    private float _spawnTimer = 0f;

    void Start()
    {
        // Ǯ �����ŭ ������Ʈ�� �̸� �����Ͽ� Ǯ�� �����մϴ�.

        PoolCreate();

        if (_useCharacter != null)
        {
            _targetRigidbody2D = _useCharacter.GetComponentInParent<Rigidbody2D>();
            _navMeshAgent = _useCharacter.GetComponentInParent<NavMeshAgent>();
        }
        else
        {
            Debug.Log("_useCharacter == null");
        }
    }

    private void Update()
    {
        SpawnDust();
    }

    private void SpawnDust()
    {
        if (_targetRigidbody2D != null)
        {
            float speed;
            if (_navMeshAgent == null)
            {
                speed = _targetRigidbody2D.linearVelocity.magnitude;
            }
            else
            {
                speed = _navMeshAgent.velocity.magnitude; // ���� �ӵ� ���
            }

            if (speed <= 0.1f) return;

            // �ӵ� 0 ~ characterMaxSpeed ������ 0~1�� ����ȭ ���� 0�� 1 ���̿� �ִ��� Ȯ���ϰ�, ����� �ּڰ��� 0 �Ǵ� �ִ��� 1�� ��ȯ
            float t = Mathf.Clamp01(speed / CharacterMaxSpeed);

            // �ӵ��� ������ ����(minSpawnInterval)����, ������ ����(maxSpawnInterval)���� ����
            float spawnInterval = Mathf.Lerp(MaxSpawnInterval, MinSpawnInterval, t);

            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= spawnInterval)
            {
                // ���� ����Ʈ ���� �� ��ġ ����
                GetDust();
                _spawnTimer = 0f;
            }
        }
    }

    // Ǯ���� ������Ʈ�� ���� Ȱ��ȭ
    public void GetDust()
    {
        if (_pool.Count > 0)
        {
            // ť���� ������Ʈ�� �����ϴ�.
            GameObject obj = _pool.Dequeue();
            // ������Ʈ�� Ȱ��ȭ�ϰ� ��ȯ�մϴ�.
            obj.SetActive(true);
            obj.transform.position = _useCharacter.transform.position;
            //StartCoroutine(DeactivateAndReturn(obj, DeactivationDelay));
            _pool.Enqueue(obj);
        }
        else
        {
            PoolCreate();
        }
    }

    // ������Ʈ�� ���� �ð�(delay) �Ŀ� ��Ȱ��ȭ�ϰ� Ǯ�� �ٽ� �߰��ϴ� �ڷ�ƾ
    private IEnumerator DeactivateAndReturn(GameObject obj, float delay)
    {
        // ������ �ð���ŭ ���
        yield return new WaitForSeconds(delay);
        // ������Ʈ�� ��Ȱ��ȭ
        obj.SetActive(false);
        // Ǯ�� �ٽ� �߰��Ͽ� ��Ȱ���� �� �ֵ��� �մϴ�.
        _pool.Enqueue(obj);
    }
    public void PoolCreate()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            // ������ �ν��Ͻ� ����
            GameObject obj = Instantiate(_dustPrefab);
            // ���� �� ������Ʈ�� ������� �����Ƿ� ��Ȱ��ȭ�մϴ�.
            //obj.SetActive(false);
            // ������Ʈ�� ť�� �����մϴ�.
            _pool.Enqueue(obj);
        }
    }
}

