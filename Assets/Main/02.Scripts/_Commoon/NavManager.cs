using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct FlockingSettings
{
    [Header("산개")]
    public bool useSeparation;      // 산개(Separation) 행동을 사용할지 여부 (true이면 적용)
    [Header("응집")]
    public bool useCohesion;        // 응집(Cohesion) 행동을 사용할지 여부 (true이면 적용)
    [Header("산개 행동의 강도")]
    public float separationWeight;  // 산개 행동의 강도 (가중치)
    [Header("응집 행동의 강도")]
    public float cohesionWeight;    // 응집 행동의 강도 (가중치)
}
public class NavManager : MonoBehaviour
{
    [Header("군집 사용여부")]
    [SerializeField] bool _isFlocking;

    [Header("트롤러 파라미터")]
    [SerializeField] float _maxSpeed = 3f;                // 보조 캐릭터의 최대 이동 속도

    [SerializeField] float _eventDistance = 0.1f;
    [SerializeField] float _maxDistance = 50f;

    [Header("플로킹 파라미터")]
    [SerializeField] float _neighborRadius = 5f;          // 주변 보조 캐릭터를 감지할 반경
    [SerializeField] float _separationDistance = 1.0f;    // 보조 캐릭터들이 최소로 유지해야 하는 거리

    [Header("스티어링 가중치 설정")]
    [SerializeField] FlockingSettings _flockingSettings;  // 산개 및 응집 행동에 대한 설정을 담은 구조체

    List<NavManager> _agents;
    NavMeshAgent _navMeshAgent;
    Rigidbody2D _rigidbody2d;
    Vector2 _direction;

