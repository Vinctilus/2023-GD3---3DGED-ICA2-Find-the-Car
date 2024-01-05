using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class aodiosettings : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    Slider MaserSlider;
    [SerializeField]
    Slider MusicSlider;
    [SerializeField]
    Slider AbientSlider;
    [SerializeField]
    Slider SFXSlider;
    

    public void setMasterVolume() 
    { 
        setVolume("Master", MaserSlider.value);
    }
    public void setMusicVolume() 
    {
        setVolume("Music", MusicSlider.value);
    }
    public void setAbientVolume() 
    {
        setVolume("Abiente", AbientSlider.value);
    }
    public void setSFXVolume() 
    {
        setVolume("SFX", SFXSlider.value);
    }

    private void setVolume(string name, float volume)
    {
        audioMixer.SetFloat(name, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(name, volume);
    }

    private void Start()
    {
        
        MaserSlider.value = haskey("Master",0.5f);
        MusicSlider.value = haskey("Music");
        AbientSlider.value = haskey("Abiente");
        SFXSlider.value = haskey("SFX");

        setMasterVolume();
        setMusicVolume();
        setAbientVolume();
        setSFXVolume();


    }
    private float haskey(string key, float stvalue =1f) 
    { if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        return stvalue;
    }
}
