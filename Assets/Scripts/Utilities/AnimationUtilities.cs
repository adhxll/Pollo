using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

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

    // This function is the one responsible for the moving up and fading money indicator.
    // You can use it for any object that is TMP_Text
    public static void AnimateAddMoney(GameObject indicator)
    {
        Sequence s = DOTween.Sequence();
        indicator.GetComponent<TMP_Text>().alpha = 127;
        indicator.transform.DORewind();   
        float vectorY = indicator.transform.localPosition.y + 50f;
        s.Append(indicator.transform.DOLocalMoveY(vectorY,2.5f));
        s.Insert(0, indicator.GetComponent<TMP_Text>().DOFade(0, 2f));
        s.Play();
    }
}
