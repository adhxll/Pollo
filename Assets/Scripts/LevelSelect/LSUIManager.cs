using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowDirection{
    previous,
    next
}


// Level Selection - UI Manager
public class LSUIManager : MonoBehaviour
{
    [SerializeField] private GameObject homeButton = null;
    [SerializeField] private GameObject settingButton = null;
    [SerializeField] private GameObject previousButton = null;
    [SerializeField] private GameObject nextButton = null;
    
    [SerializeField] private AudioClip[] audioClip = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private float transitionDuration = 0;
    private int currentIndex = 0;

    private float maxVolume = 1f;
    private float minVolume = 0f;

    private void Awake()
    {
        currentIndex = GameController.Instance.currentStage;
    }

    void Start(){
        Destroy(GameObject.FindGameObjectWithTag("MainThemeSong"));
        audioSource.clip = audioClip[currentIndex];
        audioSource.Play();
    }

    public void ChangeAudioBackground(int arrowDirection){
        AudioController.Instance.PlaySound(SoundNames.click);
        switch (arrowDirection)
        {
            case 0:
                ButtonController.OnButtonClick(previousButton);
                StartCoroutine(ChangeClip(audioClip[currentIndex - 1]));
                StageConstructor.Instance.ChangeStagePrev();
                currentIndex -= 1;
                break;
            case 1:
                ButtonController.OnButtonClick(nextButton);
                StartCoroutine(ChangeClip(audioClip[currentIndex + 1]));
                StageConstructor.Instance.ChangeStageNext();
                currentIndex += 1;
                break;
        }

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

    public void GoToHome()
    {
        ButtonController.OnButtonClick(homeButton); 
        AnimationUtilities.Instance.AnimateButtonPush(homeButton, () => SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.Homepage));
    }

    // Additive scene
    public void ShowModal()
    {
        AudioController.Instance.PlaySound(SoundNames.click);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.LSModal, true);
    }
}
