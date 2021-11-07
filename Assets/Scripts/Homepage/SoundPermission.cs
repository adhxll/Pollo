using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPermission : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject permissionController;
    private void Awake()
    {
        AudioSource source = permissionController.GetComponent<AudioSource>();
        source.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
        source.Play(); 

    }



}
