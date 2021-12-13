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
    private int currentIndex = 0;

    private float maxVolume = 1f;
    private float minVolume = 0f;

    private void Awake()
    {
        currentIndex = GameController.Instance.currentStage;
    }

    public void ChangeAudioBackgroundForward(){
        StartCoroutine(ChangeClip(audioClip[currentIndex+1]));
        currentIndex += 1;
    }

    public void ChangeAudioBackgroundBackward(){
        StartCoroutine(ChangeClip(audioClip[currentIndex-1]));
        currentIndex -= 1;
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

    void Start(){
        Destroy(GameObject.FindGameObjectWithTag("MainThemeSong"));
    }
    
}


