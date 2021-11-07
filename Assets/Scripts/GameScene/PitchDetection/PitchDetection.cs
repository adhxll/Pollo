using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pitch.Algorithm
{
    public static class PitchTracker
    {
        // This variables below are being used only for SRH algorithm
        const int spectrumSize = 1024;
        const int outputResolution = 200;
        static float[] spectrum = new float[spectrumSize];
        static float[] specRaw = new float[spectrumSize];
        static float[] specCum = new float[spectrumSize];
        static float[] specRes = new float[spectrumSize];
        static float[] srh = new float[outputResolution];

        #region frequency-domain methods

        // Taken from NWaves Library - MIT License
        // https://github.com/ar1st0crat/NWaves

        /// <summary>
        /// Estimates pitch from <paramref name="spectrum"/> using Harmonic Sum Spectrum (HSS) method.
        /// </summary>
        /// <param name="spectrum">Spectrum</param>
        /// <param name="samplingRate">Sampling rate</param>
        /// <param name="low">Lower frequency of expected pitch range</param>
        /// <param name="high">Upper frequency of expected pitch range</param>
        public static float FromHss(
            float[] spectrum,
            int samplingRate,
            float low = 80/*Hz*/,
            float high = 400/*Hz*/)
        {
            var sumSpectrum = spectrum.FastCopy();

            var fftSize = (spectrum.Length - 1) * 2;

            var startIdx = (int)(low * fftSize / samplingRate) + 1;
            var endIdx = (int)(high * fftSize / samplingRate) + 1;
            var decimations = Math.Min(spectrum.Length / endIdx, 10);

            var hssIndex = 0;
            var maxHss = 0.0f;

            for (var j = startIdx; j < endIdx; j++)
            {
                sumSpectrum[j] *= 1.5f;         // slightly emphasize 1st component

                for (var k = 2; k < decimations; k++)
                {
                    sumSpectrum[j] += (spectrum[j * k - 1] + spectrum[j * k] + spectrum[j * k + 1]) / 3;
                }

                if (sumSpectrum[j] > maxHss)
                {
                    maxHss = sumSpectrum[j];
                    hssIndex = j;
                }
            }

            return (float)hssIndex * samplingRate / fftSize;
        }

        // Taken from aldonaletto's answer from Unity Forum
        // https://answers.unity.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html

        /// <summary>
        /// <para>Estimates pitch from spectrum generated from FFT</para>
        /// </summary>
        /// <param name="spectrum">Array of samples</param>
        /// <param name="samplingRate">Sampling rate</param>
        /// <param name="threshold">Amplitude threshold to be included in the calculation</param>
        /// <returns></returns>
        public static float FromFFT(
            float[] spectrum,
            int samplingRate,
            float threshold = 0.005f)
        {
            float maxV = 0;
            var maxN = 0;
            var sc = spectrum.Length;
            int i;
            for (i = 0; i < sc; i++)
            {
                if (spectrum[i] > maxV && spectrum[i] > threshold)
                {
                    maxV = spectrum[i];
                    maxN = i;
                }
            }

            float freqN = maxN;
            if (maxN > 0 && maxN < sc - 1)
            {
                var dL = spectrum[maxN - 1] / spectrum[maxN];
                var dR = spectrum[maxN + 1] / spectrum[maxN];
                freqN += 0.5f * (dR * dR - dL * dL);
            }

            return freqN * (samplingRate / 2) / sc;
        }

        // Taken from nakakq's Github - Unlicense License
        // https://github.com/nakakq/AudioPitchEstimatorForUnity/

        /// <summary>
        /// <para> SRH (Summation of Residual Harmonics)</para>
        /// <para> T. Drugman and A. Alwan: "Joint Robust Voicing Detection and Pitch Estimation Based on Residual Harmonics", Interspeech'11, 2011.</para>
        /// </summary>
        /// <param name="spectrum">Array of samples</param>
        /// <param name="samplingRate">Sampling rate</param>
        /// <param name="low">Lower frequency of expected pitch range</param>
        /// <param name="high">Upper frequency of expected pitch range</param>
        /// <param name="harmonicsToUse">Number of overtones to estimate</param>
        /// <param name="smoothingWidth">Spectrum moving average bandwidth. The wider the bandwidth, the smoother it'll be,but it will be less accurate</param>
        /// <param name="thresholdSRH">SRH threshold. The larger the value, the stricter the judgement.</param>
        /// <returns></returns>
        public static float FromSRH(
            float[] spectrum,
            int samplingRate,
            int low = 40/*Hz*/,
            int high = 1200/*Hz*/,
            int harmonicsToUse = 5,
            float smoothingWidth = 500,
            float thresholdSRH = 7)
        {
            var nyquistFreq = samplingRate / 2.0f;

            // Calculate algorithm of audio spectrum
            for (int i = 0; i < spectrumSize; i++)
            {
                // Add a small value to prevent amplitude from becoming zero and resulting in infinity
                specRaw[i] = (float)Math.Log(spectrum[i] + 1e-9f);
            }

            // Audio spectrum cumulative sum
            specCum[0] = 0;
            for (int i = 1; i < spectrumSize; i++)
            {
                specCum[i] = specCum[i - 1] + specRaw[i];
            }

            // Calculate residual spectrum of audio
            var halfRange = Mathf.RoundToInt((smoothingWidth / 2) / nyquistFreq * spectrumSize);
            for (int i = 0; i < spectrumSize; i++)
            {
                // Smooth spectrum (moving average using cumulative sum
                var indexUpper = Math.Min(i + halfRange, spectrumSize - 1);
                var indexLower = Math.Max(i - halfRange + 1, 0);
                var upper = specCum[indexUpper];
                var lower = specCum[indexLower];
                var smoothed = (upper - lower) / (indexUpper - indexLower);

                // Removing smooth components from the previous spectrum
                specRes[i] = specRaw[i] - smoothed;
            }

            // SRH (Summation of Residual Harmonics) Score Calculation
            float bestFreq = 0, bestSRH = 0;
            for (int i = 0; i < outputResolution; i++)
            {
                var currentFreq = (float)i / (outputResolution - 1) * (high - low) + low;

                // Calculate SRH score of current frequency using equation 1 from paper
                var currentSRH = GetSpectrumAmplitude(specRes, currentFreq, nyquistFreq);
                for (int h = 2; h <= harmonicsToUse; h++)
                {
                    // At a frequency of h times, the stronger the signal, the better
                    currentSRH += GetSpectrumAmplitude(specRes, currentFreq * h, nyquistFreq);

                    // At frequency between h-1 times and h times, the stronger the signal, the worse it is
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

        #endregion

        // Get amplitude of spectrumData from frequency
        static float GetSpectrumAmplitude(float[] spec, float frequency, float nyquistFreq)
        {
            var position = frequency / nyquistFreq * spec.Length;
            var firstIndex = (int)position;
            var secondIndex = firstIndex + 1;
            var delta = position - firstIndex;
            return (1 - delta) * spec[firstIndex] + delta * spec[secondIndex];
        }
    }
}