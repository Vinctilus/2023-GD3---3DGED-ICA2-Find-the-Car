using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Volumes")]
    [SerializeField] 
    AudioSource musicVolume;
    [SerializeField] 
    AudioSource backgroundVolume;
    [SerializeField] 
    AudioSource streetNoiseVolume;
    [SerializeField] 
    AudioSource sfxVolume;

    [Header("OBJ")]
    [SerializeField] 
    GameObject soundPlayer;
    [SerializeField]
    TrafficManager carsManager;

    [Header("Resurecs")]
    public AudioClip musicTrack;
    public AudioClip backgroundClip;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public List<AudioClip> streetSounds;

    [Header("Rendom")]
    public float minRandomTime = 1f;
    public float maxRandomTime = 5f;
    public float randomTimeElapsed = 0f;
    public float randomTime = 5f;



    private void Start()
    {
        musicVolume.clip = musicTrack;
        musicVolume.Play();

        backgroundVolume.clip = backgroundClip;
        backgroundVolume.Play();
    }
    private void Update()
    {
        
        streetNoiseVolume.volume = backgroundVolume.volume * 0.9f;

        randomTimeElapsed = randomTimeElapsed+Time.deltaTime;
        if (randomTimeElapsed > randomTime) {
            playrandomBKG();
            randomTimeElapsed = 0;
            randomTime = Random.Range(minRandomTime, maxRandomTime);
        }

    }

    [Button("playSFX")]
    public void playSFX(AudioClip clip)
    {
        creatplayer(sfxVolume, clip, transform,false);
    }


    [Button("Redomesonde")]
    public void playrandomBKG()
    {
        GameObject obj = carsManager.transform.GetChild(Random.Range(0,carsManager.transform.childCount-1)).gameObject;
        Transform tr =  obj.transform;
        AudioClip clip = streetSounds[Random.Range(0, streetSounds.Count - 1)];
        creatplayer(streetNoiseVolume, clip, tr);
    }

    public void creatplayer(AudioSource copy,AudioClip clip,Transform tr, bool posioedsound = true)
    {
        GameObject Player = Instantiate(soundPlayer, tr.position, tr.rotation);
        Player.transform.parent = gameObject.transform;
        Player.GetComponent<PlayAudio>().Play(copy, clip, posioedsound);
    }
}   

