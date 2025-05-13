using UnityEngine;

public class TrollerFollowCol : MonoBehaviour
{
    TrollerMove _troller;
    void Awake()
    { _troller = transform.GetComponentInParent<TrollerMove>(); }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_troller.StateManager.TrollerState != TrollerState.Disabled && _troller.StateManager.TrollerState != TrollerState.Casting)
            {
                _troller.SetTarget(collision.transform);
                _troller.StateManager.SetState(TrollerState.Following);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_troller.StateManager.TrollerState != TrollerState.Disabled && _troller.StateManager.TrollerState != TrollerState.Casting)
            {
                _troller.SetTarget(null);
                _troller.StateManager.SetState(TrollerState.Standby);
            }
        }
    }
}
