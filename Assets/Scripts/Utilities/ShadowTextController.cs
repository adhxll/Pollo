using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class ShadowTextController : MonoBehaviour
{
    //script ini untuk ngubah text yang punya shadow. Set text foreground dan shadownya dulu di inspector oke?
    public TextMeshPro textForeground;
    public TextMeshPro textShadow;
    public string text; 
    
    void Start()
    {
        textForeground.text = text;
        textShadow.text = text; 

    }

}
