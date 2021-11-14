using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class AnimationUtilities : MonoBehaviour
{
    public static void AnimateButtonPush(GameObject obj) {
        obj.transform.DORewind();
        //squish berdasarkan scale object, bukan fixed value    
        float vectorX = obj.transform.localScale.x * -0.25f;
        float vectorY = obj.transform.localScale.y * -0.25f;
        float vectorZ = obj.transform.localScale.z * -0.25f; 
        obj.transform.DOPunchScale(new Vector3(vectorX, vectorY, vectorZ), 0.2f, 1, 1);

    }

    public static void AnimateButtonJump(GameObject obj)
    {
        obj.transform.DOPunchPosition(new Vector3(0, 5f), 1f, 0, 1f).SetLoops(-1);
    }
}
