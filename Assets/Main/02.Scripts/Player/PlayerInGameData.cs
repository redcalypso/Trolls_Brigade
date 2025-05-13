using UnityEngine;

public class PlayerInGameData : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _speedValue = 1f;
    [SerializeField] int _food1Count;
    [SerializeField] int _giveCount;

    public static PlayerInGameData Instance;
    public int GiveCount => _giveCount;
    public float MoveSpeed => _moveSpeed;
    public float SpeedValue => _speedValue;
    public int Food1Count => _food1Count;
    void Awake()
    { if (Instance == null) Instance = this; }
    private void Start()
    {
        UI_Food.Instance.FoodRefresh(_food1Count);
    }

    public void PickItem()
    { 
        _food1Count++;
        UI_Food.Instance.FoodRefresh(_food1Count);
    }
    public void GiveItem()
    { 
        _food1Count--;
        _giveCount++;
        UI_Food.Instance.FoodRefresh(_food1Count);
    }
    public void SetSpeedValue(float f)
    { _speedValue = f; }
}
