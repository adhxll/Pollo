using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Level Selection Modal - UI Manager
public class LSModalUIManager : MonoBehaviour
{
    [SerializeField] private GameObject modal = null;
    [SerializeField] private GameObject practiceButton = null, playButton = null;
    [SerializeField] private TMPro.TextMeshProUGUI levelText = null, scoreText = null, accuracyText = null;

    // Start is called before the first frame update
    void Start()
    {
        SetModalValues();
    }

    public void PlayButton()
    {
        AudioController.Instance.PlaySound(SoundNames.click);
        SetSceneToPlay(SceneStateManager.SceneState.Instruction);
        AnimationUtilities.Instance.AnimateButtonPush(playButton, () => SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GameScene));
    }

    public void PracticeButton()
    {
        AudioController.Instance.PlaySound(SoundNames.click);
        SetSceneToPlay(SceneStateManager.SceneState.Practice);
        AnimationUtilities.Instance.AnimateButtonPush(practiceButton, () => SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GameScene));
    }

    public void CloseModal()
    {
        PerspectivePan.SetPanning();
        AudioController.Instance.PlaySound(SoundNames.click);
        SceneManagerScript.Instance.SceneUnload(SceneManagerScript.SceneName.LSModal);
    }

    private void SetModalValues()
    {
        levelText.text = ModalController.Instance.levelValue;
        scoreText.text = ModalController.Instance.scoreValue;
        accuracyText.text = ModalController.Instance.accuracyValue;

        var starCount = modal.GetComponent<StarCounter>();

        starCount.starCount = ModalController.Instance.starCount;
        starCount.FillStars();
    }

    private void SetSceneToPlay(SceneStateManager.SceneState scene)
    {
        GameController.Instance.sceneState = scene;
    }

    // use this function whenever a play button is called to skip it duting developer mode
    // It takes one parameter, star. You can skip levels with desired outcome based on star obtained using this function.
    // if you put random number, it will just give you the 0 star results
    // implement this to play, restart and next button.
    private void SkipLevel(int star)
    {
        int totalScore = 0;
        int totalNotes = 100;
        int correctNotes = 0;
        int accuracy = 0;
        if (star == 3){
            accuracy = 100;
            correctNotes = 100;
            totalScore = 10000;
        }
        else if (star == 2){
            accuracy = 60;
            correctNotes = 60;
            totalScore = 1000;
        }
        else if (star == 1){
            accuracy = 30;
            correctNotes = 30;
            totalScore = 100;
        }

        PlayerPrefs.SetInt("SessionScore", totalScore);
        PlayerPrefs.SetInt("SessionTotalNotes", (int)totalNotes);
        PlayerPrefs.SetInt("SessionCorrectNotes", (int)correctNotes); 
        PlayerPrefs.SetInt("SessionAccuracy", accuracy);

        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.ResultPage);
    }
}
