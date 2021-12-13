using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject closeButton;
    [SerializeField]
    private GameObject saveButton;
    [SerializeField]
    private SettingsController settingsController;

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
