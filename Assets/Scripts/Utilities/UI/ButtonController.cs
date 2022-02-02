using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public static ButtonController Instance;
    void Awake(){
        Instance = this;
    }
    public static void ButtonClick(GameObject button){
        Debug.Log("hi from buttonClick");
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(button);
    }
}
