using UnityEngine;

public class TrollerSkillCol : MonoBehaviour
{
    [SerializeField] SkillType _skillType;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") 
            && (_skillType == SkillType.Fire || _skillType == SkillType.Ice || _skillType == SkillType.Turn))
        {
            collision.GetComponent<MonsterMove>().SetTarget(null);
            collision.GetComponent<MonsterNavAgent>().StopNavMesh();
            collision.GetComponent<Animator>().SetTrigger("Death"); 
        }

        if (collision.CompareTag("Player"))
        {
            if (_skillType == SkillType.Fire)
            {
                PlayerData.Instance.SetDyingMessage(transform.GetComponent<Collider2D>());
                LevelManager.Instance.LoadLodingLevel();
            }
            if (_skillType == SkillType.Ice)
            { collision.GetComponent<PlayerMove>().SlowPlayer(collision); }
        }

        if (collision.CompareTag("Object"))
        { collision.GetComponent<ObjectInteraction>().ActiveObject(); }
    }
}
