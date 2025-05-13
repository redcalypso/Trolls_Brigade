using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{
    [SerializeField] AudioSource _player;
    [SerializeField] AudioClip _PickItem;
    [SerializeField] AudioClip _giveItem;
    [SerializeField] GameObject[] _monsters;
    List<TrollerMove> _nearTrollerList = new List<TrollerMove>();
    TrollerMove _nearTroller;

    GameObject _nearItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GiveFood();
            PickFood();
        }
    }
    void PickFood()
    {
        if (_nearItem != null)
        {
            _player.clip = _PickItem;
            _player.Play();
            _nearItem.SetActive(false);
            PlayerInGameData.Instance.PickItem();
        }
    }
    public void SetNearItem(GameObject item)
    { _nearItem = item; }
    void GiveFood()
    {
        if (_nearTroller != null && PlayerInGameData.Instance.Food1Count > 0)
        {
            TrollerSkill skill = _nearTroller.GetComponent<TrollerSkill>();

            _player.clip = _giveItem;
            _player.Play();
            PlayerInGameData.Instance.GiveItem();
            if(PlayerInGameData.Instance.GiveCount >= 2)
            {
                _monsters[PlayerInGameData.Instance.GiveCount - 2].SetActive(true);
            }
            _nearTroller.GetComponent<TrollerSkill>().ActiveAutoSkill();
            _nearTroller.ActiveTroller();
            _nearTrollerList.Remove(_nearTroller);
            _nearTroller.GiveIcon.SetActive(false);
            _nearTroller = null;
        }
    }
    public void AddNearTrollerList(Collider2D collision)
    {
        TrollerMove troller = collision.GetComponentInChildren<TrollerMove>();

        if (troller.StateManager.TrollerState == TrollerState.Disabled && !_nearTrollerList.Contains(troller))
        { _nearTrollerList.Add(troller); }
    }
    public void FocusNaerTroller()
    {
        _nearTroller = PTSDMethod.GetClosestTarget(_nearTrollerList, transform.position);

        VisibleGiveFoodIcon();
    }
    public void RemoveNearTrollerList(Collider2D collision)
    {
        TrollerMove troller = collision.GetComponentInChildren<TrollerMove>();

        _nearTrollerList.Remove(troller);
        troller.GiveIcon.SetActive(false);
        _nearTroller = null;
    }
    void VisibleGiveFoodIcon()
    {
        foreach (TrollerMove troller in _nearTrollerList)
        {
            if (_nearTroller != null && troller == _nearTroller)
            { _nearTroller.GiveIcon.SetActive(true); }
            else
            { troller.GiveIcon.SetActive(false); }
        }
    }
}
