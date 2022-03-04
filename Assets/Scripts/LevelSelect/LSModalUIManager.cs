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
        SetSceneToPlay(GameSceneState.Instruction);
        AnimationUtilities.Instance.AnimateButtonPush(playButton, () => SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GameScene));
    }

    public void PracticeButton()
    {
        AudioController.Instance.PlaySound(SoundNames.click);
        SetSceneToPlay(GameSceneState.Practice);
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

    private void SetSceneToPlay(GameSceneState scene)
    {
        GameController.Instance.sceneState = scene;
    }
}
