using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundPermission : MonoBehaviour
{
    // Start is called before the first frame update
    
    private void Start()
    {
  
        if (PlayerPrefs.GetInt("HasPermission", 0) == 0){
        AudioSource source = this.GetComponent<AudioSource>();
        source.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
        source.Play(); 
        Debug.Log(Microphone.IsRecording(Microphone.devices[0])); 
        PlayerPrefs.SetInt("HasPermission", 1); 
        }
    }



}
