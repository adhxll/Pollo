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

    public void AnimateHit(GameObject score, float scale)
    {
        score.transform.DORewind();
        score.transform.DOPunchScale(new Vector3(scale, scale, scale), 0.25f, 1, 0);
    }
}
