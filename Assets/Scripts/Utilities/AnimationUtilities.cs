using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class AnimationUtilities : MonoBehaviour
{
    public enum AnimationType
    {
        PunchScale,
        MoveY
    }

    public static AnimationUtilities Instance;

    private void Awake()
    {
        Instance = this;
    }

    //Taro gameobject yang mau dianimate sebagai parameter, trus jalanin. 
    //Pastiin kalo mau animate terus objectnya dihilangkan / mau pindah scene, jalanin func animate baru 
    //jalanin fungsi pindah/hilang pake invoke
    public static void AnimateButtonPush(GameObject obj) { //animate squish seperti pencet tombol
        obj.transform.DORewind();
          
        float vectorX = obj.transform.localScale.x * -0.25f;
        float vectorY = obj.transform.localScale.y * -0.25f;
        float vectorZ = obj.transform.localScale.z * -0.25f; 

        obj.transform.DOPunchScale(new Vector3(vectorX, vectorY, vectorZ), 0.2f, 1, 1);
    }

    public void AnimateButtonPush(GameObject obj, Action act)
    {
        obj.transform.DORewind();

        float vectorX = obj.transform.localScale.x * -0.1f;
        float vectorY = obj.transform.localScale.y * -0.1f;
        float vectorZ = obj.transform.localScale.z * -0.1f;

        obj.transform.DOPunchScale(new Vector3(vectorX, vectorY, vectorZ), 0.1f, 1, 1)
            .OnComplete(() => act());
    }

    public static void AnimatePopUp(GameObject obj) { //animate expand kebalikan dari pencet tombol
        obj.transform.DORewind();
      
        float vectorX = obj.transform.localScale.x * 0.25f;
        float vectorY = obj.transform.localScale.y * 0.25f;
        float vectorZ = obj.transform.localScale.z * 0.25f; 
       
        obj.transform.DOPunchScale(new Vector3(vectorX, vectorY, vectorZ), 0.3f, 1);
    }
    public static void AnimatePopUpDisappear(GameObject obj) { //animate shrinking menjadi super kecil (belom menghilang)
        obj.transform.DORewind();

        float vectorX = obj.transform.localScale.x * -0.99f;
        float vectorY = obj.transform.localScale.y * -0.99f;
        float vectorZ = obj.transform.localScale.z * -0.99f;

        obj.transform.DOPunchScale(new Vector3(vectorX, vectorY, vectorZ), 0.6f, 1);
    }

    // This function is the one responsible for the moving up and fading money indicator.
    // You can use it for any object that is TMP_Text
    public static void AnimateAddMoney(GameObject indicator)
    {
        // TODO: purchase DOTweenPro so we can animate the value change of a TMP text :D
        Sequence s = DOTween.Sequence();
        indicator.GetComponent<TMP_Text>().alpha = 127;
        indicator.transform.DORewind();   
        float vectorY = indicator.transform.localPosition.y + 50f;
        s.Append(indicator.transform.DOLocalMoveY(vectorY,2.5f));
        s.Insert(0, indicator.GetComponent<TMP_Text>().DOFade(0, 2f));
        s.Play();
    }

    public static void AnimateButtonJump(GameObject obj)
    {
        obj.transform.DOPunchPosition(new Vector3(0, 5f), 1f, 0, 1f).SetLoops(-1);
    }

    // Animate group of objects based on the given parameter (duration & animationType)
    public IEnumerator AnimateObjects(GameObject[] objects, float duration, AnimationType type, float target, float from)
    {
        foreach (var obj in objects)
        {
            // Set parent to active if it's inactive
            if (obj.transform.parent != null && !obj.transform.parent.gameObject.activeSelf)
            {
                var parentObj = obj.transform.parent.gameObject;
                parentObj.SetActive(true);
            }

            obj.SetActive(true);

            switch (type)
            {
                case AnimationType.MoveY:
                    MoveY(obj, target, from);
                    break;
                case AnimationType.PunchScale:
                    PunchScale(obj);
                    break;
            }
            yield return new WaitForSeconds(duration);
        }
    }

    public IEnumerator AnimateObjects(List<GameObject> objects, float duration, AnimationType type, float target, float from)
    {
        foreach (var obj in objects)
        {
            // Set parent to active if it's inactive
            if (obj.transform.parent != null && !obj.transform.parent.gameObject.activeSelf)
            {
                var parentObj = obj.transform.parent.gameObject;
                parentObj.SetActive(true);
            }

            obj.SetActive(true);

            switch (type)
            {
                case AnimationType.MoveY:
                    MoveY(obj, target, from);
                    break;
                case AnimationType.PunchScale:
                    PunchScale(obj);
                    break;
            }
            yield return new WaitForSeconds(duration);

        }
    }

    public void AnimateHit(GameObject obj)
    {
        obj.transform.DORewind();
        obj.transform.DOPunchScale(new Vector3(-0.1f, -0.1f, -0.1f), 0.2f, 1, 1);
    }

    public void PunchScale(GameObject obj)
    {
        obj.transform.DORewind();
        obj.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 0.25f), 0.2f, 1, 1);
    }

    public void LocalMoveY(GameObject obj, float move, float duration)
    {
        obj.transform.DOLocalMoveY(move, duration);
    }

    public void MoveY(GameObject obj, float target, float from, bool loop = false)
    {
        var post = obj.transform.position;
        obj.SetActive(true);
        if (loop)
            obj.transform.DOMoveY(post.y + target, 0.75f).SetEase(Ease.InOutQuad).From(post.y + from).SetLoops(-1, LoopType.Yoyo);

        else
            obj.transform.DOMoveY(post.y + target, 0.75f).SetEase(Ease.InOutQuad).From(post.y + from);
    }

    public void MoveX(GameObject obj, float target, float from, bool loop = false)
    {
        var post = obj.transform.position;
        obj.SetActive(true);
        if (loop)
            obj.transform.DOMoveX(post.x + target, 0.75f).SetEase(Ease.InOutQuad).From(post.x + from).SetLoops(-1, LoopType.Yoyo);

        else
            obj.transform.DOMoveX(post.x + target, 0.75f).SetEase(Ease.InOutQuad).From(post.x + from);
    }

    public void MoveCameraX(Camera obj, float target, float from, bool loop = false)
    {// used to move the camera along the x axis with duration 0.75 seconds
        var post = obj.transform.position;
        //obj.SetActive(true);
        if (loop)
            obj.transform.DOMoveX(target, 0.75f).SetEase(Ease.InOutQuad).From(from).SetLoops(-1, LoopType.Yoyo);

        else
            obj.transform.DOMoveX(target, 0.75f).SetEase(Ease.InOutQuad).From(from);
    }

    // Animate waves sideways (left & right)
    public void WavesLinearAnimation(Transform obj)
    {
        obj.DOPunchScale(new Vector3(0.2f, 0f, 0f), 2f, 1, 1)
            .SetLoops(-1, LoopType.Restart)
            .SetId(1);
        obj.DOLocalMoveX(0.5f, 2f, false)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetRelative()
            .SetId(1);
    }

    // Animates waves diagonally
    public void WavesCornerAnimation(Transform obj)
    {
        var sprite = obj.GetComponent<SpriteRenderer>();
        sprite.DOFade(0.5f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetId(1);
        obj.DOPunchScale(new Vector3(0.2f, 0f, 0f), 2f, 1, 1)
            .SetLoops(-1, LoopType.Restart)
            .SetId(1);
        obj.DOLocalMove(new Vector3(-0.2f, -0.2f, 0), 2f, false)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetRelative()
            .SetId(1);
    }
}