using UnityEngine;

public enum TrollerState
{
    Disabled,
    Standby,
    Following,
    Casting
}
public class TrollerStateManager : MonoBehaviour
{
    [SerializeField] TrollerState _trollerState;
    public TrollerState TrollerState => _trollerState;

    public void SetState(TrollerState trollerState)
    { _trollerState = trollerState; }
}
