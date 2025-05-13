using UnityEngine;

public class PlayerItemCol : MonoBehaviour
{
    PlayerItemInteraction _playerItem;
    void Awake()
    { _playerItem = transform.GetComponentInParent<PlayerItemInteraction>(); }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Troller"))
        { _playerItem.AddNearTrollerList(collision); }
        if (collision.CompareTag("Item Food"))
        { _playerItem.SetNearItem(collision.gameObject); }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Troller"))
        { _playerItem.FocusNaerTroller(); }
        if (collision.CompareTag("Item Food"))
        { _playerItem.SetNearItem(collision.gameObject); }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Troller"))
        { _playerItem.RemoveNearTrollerList(collision); }
        if (collision.CompareTag("Item Food"))
        { _playerItem.SetNearItem(null); }
    }
}
