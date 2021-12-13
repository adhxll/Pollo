using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip buttonSound;
    [SerializeField]
    private AudioClip countdownSound;
    [SerializeField]
    private AudioClip coinAddedSound;
    [SerializeField]
    private AudioClip[] starsSound;

    private void Awake()
    {
        if(Instance != null) Destroy(gameObject);
        else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayButtonSound()
    {
        audioSource.clip = buttonSound;
        audioSource.Play();
    }
    
    public void PlayCountDown()
    {
        audioSource.clip = countdownSound;
        audioSource.Play();
    }
    public void PlayCoinAddSound()
    {
        audioSource.clip = coinAddedSound;
        audioSource.Play();
    }

    public void PlayStarSound(int index){
        audioSource.clip = starsSound[index];
        audioSource.Play();
    }

}
