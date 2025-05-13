using UnityEngine;
using UnityEngine.UI;

public class SettingData
{
    public float BGMVolume;
    public float SFXVolume;
    public SettingData(float bgmVolume, float sfxVolume)
    {
        BGMVolume = bgmVolume;
        SFXVolume = sfxVolume;
    }
}
public class UI_Setting : MonoBehaviour
{
    public static UI_Setting Instance;

    private SettingData _data;
    private const string SAVE_KEY = "Settings";

    public Slider BGMVolumeSlider;
    public Slider SFXVolumeSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Load();
    }
    private void Start()
    {
        SoundManager.Instance.SetBGMVolume(BGMVolumeSlider.value);
        gameObject.SetActive(false);
    }
    void Save()
    {
        _data.BGMVolume = BGMVolumeSlider.value;
        _data.SFXVolume = SFXVolumeSlider.value;
        string jsonData = JsonUtility.ToJson(_data);
        PlayerPrefs.SetString(SAVE_KEY, jsonData);
    }
    void Load()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            _data = JsonUtility.FromJson<SettingData>(jsonData);
        }
        else
        {
            _data = new SettingData(0.5f, 0.5f);
        }
        BGMVolumeSlider.value = _data.BGMVolume;
        SFXVolumeSlider.value = _data.SFXVolume;
    }

   public void OnBGMVolumeChanged()
    {
        SoundManager.Instance.SetBGMVolume(BGMVolumeSlider.value);
        Save();
    }

    public void OnSFXVolumeChanged()
    {
        SoundManager.Instance.SetSFXVolume(SFXVolumeSlider.value);
        Save();
    }
}
