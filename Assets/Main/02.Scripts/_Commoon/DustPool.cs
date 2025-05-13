using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DustPool : MonoBehaviour
{
    [SerializeField] GameObject _dustPrefab;
    [SerializeField] GameObject _useCharacter;     // 플레이어 캐릭터 또는 대상 오브젝트
    public int PoolSize = 10;
    private Queue<GameObject> _pool = new Queue<GameObject>();

    public Queue<GameObject> Pool() => _pool;

    // delay 후에 오브젝트를 다시 반환할 시간 (초)
    public float DeactivationDelay = 0.5f;
    // 캐릭터의 속도에 따른 스폰 간격 조절 변수
    private Rigidbody2D _targetRigidbody2D;           // 플레이어 캐릭터의 Rigidbody2D
    private NavMeshAgent _navMeshAgent;

    // 최대 및 최소 스폰 간격 (속도가 빠르면 min, 느리면 max)
    public float MinSpawnInterval = 0.05f;    // 최대 속도일 때 먼지 생성 간격
    public float MaxSpawnInterval = 0.5f;    // 정지 상태일 때 먼지 생성 간격
    // 플레이어 캐릭터의 최대 속도 (실제 게임에서 예상되는 최대 속도)
    public float CharacterMaxSpeed = 10f;

    private float _spawnTimer = 0f;

    void Start()
    {
        // 풀 사이즈만큼 오브젝트를 미리 생성하여 풀에 저장합니다.

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
                speed = _navMeshAgent.velocity.magnitude; // 현재 속도 계산
            }

            if (speed <= 0.1f) return;

            // 속도 0 ~ characterMaxSpeed 범위를 0~1로 정규화 값이 0과 1 사이에 있는지 확인하고, 벗어나면 최솟값인 0 또는 최댓값인 1로 반환
            float t = Mathf.Clamp01(speed / CharacterMaxSpeed);

            // 속도가 빠르면 간격(minSpawnInterval)으로, 느리면 간격(maxSpawnInterval)으로 보간
            float spawnInterval = Mathf.Lerp(MaxSpawnInterval, MinSpawnInterval, t);

            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= spawnInterval)
            {
                // 먼지 이펙트 생성 및 위치 설정
                GetDust();
                _spawnTimer = 0f;
            }
        }
    }

    // 풀에서 오브젝트를 꺼내 활성화
    public void GetDust()
    {
        if (_pool.Count > 0)
        {
            // 큐에서 오브젝트를 꺼냅니다.
            GameObject obj = _pool.Dequeue();
            // 오브젝트를 활성화하고 반환합니다.
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

    // 오브젝트를 일정 시간(delay) 후에 비활성화하고 풀에 다시 추가하는 코루틴
    private IEnumerator DeactivateAndReturn(GameObject obj, float delay)
    {
        // 지정한 시간만큼 대기
        yield return new WaitForSeconds(delay);
        // 오브젝트를 비활성화
        obj.SetActive(false);
        // 풀에 다시 추가하여 재활용할 수 있도록 합니다.
        _pool.Enqueue(obj);
    }
    public void PoolCreate()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            // 프리팹 인스턴스 생성
            GameObject obj = Instantiate(_dustPrefab);
            // 시작 시 오브젝트는 사용하지 않으므로 비활성화합니다.
            //obj.SetActive(false);
            // 오브젝트를 큐에 저장합니다.
            _pool.Enqueue(obj);
        }
    }
}

