using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using System;
using MoreMountains.Feedbacks;
public class UI_DyingMessage : MonoBehaviour
{
    [SerializeField] List<MMF_Player> Kill_Animation;
    [SerializeField] List<Image> _images;
    [SerializeField] List<string> _names;

    public static UI_DyingMessage Instance = null;

    public Action ShowAction;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        else Destroy(gameObject);
    }

    private void Start()
    {
        MaessageUpdate();
    }

    public void MaessageUpdate()
    {
        string KillerName = PlayerData.Instance.KiilerName;
        if (KillerName == null)
        {

            _images[0].gameObject.SetActive(true);
            ShowAction?.Invoke();
            return;
        }

        if (KillerName.Contains(_names[1])) // 불 마법
        {
            Kill_Animation[1].PlayFeedbacks();            
            Debug.Log(KillerName);
        }
        else if(KillerName.Contains(_names[2])) // 통
        {
            Kill_Animation[2].PlayFeedbacks();
            Debug.Log(KillerName);
        }
        else if(KillerName.Contains(_names[3])) // 몬스터
        {
            Kill_Animation[3].PlayFeedbacks();
            Debug.Log(KillerName);
        }
        else // 혹시 모를 예외
        {
            Kill_Animation[3].PlayFeedbacks();
            Debug.Log("whattttt?");
        }

        ShowAction?.Invoke();
    }
}
