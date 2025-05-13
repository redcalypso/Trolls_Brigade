using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Food : MonoBehaviour
{
    public static UI_Food Instance;
    [SerializeField] private Image _foodIcon;
    [SerializeField] private TextMeshProUGUI _countText;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void FoodRefresh(int i)
    {
        _countText.text = i.ToString();
    }
}
