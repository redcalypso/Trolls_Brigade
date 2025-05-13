using UnityEngine;

public class MonsterMove : MonoBehaviour
{

    MonsterNavAgent _nav;
    Animator _ani;
    Transform _target;

    public Transform Target => _target;
    void Awake()
    {
        _nav = transform.GetComponentInParent<MonsterNavAgent>();
        _ani = transform.GetComponentInParent<Animator>();
    }
    void FixedUpdate()
    {
        CheckDistance();
        PTSDAni.UpdateTroller(_ani, _nav.Direction, _nav.NavMeshAgent.velocity.magnitude);
    }
    public void SetTarget(Transform target)
    { 
        _target = target;
    }
    public void Death()
    { gameObject.SetActive(false); }
    public void ActiveTroller()
    {
        _ani.SetBool("Is Active", true);
    }
    void CheckDistance()
    {
        if (_target != null)
        {
            float distance = Vector2.Distance(_target.position, transform.position);

            if (distance > _nav.EventDistance)
            {
                ActiveTroller();
                _nav.SetTarget(_target); 
            }
            else
            { _nav.StopMove(); }
        }
    }
}
