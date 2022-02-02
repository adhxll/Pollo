using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Pitch.Algorithm;

public class SettingsView : SettingsElement
{
    [Header("Settings Component")]
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private Slider soundEffectsSlider = null;
    [SerializeField] private TMP_Dropdown pitchAlgoDropdown = null;
    [SerializeField] private Slider delaySlider = null;
    // [SerializeField] private TMP_Dropdown languageDropdown = null; // draft
    // [SerializeField] private Toggle notificationToggle = null; // draft

    [Space]
    [Header("UI Buttons")]
    [SerializeField] private GameObject closeButton = null;
    [SerializeField] private GameObject saveButton = null;

    [Space]
    [Header("Misc")]
    [SerializeField] private TextMeshProUGUI pitchValue = null;
    // [SerializeField] private TextMeshProUGUI toggleText = null; //draft

    public float GetMusicValue() { return musicSlider.value; }
    public float GetSoundEffectsValue() { return soundEffectsSlider.value; }
    public float GetDelayValue() { return delaySlider.value; }
    public int GetAlgorithmValue() { return pitchAlgoDropdown.value; }
    // private int GetLanguageValue() { return languageDropdown.value; }
    // private int GetNotificationValue() { return notificationToggle.isOn ? 1 : 0; }

    void Awake()
    {
        Initialize();
    }
     // initialize the value on display based on previously saved values on playerprefs
    private void Initialize()
    {
        SetMusicValue(app.model.GetMusicPlayerPrefs());
        SetSoundEffectsValue(app.model.GetSoundEffectsPlayerPrefs());
        InitializeAlgorithmDropdown();
        SetDelayValue(app.model.GetDelayPlayerPrefs());
        //SetLanguageValue();
        //SetNotificationValue((PlayerPrefs.GetInt(SettingsList.Notification.ToString(), 1) == 1)? true : false);
    }

    // connect this to slider
    public void SetMusicValue(float newValue)
    {
        musicSlider.value = newValue;
        GameController.Instance.masterMixer.SetFloat("musicVolume", app.controller.GetDecibelLogValue(newValue));
    }

    // connect this to slider
    public void SetSoundEffectsValue(float newValue)
    {
        soundEffectsSlider.value = newValue;
        GameController.Instance.masterMixer.SetFloat("soundEffects", app.controller.GetDecibelLogValue(newValue));
    }

    void InitializeAlgorithmDropdown()
    {
        string[] enumNames = Enum.GetNames(typeof(PitchAlgo));
        List<string> names = new List<string>(enumNames);
        pitchAlgoDropdown.AddOptions(names);
        SetAlgorithmValue(app.model.GetAlgorithmPlayerPrefs());
    }

    public void SetAlgorithmValue(int index)
    {
        pitchAlgoDropdown.value = index;
        pitchAlgoDropdown.captionText.text = Enum.GetName(typeof(PitchAlgo), index);
    }

    public void SetDelayValue(float newValue)
    { 
        delaySlider.value = newValue;
        pitchValue.text = newValue.ToString("0.00");
    }

    // public void SetLanguageValue(){}
    // public void SetNotificationValue(bool isToggled) // 1 = True, 0 = False
    // {
    //     notificationToggle.isOn = isToggled;
    //     toggleText.text = isToggled ? "ON" : "OFF";
    // }

    public void OnClickCloseButton(){
        ButtonController.OnButtonClick(closeButton);
        app.controller.CloseSettings();
    }

    public void OnClickSaveButton(){
        ButtonController.OnButtonClick(saveButton);
        app.controller.SaveAllSettings();
        app.controller.CloseSettings();
    }
}
