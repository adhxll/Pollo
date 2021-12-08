using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WavesAnimation : MonoBehaviour
{

    public enum WavesType
    {
        Corner,
        Linear
    }

    [SerializeField] private WavesType anim = WavesType.Linear;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("AnimateWaves", 1f);
    }

    void AnimateWaves()
    {
        foreach (Transform obj in gameObject.transform)
        {
            switch (anim)
            {
                case WavesType.Corner:
                    AnimationUtilities.Instance.WavesCornerAnimation(obj);
                    break;
                case WavesType.Linear:
                    AnimationUtilities.Instance.WavesLinearAnimation(obj);
                    break;
            }
        }
    }
}
