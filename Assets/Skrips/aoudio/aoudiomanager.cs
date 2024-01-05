using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class aoudiomanager : MonoBehaviour
{
    [Header("Volumes")]
    [SerializeField] 
    AudioSource MusicVol;
    [SerializeField] 
    AudioSource BackgrundVol;
    [SerializeField] 
    AudioSource StreetnoisVol;
    [SerializeField] 
    AudioSource SFXVolume;

    [Header("OBJ")]
    [SerializeField] 
    GameObject SoundPlayer;
    [SerializeField]
    Carsmanager carsmanager;

    [Header("Resurecs")]
    public AudioClip Music;
    public AudioClip backround;
    public AudioClip Correct;
    public AudioClip Worrng;
    public List<AudioClip> Streatsounds;

    [Header("Rendom")]
    public float minrandomtime = 1f;
    public float maxrandomtime = 5f;
    public float randomtimepast = 0f;
    public float randomtime = 5f;



    private void Start()
    {
        MusicVol.clip = Music;
        MusicVol.Play();

        BackgrundVol.clip = backround;
        BackgrundVol.Play();
    }
    private void Update()
    {
        
        StreetnoisVol.volume = BackgrundVol.volume * 0.9f;

        randomtimepast = randomtimepast+Time.deltaTime;
        if (randomtimepast > randomtime) {
            playrandomBKG();
            randomtimepast = 0;
            randomtime = Random.Range(minrandomtime, maxrandomtime);
        }

    }

    [Button("playSFX")]
    public void playSFX(AudioClip clip)
    {
        creatplayer(SFXVolume, clip, transform,false);
    }


    [Button("Redomesonde")]
    public void playrandomBKG()
    {
        GameObject obj = carsmanager.transform.GetChild(Random.Range(0,carsmanager.transform.childCount-1)).gameObject;
        Transform tr =  obj.transform;
        AudioClip clip = Streatsounds[Random.Range(0, Streatsounds.Count - 1)];
        creatplayer(StreetnoisVol, clip, tr);
    }

    public void creatplayer(AudioSource copy,AudioClip clip,Transform tr, bool posioedsound = true)
    {
        GameObject Player = Instantiate(SoundPlayer, tr.position, tr.rotation);
        Player.transform.parent = gameObject.transform;
        Player.GetComponent<playaudio>().play(copy, clip, posioedsound);
    }
}   

