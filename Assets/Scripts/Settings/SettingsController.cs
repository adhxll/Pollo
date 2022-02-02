using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Pitch.Algorithm;


// class for settings manipulation
public class SettingsController : SettingsElement
{
    public static SettingsController Instance;

    void Start()
    {
        Instance = this;
    }

    // only calls this function when the button 'Save' is pressed
    public void SaveAllSettings()
    {
        app.model.SetMusicPlayerPrefs(app.view.GetMusicValue());
        app.model.SetSoundEffectsPlayerPrefs(app.view.GetSoundEffectsValue());
        app.model.SetAlgorithmPlayerPrefs(app.view.GetAlgorithmValue());
        app.model.SetDelayPlayerPrefs(app.view.GetDelayValue());
    }

    // a function you call when a setting button is clicked
    public static void InvokeSettings()
    {
        PerspectivePan.SetPanning(); // So that when the user interacts with slider on level Selection page, the panning would be disable
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    // a function you call when a close button is clicked
    // in this case there's two buttons, the close and save button
    public void CloseSettings()
    {
        PerspectivePan.SetPanning();
        GameController.Instance.ResetMixer();
        SceneManager.UnloadSceneAsync("Settings");
    }

    // function to set settings button to any object that has SettingsButton tag
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
    public float GetDecibelLogValue(float value)
    {
        return Mathf.Log10(value) * 20;
    }
}