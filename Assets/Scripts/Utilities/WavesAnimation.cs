using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WavesAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform obj in gameObject.transform)
        {
            var init = obj.position.x;
            obj.DOPunchScale(new Vector3(0.2f, 0f, 0f), 2f, 1, 1).SetLoops(-1, LoopType.Restart);
            obj.DOMoveX(init - 1f, 2f, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }
}
