using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class AnimationUtilities : MonoBehaviour
{
    public static AnimationUtilities Instance; 
    public void animateButtonPush(GameObject obj) {
        obj.transform.DORewind();
        //squish berdasarkan scale object, bukan fixed value    
        float vectorX = obj.transform.localScale.x * -0.25f;
        float vectorY = obj.transform.localScale.y * -0.25f;
        float vectorZ = obj.transform.localScale.z * -0.25f; 
        obj.transform.DOPunchScale(new Vector3(vectorX, vectorY, vectorZ), 0.2f, 1, 1);

    }
    private void Awake()
    {
        StartSingleton(); 
    }
    private void StartSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
