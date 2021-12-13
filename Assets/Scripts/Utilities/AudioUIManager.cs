using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUIManager : MonoBehaviour
{
    public void referButtonSound(){
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().PlayButtonSound();
    }

    public void referCountdownSound(){
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().PlayCountDown();
    }

    public void referCoinAddSound(){
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().PlayCoinAddSound();
    }
    public void referStarSound(int index){
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().PlayStarSound(index);
    }
}
