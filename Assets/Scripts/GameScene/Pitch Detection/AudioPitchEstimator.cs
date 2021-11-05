using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SRH (Summation of Residual Harmonics) による基本周波数推定
// T. Drugman and A. Alwan: "Joint Robust Voicing Detection and Pitch Estimation Based on Residual Harmonics", Interspeech'11, 2011.

public class AudioPitchEstimator : MonoBehaviour
{
    [Tooltip("Lowest freq[Hz]")]
    [Range(40, 150)]
    public int frequencyMin = 40;

    [Tooltip("Highest freq [Hz]")]
    [Range(300, 1200)]
    public int frequencyMax = 600;

    [Tooltip("Number of overtones to estimate")]
    [Range(1, 8)]
    public int harmonicsToUse = 5;

    [Tooltip("Spectrum moving average bandwidth [Hz]\n The wider the bandwidth, the smoother it'll be but it will be less accurate")]
    public float smoothingWidth = 500;

    [Tooltip("SRH threshold\nThe larger the value, the judgement is stricter")]
    public float thresholdSRH = 7;

    const int spectrumSize = 1024;
    const int outputResolution = 200; // Number of elements on the frequency axis??? of SRH. Smaller numbers reduce calculation load.
    float[] spectrum = new float[spectrumSize];
    float[] specRaw = new float[spectrumSize];
    float[] specCum = new float[spectrumSize];
    float[] specRes = new float[spectrumSize];
    float[] srh = new float[outputResolution];

    public List<float> SRH => new List<float>(srh);

    /// <summary>
    /// Estimate the fundamental frequency
    /// </summary>
    /// <param name="audioSource">Input Sound Source</param>
    /// <returns>Fundamental Frequency[Hz] (Return float.NaN if null)</returns>
    public float Estimate(AudioSource audioSource)
    {
        var nyquistFreq = AudioSettings.outputSampleRate / 2.0f;

        // get audio spectrum
        if (!audioSource.isPlaying) return float.NaN;
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Hanning);

        // calculate logarithm?? of audio spectrum
        // all spectrums are calculated using logarithmic aplitude, different from paper
        for (int i = 0; i < spectrumSize; i++)
        {
            //add a small value to prevent amplitude from becoming zero and resulting in -infinity
            specRaw[i] = Mathf.Log(spectrum[i] + 1e-9f);
        }

        // audio spectrum cumulative sum (use the next one)
        specCum[0] = 0;
        for (int i = 1; i < spectrumSize; i++)
        {
            specCum[i] = specCum[i - 1] + specRaw[i];
        }

        // calculate residual spectrum of audio
        var halfRange = Mathf.RoundToInt((smoothingWidth / 2) / nyquistFreq * spectrumSize);
        for (int i = 0; i < spectrumSize; i++)
        {
            // smooth spectum (moving average using cumulative sum)
            var indexUpper = Mathf.Min(i + halfRange, spectrumSize - 1);
            var indexLower = Mathf.Max(i - halfRange + 1, 0);
            var upper = specCum[indexUpper];
            var lower = specCum[indexLower];
            var smoothed = (upper - lower) / (indexUpper - indexLower);

            //removing smooth components from the previous spectrum
            specRes[i] = specRaw[i] - smoothed;
        }

        // SRH (Summation of Residual Harmonics) Score Calculation
        float bestFreq = 0, bestSRH = 0;
        for (int i = 0; i < outputResolution; i++)
        {
            var currentFreq = (float)i / (outputResolution - 1) * (frequencyMax - frequencyMin) + frequencyMin;

            // calculate SRH score of current frequency using equation 1 fro mpaper
            var currentSRH = GetSpectrumAmplitude(specRes, currentFreq, nyquistFreq);
            for (int h = 2; h <= harmonicsToUse; h++)
            {
                // At a frequency of h times, the stronger the signal, the better
                currentSRH += GetSpectrumAmplitude(specRes, currentFreq * h, nyquistFreq);

                // At frequencies between h-1 times and h times, the stronger the signal, the worse it is
                currentSRH -= GetSpectrumAmplitude(specRes, currentFreq * (h - 0.5f), nyquistFreq);
            }
            srh[i] = currentSRH;

            // Record the frequency with the highest score
            if (currentSRH > bestSRH)
            {
                bestFreq = currentFreq;
                bestSRH = currentSRH;
            }
        }

        // SRH score is below the threshold → Consider that there is no clear fundamental frequency
        if (bestSRH < thresholdSRH) return float.NaN;

        return bestFreq;
    }

    // Get amplitude ofSpectrum data from frequency[Hz]
    float GetSpectrumAmplitude(float[] spec, float frequency, float nyquistFreq)
    {
        var position = frequency / nyquistFreq * spec.Length;
        var index0 = (int)position;
        var index1 = index0 + 1;
        var delta = position - index0;
        return (1 - delta) * spec[index0] + delta * spec[index1];
    }

}