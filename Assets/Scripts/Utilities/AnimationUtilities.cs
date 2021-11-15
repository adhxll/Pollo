using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class AnimationUtilities : MonoBehaviour
{
    public static AnimationUtilities Instance;

    private void Start()
    {
        Instance = this;
    }

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

    public void AnimateHit(GameObject obj)
    {
        obj.transform.DOPunchScale(new Vector3(-0.1f, -0.1f, -0.1f), 0.2f, 1, 1);
    }

    public void PunchScale(GameObject obj)
    {
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

    // Enumeration
    public enum AnimationType
    {
        PunchScale,
        MoveY
    }
}
