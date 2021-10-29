using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 
public class ButtonScript : MonoBehaviour
{
    public GameObject button;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        
       
    }
    private void OnMouseDown()
    {
        AnimationUtilities.Instance.animateButtonPush(button);
        Invoke(nameof(MoveScene), 0.2f); 
                
              
        Debug.Log($"Pressed button {button.tag}"); 
    }
    // Update is called once per frame
    private void MoveScene() {
        switch (button.tag) {
            case "PlayButton":
                //TODO: Transition animation
                SceneManagerScript.GoToGameScene();
                break;
            default:
                Debug.Log($"Pressed button {button.tag} but it has no function yet.");
                break; 
        }
    
    
    }
}
