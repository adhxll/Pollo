using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instace;
    // Use this for initialization

    private void Awake()
    {
        Instace = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AnimateHit(GameObject obj, float scale)
    {
        obj.transform.DORewind();
        obj.transform.DOPunchScale(new Vector3(scale, scale, scale), 0.25f, 1, 0);
    }

    public void AnimateJump(GameObject obj, float jump, float duration)
    {
        obj.transform.DOPunchPosition(new Vector3(0, jump, 0), duration, 1, 0);
    }

    public void AnimatePos(GameObject obj, float move, float duration)
    {
        obj.transform.DOLocalMoveY(move, duration);
    }
}
