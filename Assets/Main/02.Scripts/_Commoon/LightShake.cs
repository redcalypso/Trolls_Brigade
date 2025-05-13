using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;



public class LightShake : MonoBehaviour
{
    [SerializeField] Light2D _light2D;

    [Header("position 구간 설정")]
    [Range(-0.5f, 0.5f)] public float MinPositionX = 0;
    [Range(-0.5f, 0.5f)] public float MaxPositionX = 0;

    [Header("Intensity 구간 설정")]
    [Range(0.5f, 2f)] public float MinIntensity = 1;
    [Range(0.5f, 2f)] public float MaxIntensity = 1;

    [Header("Radius Outer 구간 설정")]
    [Range(0.5f, 2f)] public float MinRadiusOuter = 1;
    [Range(0.5f, 2f)] public float MaxRadiusOuter = 1;

    [Header("값이 높을 수록 느려짐")]
    [Range(0.1f,2f)] public float LateTime = 1f;
    float _timar = 0;

    [Header("값을 바꿀 내용")]
    public bool PositionUpdate;
    public bool IntensityUpdate;
    public bool RadiusOuterUpdate;

    [Header("YoYo")]
    public bool yoyo;

    Vector3 _defaultPosition;
    float _defaultIntensity;
    float _defaultRadiusOuter;
    private bool _loop;
    void Awake()
    {
        _light2D = GetComponent<Light2D>();    
    }
    private void Start()
    {
        _defaultIntensity = _light2D.intensity;
        _defaultRadiusOuter = _light2D.pointLightOuterRadius;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        LightUpdate();
    }

    void LightUpdate()
    {
        if (!yoyo)
        {
            _timar += Time.deltaTime / LateTime;
        }

        if (PositionUpdate)
        {
            transform.localPosition = Vector3.Lerp(Vector3.right * MinPositionX + Vector3.up *_defaultPosition.y, Vector3.right * MaxPositionX + Vector3.up * _defaultPosition.y, _timar);
        }
        else if(!PositionUpdate)
        { transform.localPosition = _defaultPosition; }

        if (IntensityUpdate)
        {
            _light2D.intensity = Mathf.Lerp(MinIntensity * _defaultIntensity, MaxIntensity * _defaultIntensity, _timar);
        }
        else if (!IntensityUpdate)
        { _light2D.intensity = _defaultIntensity; }
        if (RadiusOuterUpdate)
        {
            _light2D.pointLightOuterRadius = Mathf.Lerp(MinRadiusOuter * _defaultRadiusOuter, MaxRadiusOuter * _defaultRadiusOuter, _timar);
        }
        else if (!RadiusOuterUpdate)
        { _light2D.pointLightOuterRadius = _defaultRadiusOuter; }
        if (yoyo)
        {
            if (_loop)
            {
                _timar += Time.deltaTime / LateTime;
            }
            else
            {
                _timar -= Time.deltaTime / LateTime;

            }

            if (_timar >= 1)
            {
                _loop = false;
            }
            else if (_timar <= 0)
            {
                _loop = true;
            }
        }
        else
        {
            if (_timar >= 1)
            {
                _timar = 0;
            }

            if (_timar <= 0)
            {
                _timar = 0;
            }
        }

    }
}
