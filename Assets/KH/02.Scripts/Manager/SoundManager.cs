using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource BGMSource;

    public List<AudioSource> SFXs;
    public int SFXSourceCount = 5;

    public float BgmVolume = 0.5f;
    public float SfxVolume = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // BGM 오디오 소스 초기화
        if (BGMSource == null)
        {
            BGMSource = gameObject.AddComponent<AudioSource>();
            BGMSource.loop = true;
        }

    }

    public void SetBGMVolume(float volume)
    {
        BgmVolume = Mathf.Clamp01(volume);
        BGMSource.volume = BgmVolume;
    }
    public void SetSFXVolume(float volume)
    {
        SfxVolume = Mathf.Clamp01(volume);

        // 모든 SFX 소스의 볼륨 업데이트
        foreach (AudioSource source in SFXs)
        {
            source.volume = SfxVolume;
        }
    }
}
