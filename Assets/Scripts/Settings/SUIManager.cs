using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Settings - UI Manager
public class SUIManager : MonoBehaviour
{
    [SerializeField] private GameObject closeButton = null;
    [SerializeField] private GameObject saveButton = null;
    [SerializeField] private SettingsController settingsController = null;

    public void CloseButton(){
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(closeButton);
        settingsController.CloseSettings();
    }

    public void SaveButton(){
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(saveButton);
        SettingsController.Instance.SaveAllSettings();
        settingsController.CloseSettings();
    }
}
