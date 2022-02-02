using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public static ButtonController Instance;
    void Awake(){
        Instance = this;
    }

    // A class to group all of them ButtonClick actions
    public static void OnButtonClick(GameObject button){
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(button);
    }
}
