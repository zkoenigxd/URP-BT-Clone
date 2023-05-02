using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer audioMixer;

    public const string MAIN_KEY = "mainVolume";
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "SFXVolume";

    protected override void OnAwake()
    {
        audioMixer = FindObjectOfType<AudioMixer>();
    }

    private void Start()
    {
        LoadVolume();
    }

    void LoadVolume()
    {
        float mainVolume = PlayerPrefs.GetFloat(MAIN_KEY, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float SFXVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        audioMixer.SetFloat(VolumeSettings.MIXER_MAIN, Mathf.Log10(mainVolume) * 20);
        audioMixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(mainVolume) * 20);
        audioMixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(mainVolume) * 20);
    }
}
