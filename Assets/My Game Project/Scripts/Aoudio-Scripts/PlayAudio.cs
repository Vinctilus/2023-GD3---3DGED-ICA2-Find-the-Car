using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class PlayAudio : MonoBehaviour
{
    private AudioSource audioSource;

    
    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }


    void Update(){
        if (!audioSource.isPlaying)    {

            Destroy(gameObject);
        }
      

    }

    public void Play(AudioSource copy, AudioClip clipToPlay, bool isPositionalSound)
    {
        audioSource.outputAudioMixerGroup = copy.outputAudioMixerGroup; 
        audioSource.clip = clipToPlay;
        audioSource.Play();
        if( isPositionalSound ) 
        { 
            audioSource.spatialBlend = 1.0f; 
        }
}

}
    
