using UnityEngine;

public class TrollerSkillTriggerCol : MonoBehaviour
{
    [SerializeField] float _coolDown = 4f;

    TrollerSkill _trollerSkill;
    float time;
    void Awake()
    { 
        _trollerSkill = transform.GetComponentInParent<TrollerSkill>();
        time = 2f;
    }
     void Update()
    {
        if(time <= _coolDown && _trollerSkill.StateManager.TrollerState != TrollerState.Casting
            && _trollerSkill.StateManager.TrollerState != TrollerState.Disabled)
        { time += Time.deltaTime; }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        TrollerState state = _trollerSkill.StateManager.TrollerState;

        if (time >=_coolDown && state != TrollerState.Disabled && state != TrollerState.Casting)
        {
            if(collision.CompareTag("Monster"))
            {
                if (_trollerSkill.SkillType == SkillType.Fire)
                {
                    time = 0;
                    _trollerSkill.SkillFire(collision.transform); 
                }
                else if (_trollerSkill.SkillType == SkillType.Ice)
                {
                    time = 0;
                    _trollerSkill.SkillIce(collision.transform); 
                }
                else if (_trollerSkill.SkillType == SkillType.Turn)
                {
                    time = 0;
                    _trollerSkill.SkillTurn(collision.transform); 
                }
            }
            else if (collision.CompareTag("Object"))
            {
                if (_trollerSkill.SkillType == SkillType.Fire && collision.GetComponent<ObjectInteraction>().ObjectType == ObjectType.Boom)
                {
                    time = 0;
                    _trollerSkill.SkillFire(collision.transform); }
                else if(_trollerSkill.SkillType == SkillType.Ice && collision.GetComponent<ObjectInteraction>().ObjectType == ObjectType.Ice)
                {
                    time = 0;
                    _trollerSkill.SkillIce(collision.transform); 
                }
                else if (_trollerSkill.SkillType == SkillType.Turn && collision.GetComponent<ObjectInteraction>().ObjectType == ObjectType.Breakable)
                {
                    time = -2;
                    _trollerSkill.SkillTurn(collision.transform);
                }
            }
        }
    }
}
