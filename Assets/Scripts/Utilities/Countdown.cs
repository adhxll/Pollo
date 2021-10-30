using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Countdown : MonoBehaviour
{
    public GameObject[] countObject;
    public GameObject[] shownObject;
    public GameObject barMeasure;

    private GameObject titleButton;
    private GameObject countButton;
    private TMPro.TextMeshProUGUI title;
    private TMPro.TextMeshProUGUI countdown;

    int count = 3;
    float delay = 1;

    private void Awake()
    {
        titleButton = countObject[0];
        countButton = countObject[1];
        title = countObject[0].GetComponentInChildren<TMPro.TextMeshProUGUI>();
        countdown = countObject[1].GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    void Start()
    {   
        StartCoroutine(CountdownStart());
    }

    void Update()
    {
    }

    IEnumerator CountdownStart()
    {
        while (count > 0)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(AnimateObjects(countObject));
            countdown.SetText(count.ToString());
            count --;
        }

        yield return new WaitForSeconds(delay);

        titleButton.SetActive(false);
        AnimateButton(countButton);
        countdown.SetText("Go!");

        yield return new WaitForSeconds(delay);

        StartCoroutine(AnimateObjects(shownObject));
        this.gameObject.SetActive(false);

        yield return new WaitForSeconds(delay);

    }

    IEnumerator AnimateObjects(GameObject[] objects)
    {
        foreach(var obj in objects)
        {
            if (!obj.activeSelf)
                obj.SetActive(true);

            AnimateButton(obj);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void AnimateButton(GameObject obj)
    {
        obj.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 0.25f), 0.2f, 1, 1);
    }
}
