using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAudioController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClip;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private float transitionDuration;

    private float maxVolume = 1f;
    private float minVolume = 0f;

    public void ChangeAudioBackground(){
        if(audioSource.clip == audioClip[0]) StartCoroutine(ChangeClip(audioClip[1]));
        else StartCoroutine(ChangeClip(audioClip[0]));
    }

    private IEnumerator ChangeClip(AudioClip _audioClip){
        for (float t=0f; t<transitionDuration; t+=Time.deltaTime){
            audioSource.volume = Mathf.Lerp(audioSource.volume, minVolume, t / transitionDuration);
            yield return null;
        }
        audioSource.Stop();
        audioSource.clip = _audioClip;
        audioSource.Play();

        for (float t=0f; t<transitionDuration; t+=Time.deltaTime){
            audioSource.volume = Mathf.Lerp(audioSource.volume, maxVolume, t / transitionDuration);
            yield return null;
        }
    }
}


