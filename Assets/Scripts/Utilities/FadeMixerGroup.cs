using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class FadeMixerGroup
{
    public static IEnumerator StartFade (AudioMixer audioMixer, string exposedParam, float duration, Fade fade)
    {
        float currentTime = 0;
        float currentVol;
        float targetValue = 0;
        audioMixer.GetFloat(exposedParam, out currentVol);
       
        switch (fade)
        {
            case Fade.In:
                currentVol = -80;
                targetValue = Mathf.Clamp(100, 0.0001f, 1);
                break;
            case Fade.Out:
                currentVol = Mathf.Pow(10, currentVol / 20);
                targetValue = 0;
                break;
        }

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(0, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }

    public enum Fade {
        Out,
        In
    }
}