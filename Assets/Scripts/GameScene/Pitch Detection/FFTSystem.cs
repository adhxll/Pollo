using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTSystem : MonoBehaviour
{
    PitchDetector pitchDetector;

    private float pitchValue = 0;
    private int qSamples = 1024;    // Array size, it should be a power of 2
    private float[] spectrum;   // Audio spectrum

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
    }

    void AnalyzeSound()
    {
        var pitch = GetComponent<AudioPitchEstimator>().Estimate(audioSource);
        var midiNote = 0;
        var midiCents = 0;

        pitchValue = pitch;

        // Converting frequency to MIDI notes, this methods is another black magic and I'm not sure how it works.
        // All I know is this one does it job really well.
        PitchAC.PitchDsp.PitchToMidiNote(pitch, out midiNote, out midiCents);
        pitchDetector.pitch = pitch;
        pitchDetector.midiNote = midiNote;

        // Log pitch detection changes.
        // Pitch detection will be called on each frame updates, so it'll print the result at least 30 times every seconds, whether there's a changes or not in the detected note.
        // To make it easier for us in the debugging process, this function below will only print the result if the detected pitch is different with the previous one.
        if (midiNote != 0 && midiNote != tempMidi)
        {
            tempMidi = midiNote;
            //Debug.Log($"FFT Transcribed : {midiNote}, time : {SongManager.GetAudioSourceTime()}");
        }
    }

    public void StartPlaying()
    {
        pitchDetector.source.PlayScheduled(0);
    }

    public void StartRecording()
    {
        // Using default active microphone on the platform/device
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 1, AudioSettings.outputSampleRate);

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
