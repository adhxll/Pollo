using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTSystem : MonoBehaviour
{
    PitchDetector pitchDetector;

    public float pitchValue = 0;
    int qSamples = 1024;  // array size
    float threshold = 0.005f;      // minimum amplitude to extract pitch
    float rmsValue;   // sound level - RMS
    private float[] samples; // audio samples
    private float[] spectrum; // audio spectrum
    private float fSample;

    AudioSource audioSource;
    string microphone = null;
    int tempMidi = 0;
    double dspTime;

    private void Awake()
    {
        pitchDetector = GetComponent<PitchDetector>();
        audioSource = pitchDetector.source;
    }

    void Start()
    {
        samples = new float[qSamples];
        spectrum = new float[qSamples];
        fSample = AudioSettings.outputSampleRate;
    }

    void Update()
    {
        AnalyzeSound();
    }

    //void StartPlaying()
    //{
    //    dspTime = AudioSettings.dspTime;
    //    pitchDetector.source.PlayScheduled(0);
    //}

    void AnalyzeSound()
    {
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        float maxV = 0;
        var maxN = 0;
        int i;
        for (i = 0; i < qSamples; i++)
        {
            if (spectrum[i] > maxV && spectrum[i] > threshold)
            {
                maxV = spectrum[i];
                maxN = i;
            }
        }

        float freqN = maxN;
        if (maxN > 0 && maxN < qSamples - 1)
        {
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }

        var pitch = freqN * (fSample / 2) / qSamples; // convert index to frequency
        var midiNote = 0;
        var midiCents = 0;

        pitchValue = pitch;
        Pitch.PitchDsp.PitchToMidiNote(pitch, out midiNote, out midiCents);

        pitchDetector.pitch = pitch;
        pitchDetector.midiNote = midiNote;

        if (midiNote != 0 && midiNote != tempMidi)
        {
            tempMidi = midiNote;
            Debug.Log($"FFT Transcribed : {midiNote}, time : {SongManager.GetAudioSourceTime()}");
        }
    }

    public void StartRecording()
    {
        audioSource.clip = Microphone.Start(null, true, 60, 44100);

        audioSource.loop = true;
        audioSource.mute = false;

        //check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
        if (Microphone.IsRecording(microphone))
        {
            // Wait until the recording has started. 
            while (!(Microphone.GetPosition(microphone) > 0)) {} 
            audioSource.Play();
        }
    }
}
