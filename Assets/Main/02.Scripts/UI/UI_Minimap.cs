using UnityEngine;

public class UI_Minimap : MonoBehaviour
{
    PlayerInGameData _playerData;
    RectTransform _rect;
    Vector2 _offset;
    Vector2 _position;

    private void Awake()
    {
        _playerData = GetComponent<PlayerInGameData>();
        _rect = GetComponent<RectTransform>();
        _offset = _rect.localPosition;
    }
    void Update()
    { UpdateMinimap(); }
    void UpdateMinimap()
    {
        _position = new Vector2(_playerData.transform.position.x, _playerData.transform.position.y);
        _rect.localPosition = _offset - _position;
    }
}
