using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Achievement - UI Manager
public class AUIManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsButton = null;
    [SerializeField] private GameObject homeButton = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HomeButton(){
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(homeButton);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.Homepage);
    }

    public void SettingsButton(){
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(settingsButton);
    }

}
