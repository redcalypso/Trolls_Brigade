using UnityEngine;
using UnityEngine.AI;

public class MonsterNavAgent : MonoBehaviour
{
    [Header("�ӵ� ����")]
    [SerializeField] float _maxSpeed = 3f;

    [Header("���� �Ÿ�")]
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
        // Z ����
        Vector3 fixedPos = transform.position;
        fixedPos.z = 0;
        transform.position = fixedPos;

        // Ÿ�� ���� ���̸� SetDestination �ּ� ȣ��
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

        _lastTargetPos = Vector3.positiveInfinity; // ������ ó���� SetDestination
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