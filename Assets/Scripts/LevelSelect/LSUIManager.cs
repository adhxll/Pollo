using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSUIManager : MonoBehaviour
{
    [SerializeField] private GameObject homeButton = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GoToHome()
    {
        AudioController.Instance.PlayButtonSound();
        AnimationUtilities.Instance.AnimateButtonPush(homeButton, () => SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.Homepage));
    }

    // Additive scene
    public void ShowModal()
    {
        AudioController.Instance.PlayButtonSound();
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.LSModal, true);
    }

}
