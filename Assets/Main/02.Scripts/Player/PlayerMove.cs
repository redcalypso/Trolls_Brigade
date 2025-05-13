using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Color _color;
    PlayerInGameData _playerData;
    Animator _ani;
    Rigidbody2D _body;
    Vector2 _lastInput = Vector2.zero;

    void Awake()
    {
        _playerData = GetComponent<PlayerInGameData>();
        _ani = GetComponent<Animator>();
        _body = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        AnimatorClipInfo[] clipInfo = _ani.GetCurrentAnimatorClipInfo(0);
        if (clipInfo[0].clip.name.Contains("death"))
        {
            _body.linearVelocity = Vector2.zero;
        }
        else
        {
            Move(_body, InputValue(), _playerData.MoveSpeed * _playerData.SpeedValue);
            PTSDAni.UpdateAny(_ani, InputValue());
            MoveInputOneFrame();
        }
    }
    void Move(Rigidbody2D body, Vector2 dir, float speed)
    {
        if (dir.magnitude > 0.1f)
        { dir.Normalize(); }

        else
        { dir = Vector2.zero; }

        body.linearVelocity = dir * speed;
    }
    Vector2 InputValue()
    {
        Vector2 dir;
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.y = Input.GetAxisRaw("Vertical");

#if UNITY_ANDRIOD
        dir.x = Joystick.Horizontal;
        dir.y = Joystick.Vertical;
#endif
        return dir;
    }
    public void SlowPlayer(Collider2D collision)
    {
        StartCoroutine(C_IceEffect(collision));
    }
    IEnumerator C_IceEffect(Collider2D collision)
    {
        PlayerInGameData.Instance.SetSpeedValue(0.5f);
        collision.GetComponent<SpriteRenderer>().color = _color;

        yield return new WaitForSeconds(1f);

        PlayerInGameData.Instance.SetSpeedValue(1f);
        collision.GetComponent<SpriteRenderer>().color = Color.white;
    }
    void MoveInputOneFrame()
    {
        Vector2 currentInput = InputValue();

        // �̵� ���� (���� �Է��� 0�ε� ���� �Է��� ����)
        if (_lastInput == Vector2.zero && currentInput != Vector2.zero)
        {
            Move(_body, currentInput, _playerData.MoveSpeed * _playerData.SpeedValue);
            PTSDAni.UpdateAny(_ani, currentInput); // �ִϸ��̼� ���ŵ� ���⼭��
        }
        // �̵� �ߴ� (���� �Է��� �־��µ� ���� 0)
        else if (_lastInput != Vector2.zero && currentInput == Vector2.zero)
        {
            Move(_body, Vector2.zero, 0f);
            PTSDAni.UpdateAny(_ani, Vector2.zero);
        }

        _lastInput = currentInput; // ���� �Է� ����
    }
}
