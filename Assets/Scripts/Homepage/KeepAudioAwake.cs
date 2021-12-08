using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepAudioAwake : MonoBehaviour
{
    public static KeepAudioAwake Instance;
    private AudioSource audioSource;
    private void Awake()
    {
        if(Instance != null) Destroy(gameObject);
        else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
    }
    private void Start(){
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying) audioSource.Play();
    }
 
    public void StopMusic()
    {
        audioSource.Stop();
    }
}
