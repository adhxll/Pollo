using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSModalUIManager : MonoBehaviour
{
    [SerializeField] private GameObject modal = null;
    [SerializeField] private GameObject practiceButton, playButton;
    [SerializeField] private TMPro.TextMeshProUGUI levelText, scoreText, accuracyText = null;

    // Start is called before the first frame update
    void Start()
    {
        SetModalValues();
    }

    public void PlayButton()
    {
        AudioController.Instance.PlayButtonSound();
        SetSceneToPlay(SceneStateManager.SceneState.Instruction);
        AnimationUtilities.Instance.AnimateButtonPush(playButton, () => SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GameScene));
    }

    public void PracticeButton()
    {
        AudioController.Instance.PlayButtonSound();
        SetSceneToPlay(SceneStateManager.SceneState.Practice);
        AnimationUtilities.Instance.AnimateButtonPush(playButton, () => SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GameScene));
    }

    public void CloseModal()
    {
        AudioController.Instance.PlayButtonSound();
        SceneManagerScript.Instance.SceneUnload(SceneManagerScript.SceneName.LSModal);
    }

    private void SetModalValues()
    {
        levelText.text = ModalController.Instance.levelValue;
        scoreText.text = ModalController.Instance.scoreValue;
        accuracyText.text = ModalController.Instance.accuracyValue;

        var starCount = modal.GetComponent<StarCounter>();

        starCount.StarCount = ModalController.Instance.starCount;
        starCount.FillStars();
    }

    private void SetSceneToPlay(SceneStateManager.SceneState scene)
    {
        GameController.Instance.sceneState = scene;
    }
}
