using UnityEngine;

public class MonsterFollowCol : MonoBehaviour
{
    MonsterMove _monster;
    void Awake()
    { _monster = transform.GetComponentInParent<MonsterMove>(); }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _monster.SetTarget(collision.transform);
        }
    }
}
