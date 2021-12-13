using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject closeButton;
    [SerializeField]
    private Gameobject saveButton;

    public void CloseButton(){
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(closeButton);
    }

    public void SaveButton(){
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(saveButton);
    }
}
