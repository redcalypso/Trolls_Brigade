using DG.Tweening;
using UnityEngine;

public class UI_SettingsButton : MonoBehaviour
{
    public GameObject SettingUI;
    public Vector3 OriginalScale;

    public void TurnOnAndOff()
    {
        Vector3 originalScale = OriginalScale;
        if (SettingUI.activeInHierarchy == false)
        {
            SettingUI.SetActive(true);
            SettingUI.transform.DOKill(true);
            SettingUI.transform.DOScale(originalScale * 1.2f, 0.3f)
                .SetEase(Ease.OutBack) // 1.2�� Ŀ��
            .OnComplete(() => SettingUI.transform.DOScale(originalScale, 0.1f)
            .SetEase(Ease.InOutSine)); // ���� ũ���

        }
        else
        {
            SettingUI.transform.DOKill(true);
            SettingUI.transform.DOScale(0, 0.2f).SetEase(Ease.InBack)
            .OnComplete(() => SettingUI.SetActive(false));
        }
    }

    void Start()
    {
        OriginalScale = SettingUI.transform.localScale;
    }

    void Update()
    {
        
    }
}
