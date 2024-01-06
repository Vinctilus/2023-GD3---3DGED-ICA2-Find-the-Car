using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("AudioMixer")]
    [SerializeField]
    private AudioMixer audioMixer;

    [Header("Volume Sliders")]
    [SerializeField]
    Slider masterVolumeSlider;
    [SerializeField]
    Slider musicVolumeSlider;
    [SerializeField]
    Slider ambientVolumeSlider;
    [SerializeField]
    Slider sfxVolumeSlider;
    

    public void SetMasterVolume() 
    { 
        SetVolume("Master", masterVolumeSlider.value);
    }
    public void SetMusicVolume() 
    {
        SetVolume("Music", musicVolumeSlider.value);
    }
    public void SetAmbientVolume() 
    {
        SetVolume("Abiente", ambientVolumeSlider.value);
    }
    public void SetSFXVolume() 
    {
        SetVolume("SFX", sfxVolumeSlider.value);
    }

    private void SetVolume(string name, float volume)
    {
        audioMixer.SetFloat(name, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(name, volume);
    }

    private void Start()
    {
        
        masterVolumeSlider.value = GetStoredValue("Master",0.5f);
        musicVolumeSlider.value = GetStoredValue("Music");
        ambientVolumeSlider.value = GetStoredValue("Abiente");
        sfxVolumeSlider.value = GetStoredValue("SFX");

        SetMasterVolume();
        SetMusicVolume();
        SetAmbientVolume();
        SetSFXVolume();


    }
    private float GetStoredValue(string key, float stvalue =1f) 
    { if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        return stvalue;
    }
}
