using UnityEngine;
using UnityEngine.AI;

public class MonsterNavAgent : MonoBehaviour
{
    [Header("속도 설정")]
    [SerializeField] float _maxSpeed = 3f;

    [Header("도달 거리")]
    [SerializeField] float _eventDistance = 0.1f;

    NavMeshAgent _agent;
    Transform _currentTarget;
    Vector3 _lastTargetPos;

    public float EventDistance => _eventDistance;
    public Vector2 Direction => _agent.velocity.normalized;
    public NavMeshAgent NavMeshAgent => _agent;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        _agent.speed = _maxSpeed;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        StopNavMesh();
    }

    void LateUpdate()
    {
        // Z 고정
        Vector3 fixedPos = transform.position;
        fixedPos.z = 0;
        transform.position = fixedPos;

        // 타겟 추적 중이면 SetDestination 최소 호출
        if (_currentTarget != null)
        {
            Vector3 targetPos = _currentTarget.position;
            if ((targetPos - _lastTargetPos).sqrMagnitude > 0.01f)
            {
                _lastTargetPos = targetPos;
                _agent.SetDestination(targetPos);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        _currentTarget = target;

        _lastTargetPos = Vector3.positiveInfinity; // 무조건 처음에 SetDestination
        if (!_agent.enabled) _agent.enabled = true;
        _agent.isStopped = false;
    }

    public void StopMove()
    {
        _currentTarget = null;
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
    }

    public void StopNavMesh()
    {
        _agent.enabled = false;
    }
}