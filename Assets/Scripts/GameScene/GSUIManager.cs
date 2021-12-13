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
    [SerializeField] private Dropdown dropdown = null;
    [SerializeField] private Slider delaySlider = null;
    [SerializeField] private Slider pitchSwitch = null;
    [SerializeField] private Slider repeatSwitch = null;
    [SerializeField] private TMPro.TextMeshProUGUI sliderValue = null;

    // Set slider value
    private bool forcePitch
    {
        get
        {
            var v = PlayerPrefs.GetInt(SettingsList.ForcePitch.ToString());
            return v == 0 ? false : true;
        }

        set
        {
            var v = SettingsList.ForcePitch.ToString();
            var i = value ? 1 : 0;
            PlayerPrefs.SetInt(v, i);
            pitchSwitch.value = i;
        }
    }

    private bool repeatSection
    {
        get
        {
            var v = PlayerPrefs.GetInt(SettingsList.RepeatSection.ToString());
            return v == 0 ? false : true;
        }

        set
        {
            var v = SettingsList.RepeatSection.ToString();
            var i = value ? 1 : 0;
            PlayerPrefs.SetInt(v, i);
            repeatSwitch.value = i;
        }
    }

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
        // Check Dropdown
        if (dropdown != null)
            LoadList();

        // Check Slider
        if (delaySlider != null)
        {
            LoadDelay();
            SetDelay();
        }

        if (pitchSwitch != null && repeatSwitch != null)
            LoadPractice();

        // Check & Animate Background
        if (background != null)
            AnimateStart();
    }

    void LoadPractice()
    {
        pitchSwitch.value = Convert.ToInt32(forcePitch);
        repeatSwitch.value = Convert.ToInt32(repeatSwitch);
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
        AudioController.Instance.PlayButtonSound();
        SceneStateManager.Instance.ChangeSceneState(SceneStateManager.SceneState.Pause, false);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GSPause, true);
        SongManager.Instance.PauseSong();
    }

    public void RestartGame()
    {
        AudioController.Instance.PlayButtonSound();
        SceneManagerScript.Instance.SceneUnload(SceneManagerScript.SceneName.GSPause);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GameScene);
    }

    public void ResumeGame()
    {
        AudioController.Instance.PlayButtonSound();
        SceneStateManager.Instance.ChangeSceneState(SceneStateManager.SceneState.Countdown, false);
        SceneManagerScript.Instance.SceneUnload(SceneManagerScript.SceneName.GSPause);
        SongManager.Instance.ResumeSong();
    }

    public void GoToLevelSelection()
    {
        AudioController.Instance.PlayButtonSound();
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.LevelSelection);
    }

    // Pause Menu - Dropdown
    void LoadList()
    {
        string[] enumNames = Enum.GetNames(typeof(PitchAlgo));
        List<string> names = new List<string>(enumNames);
        dropdown.AddOptions(names);
        dropdown.value = PlayerPrefs.GetInt("pitchAlgo");
    }

    public void DropdownValueChanged(int index)
    {
        dropdown.value = index;
        PlayerPrefs.SetInt("pitchAlgo", index);
    }

    // Pause Menu - Slider
    public void LoadDelay()
    {
        var delay = SongManager.Instance.GetNoteDelay();
        sliderValue.SetText(delay.ToString("0.00"));
        delaySlider.value = delay;
    }

    public void SetDelay()
    {
        delaySlider.onValueChanged.AddListener((v) =>
        {
            sliderValue.SetText(v.ToString("0.00"));
            PlayerPrefs.SetFloat("NoteDelay", v);
        });
    }

    #endregion

    public void ClosePianoScale()
    {
        AudioController.Instance.PlayButtonSound();
        SceneStateManager.Instance.ChangeSceneState(SceneStateManager.SceneState.Countdown);
        SceneManagerScript.Instance.SceneUnload(SceneManagerScript.SceneName.GSPianoScale);
    }

    public void ClickForcedPitch()
    {
        if (forcePitch)
            forcePitch = false;
        else
            forcePitch = true;
    }

    public void ClickRepeatSection()
    {
        if (repeatSection)
            repeatSection = false;
        else
            repeatSection = true;
    }
}
