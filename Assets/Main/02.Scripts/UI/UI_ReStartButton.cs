using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_ReStartButton : MonoBehaviour
{

    [SerializeField] float _waitTime;

    [SerializeField] TextMeshProUGUI _buttonText;
    [SerializeField]
    [Range(0.5f, 1.5f)] float _buttonScale;
    [SerializeField] float _scaleTime;

    private void Awake()
    {
        InitFade();
        UI_DyingMessage.Instance.ShowAction += ShowButton;
    }

    public void OnClickLoadMainScene()
    {
        LevelManager.Instance.LoadLodingAgainLevel();
    }
    public void InitFade()
    {
        _buttonText.gameObject.SetActive(false);
        Color textColor = _buttonText.color;
        textColor.a = 0f;
        _buttonText.color = textColor;
    }
    public void ShowButton()
    {
        _buttonText.gameObject.SetActive(true);
        _buttonText.DOFade(1f, _waitTime).SetEase(Ease.Linear).OnComplete(() => {
            _buttonText.rectTransform.DOScale(Vector3.one * _buttonScale, _scaleTime).SetLoops(-1, LoopType.Yoyo);
        });
    }
}
