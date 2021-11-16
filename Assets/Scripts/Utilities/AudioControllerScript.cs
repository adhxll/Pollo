using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControllerScript : MonoBehaviour
{
    public static AudioControllerScript Instance;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip buttonSound;
    [SerializeField]
    private AudioClip countdownSound;

    public void PlayButtonSound()
    {
        audioSource.clip = buttonSound;
        audioSource.Play();
    }

    public void PlayCountdownSound()
    {
        audioSource.clip = countdownSound;
    }

}
