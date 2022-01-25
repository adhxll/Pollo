using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] private Image handleImage = null;
    [Space]
    [SerializeField] private Sprite active = null;
    [SerializeField] private Sprite inactive = null;

    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value > 0)
            handleImage.sprite = active;
        else
            handleImage.sprite = inactive;
    }
}
