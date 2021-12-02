using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;


// List of settings variable that can be manipulated
// Use these as the playerprefs keys
// made it public so any class can use it becauese it will be referenced by other class
enum SettingsList
{
    Music,
    SoundEffects,
    Language,
    Notification,
};

// class for settings manipulation
public class SettingsController : MonoBehaviour
{
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider soundEffectsSlider;
    [SerializeField]
    private Dropdown languageDropdown;
    [SerializeField]
    private Toggle notificationToggle;
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private TextMeshProUGUI toggleText;
    public static SettingsController Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // initialize the value on display based on previously saved values on playerprefs
    private void Initialize()
    {
        SetVolumeValue(PlayerPrefs.GetFloat(SettingsList.Music.ToString(),0));
        SetSoundEffectsValue(PlayerPrefs.GetFloat(SettingsList.SoundEffects.ToString(), 0));
        //SetLanguageValue();
        //SetNotificationValue((PlayerPrefs.GetInt(SettingsList.Notification.ToString(), 1) == 1)? true : false);
    }

    private float GetVolumeValue()
    {
        return musicSlider.value;
    }

    private float GetSoundEffectsValue()
    {
        return soundEffectsSlider.value;
    }

    private int GetLanguageValue()
    {
        return languageDropdown.value;
    }

    private int GetNotificationValue()
    {
        return notificationToggle.isOn? 1: 0;
    }

    // connect this to slider
    public void SetVolumeValue(float newValue)
    {
        musicSlider.value = newValue;
        mixer.SetFloat("musicVolume", newValue);
        
    }

    // connect this to slider
    public void SetSoundEffectsValue(float newValue)
    {
        soundEffectsSlider.value = newValue;
        mixer.SetFloat("soundEffects", newValue);
    }

    public void SetLanguageValue()
    {

    }

    public void SetNotificationValue(bool isToggled) // 1 = True, 0 = False
    {
        //notificationToggle
        notificationToggle.isOn = isToggled;
        toggleText.text = isToggled?"ON":"OFF";

    }

    private void ResetMixer()
    {
        // Reset the mixer
        mixer.SetFloat("soundEffects", PlayerPrefs.GetFloat(SettingsList.SoundEffects.ToString()));
        mixer.SetFloat("musicVolume", PlayerPrefs.GetFloat(SettingsList.Music.ToString()));
    }

    // only calls this function when the button 'Save' is pressed
    public void SaveAllSettings()
    {
        PlayerPrefs.SetFloat(SettingsList.Music.ToString(), GetVolumeValue());
        PlayerPrefs.SetFloat(SettingsList.SoundEffects.ToString(), GetSoundEffectsValue());
        //PlayerPrefs.SetInt(SettingsList.Language.ToString(), GetLanguageValue());
        //PlayerPrefs.SetInt(SettingsList.Notification.ToString(), GetNotificationValue());
    }

    // a function you call when a setting button is clicked
    public static void InvokeSettings()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    // a function you call when a close button is clicked
    // in this case there's two buttons, the close and save button
    public void CloseSettings()
    {
        ResetMixer();
        SceneManager.UnloadSceneAsync("Settings");

    }

    public static void SetSettingsButton()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("SettingsButton");
        int i = 0;
        foreach (GameObject b in temp)
        {
            Button button = b.GetComponent<Button>();
            button.onClick.RemoveAllListeners(); // initializing the state of the current button so the listener don't add up
            button.onClick.AddListener(InvokeSettings); // adding the InvokeSettings function as listener, so it will open the scene programmatically
            i++;
        }
    }

}
