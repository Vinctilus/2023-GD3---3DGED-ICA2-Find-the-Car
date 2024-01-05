using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class playaudio : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

   
    // Update is called once per frame
    void Update(){
        if (!audioSource.isPlaying)    {

            Destroy(gameObject);
        }
      

    }

    public void play(AudioSource copy, AudioClip play, bool Posionsound)
    {
        audioSource.outputAudioMixerGroup = copy.outputAudioMixerGroup; 
        audioSource.clip = play;
        audioSource.Play();
        if( Posionsound ) { audioSource.spatialBlend = 1.0f; }
}

}
    
