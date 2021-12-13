using System;
using System.Collections;
using System.Collections.Generic;
using Pitch.Algorithm;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GSUIManager : MonoBehaviour
{
    public static GSUIManager Instance;

    [Header("Overlay Objects")]
    [SerializeField] private Image background = null;
    [SerializeField] private bool animateBackground = true;
    private List<GameObject> objects = new List<GameObject>();

    [Space]
    //[SerializeField] private Dropdown dropdown = null;
    //[SerializeField] private Slider delaySlider = null;
    [SerializeField] private Slider pitchSwitch = null;
    [SerializeField] private Slider repeatSwitch = null;
    //[SerializeField] private TMPro.TextMeshProUGUI sliderValue = null;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        // Check & Animate Background
        if (background != null)
            AnimateStart();
    }

    void AnimateStart()
    {
        // Fade background on start
        if (animateBackground)
        {
            background.DOFade(0f, 0f);
            background.DOFade(0.75f, 0.5f);
        }

        //Check UI child and assign them to objects
        foreach (Transform child in background.transform)
        {
            objects.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        if (objects != null)
            StartCoroutine(AnimationUtilities.Instance.AnimateObjects(objects, 0.1f, AnimationUtilities.AnimationType.MoveY, 0f, Screen.height));
    }

    #region PauseMenu

    public void ShowPause()
    {
        AudioController.Instance.PlaySound(SoundNames.click);
        SceneStateManager.Instance.ChangeSceneState(SceneStateManager.SceneState.Pause, false);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GSPause, true);
        SongManager.Instance.PauseSong();
    }

    public void RestartGame()
    {
        AudioController.Instance.PlaySound(SoundNames.click);
        SceneManagerScript.Instance.SceneUnload(SceneManagerScript.SceneName.GSPause);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GameScene);
    }

    public void ResumeGame()
    {
        AudioController.Instance.PlaySound(SoundNames.click);
        SceneStateManager.Instance.ChangeSceneState(SceneStateManager.SceneState.Countdown, false);
        SceneManagerScript.Instance.SceneUnload(SceneManagerScript.SceneName.GSPause);
        SongManager.Instance.ResumeSong();
    }

    //public void referSound(string sound){
    //    GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().PlaySound(sound);
    //}

    public void GoToLevelSelection()
    {
        AudioController.Instance.PlaySound(SoundNames.click);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.LevelSelection);
    }

    #endregion

    public void ClosePianoScale()
    {
        AudioController.Instance.PlaySound(SoundNames.click);
        SceneStateManager.Instance.ChangeSceneState(SceneStateManager.SceneState.Countdown);
        SceneManagerScript.Instance.SceneUnload(SceneManagerScript.SceneName.GSPianoScale);
    }

    // Forced Pitch - Switch
    public void SetForcedPitch()
    {
        pitchSwitch.onValueChanged.AddListener((v) =>
        {
            int value = (int)pitchSwitch.value;
            PlayerPrefs.SetInt("isForcedPitch", value);
        });
    }

    // Repeat Section - Switch
    public void SetRepeatSection()
    {
        repeatSwitch.onValueChanged.AddListener((v) =>
        {
            int value = (int)repeatSwitch.value;
            PlayerPrefs.SetInt("isRepeatSection", value);
        });
    }
}
