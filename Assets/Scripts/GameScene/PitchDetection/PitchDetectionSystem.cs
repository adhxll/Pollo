using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pitch.Algorithm;

public class PitchDetectionSystem : MonoBehaviour
{
    private PitchDetector pitchDetector;
    private AudioSource audioSource;

    [SerializeField]
    private Dropdown dropdown = null;

    private int sampleCount = 2048;
    private int audioSamplerate;
    private float[] spectrum;
    private float[] buffer;

    private string microphone = null;
    private float pitchValue = 0;
    private int tempMidi = 0;

    private PitchAlgo algo
    {
        get
        {
            var player = PlayerPrefs.GetInt("pitchAlgo");
            return (PitchAlgo)player;
        }
    }

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
        PopulateList();

        pitchDetector = GetComponent<PitchDetector>();
        audioSamplerate = AudioSettings.outputSampleRate;

        audioSource = pitchDetector.GetSource();
        spectrum = new float[sampleCount];
        buffer = new float[sampleCount];
    }

    void AnalyzeSound()
    {
        audioSource.GetOutputData(buffer, 0);
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Hanning);

        switch (algo)
        {
            case PitchAlgo.SRH:
                pitchValue = PitchTracker.FromSRH(spectrum, audioSamplerate);
                break;

            case PitchAlgo.HSS:
                pitchValue = PitchTracker.FromHSS(spectrum, audioSamplerate, 80, 1600);
                break;

            case PitchAlgo.HPS:
                pitchValue = PitchTracker.FromHPS(spectrum, audioSamplerate, 80, 1600);
                break;

            case PitchAlgo.FFT:
                pitchValue = PitchTracker.FromFFT(spectrum, audioSamplerate);
                break;

        }

        PitchUtilities.PitchToMidiNote(pitchValue, out int midiNote, out int midiCents);
        pitchDetector.SetPitch(pitchValue);
        pitchDetector.SetMidiNote(midiNote);

        // Log pitch detection changes.
        // Pitch detection will be called on each frame updates, so it'll print the result at least 30 times every seconds, whether there's a changes or not in the detected note.
        // To make it easier for us in the debugging process, this function below will only print the result if the detected pitch is different with the previous one.
        if (midiNote != 0 && midiNote != tempMidi)
        {
            tempMidi = midiNote;
            //Debug.Log($"Transcribed : {midiNote}, time : {SongManager.GetAudioSourceTime()}");
        }
    }

    public void StartPlaying()
    {
        pitchDetector.GetSource().PlayScheduled(0);
    }

    public void StartRecording()
    {
        // Using default active microphone on the platform/device
        microphone = Microphone.devices[0];
        audioSource.clip = Microphone.Start(microphone, true, 1, audioSamplerate);

        audioSource.loop = true;
        audioSource.mute = false;

        // Check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
        if (Microphone.IsRecording(microphone))
        {
            // Wait until the recording has started. 
            while (!(Microphone.GetPosition(microphone) > 0)) { }
            audioSource.Play();
        }
    }

    void PopulateList()
    {
        string[] enumNames = Enum.GetNames(typeof(PitchAlgo));
        List<string> names = new List<string>(enumNames);
        dropdown.AddOptions(names);
        dropdown.value = (int)algo;
    }

    public void DropdownValueChanged(int index)
    {
        dropdown.value = index;
        PlayerPrefs.SetInt("pitchAlgo", index);
    }
}
