using UnityEngine;

public class ObjectEffectCol : MonoBehaviour
{
    [SerializeField] ObjectType _objectType;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(_objectType == ObjectType.Boom)
            {
                PlayerData.Instance.SetDyingMessage(transform.GetComponent<Collider2D>());
                LevelManager.Instance.LoadLodingLevel();
            }
            if (_objectType == ObjectType.Ice)
            { collision.GetComponent<PlayerMove>().SlowPlayer(collision); }
        }
        if (collision.CompareTag("Object"))
        { collision.GetComponent<ObjectInteraction>().ActiveObject(); }
        if (collision.CompareTag("Monster")&& _objectType == ObjectType.Boom)
        {
            collision.GetComponent<MonsterMove>().SetTarget(null);
            collision.GetComponent<MonsterNavAgent>().StopNavMesh();
            collision.GetComponent<Animator>().SetTrigger("Death");
        }
    }
}
