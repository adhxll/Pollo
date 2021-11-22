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

    private void Awake()
    {
        Instance = this;
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

}