    public float EventDistance => _eventDistance;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public Vector2 Direction => _direction;
    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        NavMeshAgent.updateRotation = false;
        NavMeshAgent.updateUpAxis = false;
        NavMeshAgent.speed = _maxSpeed;
    }
    void LateUpdate()
    {
        Vector3 fixedPos = transform.position;
        fixedPos.z = 0f;
        transform.position = fixedPos;
    }
    public void SetAgents(List<NavManager> agents)
    { _agents = agents; }
    public void SetAvoidanceType(ObstacleAvoidanceType type)
    { NavMeshAgent.obstacleAvoidanceType = type; }
    public void Move(Transform target) // 기본 이동
    {
        NavMeshAgent.enabled = true;

        NavMeshAgent.isStopped = false;
        NavMeshAgent.SetDestination(target.position);

        _direction = target.transform.position - transform.position;
        _direction.Normalize();

        if (_isFlocking)
        {
            _flockingSettings.useSeparation = true;
            FlockingSystem(target);
        }
    }
    public void StopMove()
    {
        NavMeshAgent.isStopped = true;
        NavMeshAgent.velocity = Vector3.zero;

        if (_isFlocking)
        { _flockingSettings.useSeparation = false; }
    }
    public void StopNavMesh()
    {
        NavMeshAgent.enabled = false;
    }
    public void FlockingSystem(Transform target) // 산개 시스템
    {
        if (NavMeshAgent.isStopped) return;
        // Separation, Alignment, Cohesion 힘을 각각 계산.
        // 산개와 응집은 구조체 설정에 따라 사용 여부와 강도가 결정.
        Vector2 separation = _flockingSettings.useSeparation ? CalculateSeparation() * _flockingSettings.separationWeight : Vector2.zero;
        Vector2 cohesion = _flockingSettings.useCohesion ? CalculateCohesion() * _flockingSettings.cohesionWeight : Vector2.zero;
        Vector2 alignment = CalculateAlignment();

        // 세 가지 행동에서 나온 힘들을 모두 합칩니다.
        Vector2 steeringForce = separation + alignment + cohesion;

        // 현재 속도에 스티어링 힘을 적용하여 새로운 속도를 계산.
        // Time.fixedDeltaTime을 곱해 FixedUpdate() 주기에 맞게 보정.
        Vector2 desiredVelocity = _rigidbody2d.linearVelocity + steeringForce * Time.fixedDeltaTime;

        // 추가: 목표와의 거리에 따른 감속 처리
        float distanceToTarget = Vector2.Distance(target.transform.position, transform.position);
        if (distanceToTarget <= _eventDistance)
        {
            // 목표와의 거리가 stoppingDistance 이하면, 속도를 선형 보간(Lerp)하여 점진적으로 감소
            desiredVelocity = Vector2.Lerp(desiredVelocity, Vector2.zero, 1f - (distanceToTarget / NavMeshAgent.stoppingDistance));
        }

        // 계산된 속도가 최대 속도를 넘지 않도록 제한합니다. 제한 ? 보정 ?
        desiredVelocity = Vector2.ClampMagnitude(desiredVelocity, _maxSpeed);

        // Rigidbody2D의 속도를 업데이트하여 보조 캐릭터를 이동시킵니다. 
        _rigidbody2d.linearVelocity = desiredVelocity;

    }
    Vector2 CalculateSeparation() // 산개 계산 : 너무 가까이 있는 보조 캐릭터들과의 거리를 유지하도록 힘을 계산
    {
        Vector2 force = Vector2.zero;
        int count = 0;
        foreach (NavManager agent in _agents)
        {
            if (agent == this) continue;
            float distance = Vector2.Distance(transform.position, agent.transform.position);
            // 만약 거리(distance)가 최소 유지 거리(separationDistance)보다 작으면
            if (distance < _separationDistance && distance > 0.001f)
            {
                // 자신과 다른 에이전트 간의 방향을 구합니다.
                Vector2 diff = (Vector2)(transform.position - agent.transform.position);
                // 거리에 반비례하도록 정규화하여 힘을 계산 (거리가 가까울수록 큰 힘)
                diff = diff.normalized / (distance + 0.001f);
                force += diff; // 계산한 힘을 누적합니다.
                count++;       // 대상 수 증가
            }
        }
        if (count > 0)
        {
            force /= count; // 평균화
        }
        return force.normalized; // 최종적으로 방향만 반환 (강도는 가중치에 의해 조정됨)
    }
    Vector2 CalculateAlignment() // 정렬 계산 : 주변 보조 캐릭터들의 평균 속도를 기반으로 정렬하도록 하는 힘 계산
    {
        Vector2 averageVelocity = Vector2.zero;
        int count = 0;
        foreach (NavManager agent in _agents)
        {
            if (agent == this) continue;
            float distance = Vector2.Distance(transform.position, agent.transform.position);
            if (distance < _neighborRadius)
            {
                averageVelocity += agent._rigidbody2d.linearVelocity; // 주변 에이전트의 속도를 누적
                count++;
            }
        }
        if (count > 0)
        {
            averageVelocity /= count; // 평균 속도 계산
            // 현재 속도와 평균 속도의 차이를 반환하여, 정렬을 유도합니다.
            return averageVelocity - _rigidbody2d.linearVelocity;
        }
        return Vector2.zero;
    }
    Vector2 CalculateCohesion() // 응집 계산: 주변 보조 캐릭터들의 중심(중심 위치)으로 모이도록 하는 힘 계산
    {
        Vector2 centerOfMass = Vector2.zero;

        int count = 0;
        foreach (NavManager agent in _agents)
        {
            if (agent == this) continue;
            float distance = Vector2.Distance(transform.position, agent.transform.position);
            if (distance < _neighborRadius)
            {
                centerOfMass += (Vector2)agent.transform.position; // 주변 에이전트의 위치 누적
                count++;
            }
        }
        if (count > 0)
        {
            centerOfMass /= count; // 그룹의 중심(평균 위치) 계산
            // 현재 위치에서 그룹 중심까지의 방향을 계산하고, 최대 속도에 맞춰 보정한 후 현재 속도와의 차이를 반환합니다.
            Vector2 desired = (centerOfMass - (Vector2)transform.position).normalized * _maxSpeed;
            return desired - _rigidbody2d.linearVelocity;
        }
        return Vector2.zero;
    }
}