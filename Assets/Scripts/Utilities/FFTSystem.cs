using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTSystem : MonoBehaviour
{
    PitchDetector pitchDetector;

    private float pitchValue = 0;
    private int qSamples = 2048;    // Array size, it should be a power of 2
    private float threshold = 0.005f;   // Minimum amplitude to extract pitch
    private float[] spectrum;   // Audio spectrum
    private float fSample;

    AudioSource audioSource;
    private string microphone = null;
    private int tempMidi = 0;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        AnalyzeSound();
    }

    void Initialize()
    {
        pitchDetector = GetComponent<PitchDetector>();
        audioSource = pitchDetector.source;
        spectrum = new float[qSamples];
        fSample = AudioSettings.outputSampleRate;
    }

    // In short this function is a black magic.
    void AnalyzeSound()
    {
        // Analyze & storing audio spectrum (a chart showing a handful of frequency list and it's amplitude value) of the given audio source in an array
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Hanning);

        // Finding which frequency range is the most dominant (highest amplitude) based on the analyzed/stored spectrum
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

        // After knowing which frequency range is the most domninant,
        // this function below will determine what is the frequency of that pitch by comparing with its neighbors.

        // In case you're wondering what I mean by frequency range, remember that we had an array with power of 2 length? In this case it's 2048.
        // So, digital audio signal/waves (mp3) usually had their own predefined frequency range, usually it's 44100hz.
        // Yeah, in short, those audio file may consist any frequency between 0 - 44100hz in theory.
        // In this code, I use Unity audio default audio setting (line 34) to read the default frequency.
        // So, each block of those array will consist any 44100/2048 or 21,5hz of frequency range per block.
        // To visualize sample[0] = 0 - 21,5hz, sample[1] = 21,6 - 43hz, etc.
        // Since all we got is frequency range, not the exact frequency, this function below will determine what is the exact frequency by it's neighbors (n+1 & n-1) amplitude value.

        float freqN = maxN;
        if (maxN > 0 && maxN < qSamples - 1)
        {
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }

        var pitch = freqN * (fSample / 2) / qSamples; // Convert index to frequency
        var midiNote = 0;
        var midiCents = 0;

        pitchValue = pitch;
        Pitch.PitchDsp.PitchToMidiNote(pitch, out midiNote, out midiCents);

        pitchDetector.pitch = pitch;
        pitchDetector.midiNote = midiNote;

        // Log pitch detection changes.
        // Pitch detection will be called on each frame updates, so it'll print the result at least 30 times every seconds, whether there's a changes or not in the detected note.
        // To make it easier for us in the debugging process, this function below will only print the result if the detected pitch is different with the previous one.
        if (midiNote != 0 && midiNote != tempMidi)
        {
            tempMidi = midiNote;
            Debug.Log($"FFT Transcribed : {midiNote}, time : {SongManager.GetAudioSourceTime()}");
        }
    }

    public void StartPlaying()
    {
        pitchDetector.source.PlayScheduled(0);
    }

    public void StartRecording()
    {
        // Using default active microphone on the platform/device
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 1, 44100);

        audioSource.loop = true;
        audioSource.mute = false;

        // Check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
        if (Microphone.IsRecording(microphone))
        {
            // Wait until the recording has started. 
            while (!(Microphone.GetPosition(microphone) > 0)) {} 
            audioSource.Play();
        }
    }
}
