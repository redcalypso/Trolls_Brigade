using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum SkillType
{
    Fire,
    Heal,
    Ice,
    Turn
}
public class TrollerSkill : MonoBehaviour
{
    [SerializeField] GameObject _skillPrefab;
    [SerializeField] AudioSource _skillAudio;
    [SerializeField] GameObject _autoSkillPrefab;
    [SerializeField] ParticleSystem _skillText;
    [SerializeField] SkillType _skillType;

    TrollerMove _trollerMove;
    TrollerStateManager _stateManager;
    NavManager _nav;
    Animator _ani;

    Rigidbody2D _rb;
    BoxCollider2D _collider;
    Vector2 _dashDestination;
    Vector2 _direction;

    public SkillType SkillType => _skillType;
    public TrollerStateManager StateManager => _stateManager;
    void Awake()
    {
        _skillAudio = transform.GetComponent<AudioSource>();
        _ani = transform.GetComponent<Animator>();
        _trollerMove = transform.GetComponent<TrollerMove>();
        _stateManager = transform.GetComponent<TrollerStateManager>();
        _nav = transform.GetComponent<NavManager>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }
    public void ActiveAutoSkill()
    {
        switch (_skillType)
        {
            case SkillType.Heal:
                SKillAutoHeal();
                break;
            case SkillType.Ice:
                SKillAutoIce();
                break;
        }
    }
    public void SkillFire(Transform target)
    { StartCoroutine(C_SkillFire(target)); }
    public void SkillIce(Transform target)
    { StartCoroutine(C_SkillIce(target)); }
    public void SKillAutoHeal()
    { StartCoroutine(C_SKillAutoHeal()); }
    public void SKillAutoIce()
    { StartCoroutine(C_SKillAutoIce()); }
    public void SkillTurn(Transform target)
    { StartCoroutine(C_SkillTurn(target)); }

    IEnumerator C_SkillFire(Transform target)
    {
        _ani.SetTrigger("Casting");
        transform.GetChild(0).gameObject.SetActive(true);
        foreach (Transform child in transform.GetChild(0))
        {
            child.gameObject.SetActive(true);
        }
        StartCoroutine(PlayerEffectOnOff(1f, 1.5f, 0.5f));
        _skillText.Play();
        _stateManager.SetState(TrollerState.Casting);
        _nav.StopMove();
        GameObject skill = Instantiate(_skillPrefab);
        skill.transform.position = target.position + Vector3.up * Random.Range(-1f, 1f) + Vector3.right * Random.Range(-1f, 1f);

        yield return new WaitForSeconds(2f);

        transform.GetChild(0).gameObject.SetActive(false);
        _stateManager.SetState(TrollerState.Standby);
        _nav.Move(_trollerMove.Target);
    }
    IEnumerator C_SkillIce(Transform target)
    {
        _ani.SetTrigger("Casting");
        StartCoroutine(PlayerEffectOnOff(1f, 1f, 0.5f));
        _skillText.Play();
        _stateManager.SetState(TrollerState.Casting);
        _nav.StopMove();
        GameObject skill = Instantiate(_skillPrefab);
        skill.transform.position = target.position;

        yield return new WaitForSeconds(2f);

        _stateManager.SetState(TrollerState.Standby);
        _nav.Move(_trollerMove.Target);
    }
    IEnumerator C_SKillAutoHeal()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10,30));

            _skillAudio.Play();
            _ani.SetTrigger("Casting");

            StartCoroutine(PlayerEffectOnOff(1f, 2f, 0.5f));
            _skillText.Play();
            _stateManager.SetState(TrollerState.Casting);
            _nav.StopMove();
            _skillPrefab.SetActive(true);

            yield return new WaitForSeconds(1f);

            transform.GetChild(0).gameObject.SetActive(false);
            _stateManager.SetState(TrollerState.Standby);
            _nav.Move(_trollerMove.Target);
        }
    }
    IEnumerator C_SKillAutoIce()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10, 15));
            if (_stateManager.TrollerState != TrollerState.Casting)
            {
                _ani.SetTrigger("Casting");
                foreach (Transform child in transform.GetChild(0))
                {
                    child.gameObject.SetActive(true);
                }
                StartCoroutine(PlayerEffectOnOff(1f,1f,0.5f));
                _skillText.Play();
                _stateManager.SetState(TrollerState.Casting);
                _nav.StopMove();
                _autoSkillPrefab.SetActive(true);

                yield return new WaitForSeconds(1f);

                transform.GetChild(0).gameObject.SetActive(false);
                _stateManager.SetState(TrollerState.Standby);
                _nav.Move(_trollerMove.Target);
            }
        }
    }
    IEnumerator C_SkillTurn(Transform target)
    {
        _ani.SetTrigger("Casting");
        transform.GetChild(0).gameObject.SetActive(true);
        _skillPrefab.SetActive(true);
        _nav.StopMove();
        _nav.StopNavMesh();
        _rb.simulated = true;
        _collider.isTrigger = false;
        _skillText.Play();
        _stateManager.SetState(TrollerState.Casting);

        // 대상 방향으로 돌진
        _direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        _dashDestination = _direction * 15f;
        PTSDAni.UpdateTroller(_ani, _direction, _direction.magnitude);

        _rb.linearVelocity = _dashDestination;
        yield return new WaitForSeconds(3f);

        transform.GetChild(0).gameObject.SetActive(false);
        _skillPrefab.SetActive(false);
        _rb.simulated = false;
        _collider.isTrigger = true;
        _stateManager.SetState(TrollerState.Standby);
        _nav.Move(_trollerMove.Target);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (SkillType == SkillType.Turn && collision.gameObject.CompareTag("Wall") == true)
        {
            _skillAudio.Play();
            _direction = Vector2.Reflect(_direction, collision.contacts[0].normal).normalized;
            PTSDAni.UpdateTroller(_ani, _direction, _direction.magnitude);
            _rb.linearVelocity = _direction * 15f;

        }
    }
    IEnumerator PlayerEffectOnOff(float startScale ,float time, float dur)
    {
        transform.GetChild(0).transform.DOScale(startScale, dur);
        yield return new WaitForSeconds(time);
        transform.GetChild(0).transform.DOScale(0f, dur);
    }
}
