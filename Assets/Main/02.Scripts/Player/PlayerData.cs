using UnityEngine;


public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance = null;

    private string _kiilerName;
    public string KiilerName => _kiilerName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDyingMessage(Collider2D collider)
    {
        _kiilerName = collider.transform.root.name;
        print(_kiilerName);
    }
}
