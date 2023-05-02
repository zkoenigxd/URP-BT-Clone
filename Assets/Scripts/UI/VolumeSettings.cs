using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider mainSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider musicSlider;

    public const string MIXER_MAIN = "MainVolume";
    public const string MIXER_SFX = "SFXVolume";
    public const string MIXER_MUSIC = "MusicVolume";

    float mainTemp;
    float SFXTemp;
    float musicTemp;

    private void Awake()
    {
        mainSlider.onValueChanged.AddListener(SetMainVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    private void OnEnable()
    {
        mainTemp = mainSlider.value;
        SFXTemp = SFXSlider.value;
        musicTemp = musicSlider.value;

    }

    private void Start()
    {
        mainSlider.value = PlayerPrefs.GetFloat(AudioManager.MAIN_KEY, 1f);
        SFXSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
    }

    private void SetPrefs()
    {
        PlayerPrefs.SetFloat(AudioManager.MAIN_KEY, mainSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, SFXSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
    }

    void SetMainVolume(float value)
    {
        audioMixer.SetFloat(MIXER_MAIN, Mathf.Log10(value) * 20);
    }

    void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }

    void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }

    public void ConfirmChanges()
    {
        SetPrefs();
        gameObject.SetActive(false);
    }

    public void CancelChanges()
    {
        mainSlider.value = mainTemp;
        SFXSlider.value = SFXTemp;
        musicSlider.value = musicTemp;
        gameObject.SetActive(false);
    }
}
