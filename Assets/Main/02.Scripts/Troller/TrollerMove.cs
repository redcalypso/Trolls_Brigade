using UnityEngine;
using UnityEngine.AI;

public class TrollerMove : MonoBehaviour
{
    [SerializeField] GameObject _giveIcon;

    NavManager _nav;
    TrollerStateManager _stateManager;
    Animator _ani;
    Rigidbody2D _rigi;
    Transform _target;

    public GameObject GiveIcon => _giveIcon;
    public TrollerStateManager StateManager => _stateManager;
    public Transform Target => _target;
    void Awake()
    {
        _nav = transform.GetComponentInParent<NavManager>();
        _stateManager = transform.GetComponentInParent<TrollerStateManager>();
        _ani = transform.GetComponentInParent<Animator>();
        _rigi = transform.GetComponentInParent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        CheckDistance();
        PTSDAni.UpdateTroller(_ani, _nav.Direction, _nav.NavMeshAgent.velocity.magnitude);
        if (_rigi.linearVelocity.magnitude > 0 && _stateManager.TrollerState != TrollerState.Casting)
        { _rigi.linearVelocity = Vector2.zero; }
    }
    public void SetTarget(Transform target)
    { _target = target; }
    public void ActiveTroller()
    {
        if(_stateManager.TrollerState == TrollerState.Disabled)
        {
            _stateManager.SetState(TrollerState.Standby);
            _nav.SetAvoidanceType(ObstacleAvoidanceType.LowQualityObstacleAvoidance);
            _ani.SetBool("Is Active", true);
        }
    }
    void CheckDistance()
    {
        if (_target != null && _stateManager.TrollerState != TrollerState.Casting)
        {
            float distance = Vector2.Distance(_target.position, transform.position);

            if (distance > _nav.EventDistance)
            { _nav.Move(_target); }
            else
            {
                _target = null;
                _nav.StopMove(); 
            }
        }
    }
}

