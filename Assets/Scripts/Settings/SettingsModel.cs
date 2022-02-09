using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will directly interact with the SettingsList enum which corresponds to the PlayerPrefs
// Usually a model class looks like a class object with its own attributes/property, but in this case it just access it directly from playerprefs 
// You can say that the attributes of this class is the SettingsList Enum
// In the future, I hope everyone will use this class to access and manipulate settings
public class SettingsModel : SettingsElement 
{
    #region Getter
    public float GetMusicPlayerPrefs(){
        return PlayerPrefs.GetFloat(SettingsList.Music.ToString(), 1);
    }

    public float GetSoundEffectsPlayerPrefs(){
        return PlayerPrefs.GetFloat(SettingsList.SoundEffects.ToString(), 1);
    }
    
    public int GetAlgorithmPlayerPrefs(){
        return PlayerPrefs.GetInt(SettingsList.Algorithm.ToString(), 0);
    }

    public float GetDelayPlayerPrefs(){
        return PlayerPrefs.GetFloat(SettingsList.Delay.ToString(), 0);
    }

    public float GetForcePitchPlayerPrefs(){ // kok ForcePitch float ya?
        return PlayerPrefs.GetFloat(SettingsList.ForcePitch.ToString(), 0);
    }
    public int GetRepeatSectionPlayerPrefs(){
        return PlayerPrefs.GetInt(SettingsList.RepeatSection.ToString(), 0);
    }
    public float GetDisableAnalyticsPlayerPrefs(){
        return PlayerPrefs.GetInt(DeveloperMode.DisableAnalytics.ToString(), 0);
    }
    #endregion Getter

    #region Setter
    public void SetMusicPlayerPrefs(float value){
        PlayerPrefs.SetFloat(SettingsList.Music.ToString(),value);
    }

    public void SetSoundEffectsPlayerPrefs(float value){
        PlayerPrefs.SetFloat(SettingsList.SoundEffects.ToString(), value);
        
    }
    public void SetAlgorithmPlayerPrefs(int value){
        PlayerPrefs.SetInt(SettingsList.Algorithm.ToString(), value);
    }
    public void SetDelayPlayerPrefs(float value){
        PlayerPrefs.SetFloat(SettingsList.Delay.ToString(), value);
    }
    public void SetForcePitchPlayerPrefs(float value){ // kok ForcePitch float ya?
        PlayerPrefs.SetFloat(SettingsList.ForcePitch.ToString(), value);
    }
    public void SetRepeatSectionPlayerPrefs(int value){
        PlayerPrefs.SetInt(SettingsList.RepeatSection.ToString(), value);
    }
    public void SetDisableAnalyticsPlayerPrefs(int value){
        PlayerPrefs.SetInt(DeveloperMode.DisableAnalytics.ToString(), value);
    }
    #endregion Getter
}




