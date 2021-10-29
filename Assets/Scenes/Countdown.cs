using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public GameObject titlePlaceholder;
    public TMPro.TextMeshProUGUI title;
    public TMPro.TextMeshProUGUI countdown;

    private GameObject self;

    int count = 3;
    float delay = 1;
    void Start()
    {   
        self = GetComponent<GameObject>();
        StartCoroutine(CountdownStart());
    }

    void Update()
    {
    }

    IEnumerator CountdownStart()
    {
        while (count > 0)
        {
            countdown.SetText(count.ToString());
            count --;
            yield return new WaitForSeconds(delay);
        }

        countdown.SetText("Go!");
        titlePlaceholder.gameObject.SetActive(false);

        yield return new WaitForSeconds(delay);
        self.SetActive(false);
    }
}
