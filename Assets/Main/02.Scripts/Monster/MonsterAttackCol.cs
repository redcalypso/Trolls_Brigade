using UnityEngine;

public class MonsterAttackCol : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && transform.GetComponent<Collider2D>().enabled == true)
        {
            PlayerData.Instance.SetDyingMessage(transform.GetComponent<Collider2D>());
            LevelManager.Instance.LoadLodingLevel();
        }
    }
}
