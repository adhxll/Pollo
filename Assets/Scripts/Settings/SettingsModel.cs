using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will directly interact with the SettingsList enum which corresponds to the PlayerPrefs
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
    #endregion Getter
}
