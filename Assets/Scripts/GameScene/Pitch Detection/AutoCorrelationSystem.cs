using Pitch;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoCorrelationSystem : MonoBehaviour
{
    private PitchDetector pitchDetectors;

    PitchTracker pitchTracker;
    private float[] buffer = new float[1024];
    //private int midiCents = 0;
    private int midiNote = 0;
    private PitchDetectionResult result;
    double dspTime;
    int tempMidi = 0;

    private void Awake()
    {
        pitchTracker = new PitchTracker();
        pitchDetectors = GetComponent<PitchDetector>();
        pitchTracker.SampleRate = 44100;
    }

    private void Update()
    {
        pitchDetectors.source.GetOutputData(buffer, 0);
        pitchTracker.ProcessBuffer(buffer, 0);
        pitchDetectors.pitch = pitchTracker.CurrentPitchRecord.Pitch;
        pitchDetectors.midiNote = pitchTracker.CurrentPitchRecord.MidiNote;
        midiNote = pitchDetectors.midiNote;

        if (midiNote != 0 && midiNote != tempMidi)
        {
            tempMidi = midiNote;
            Debug.Log($"AUTOCORR Transcribed : {midiNote}, time : {SongManager.GetAudioSourceTime()}");
            //Debug.Log($"AUTOCORR Transcribed : {midiNote}, time : {AudioSettings.dspTime - dspTime}");
        }
        // pitchDetectors.midiCents = pitchTracker.CurrentPitchRecord.MidiCents;
    }

    void StartPlay()
    {
        dspTime = AudioSettings.dspTime;
        pitchDetectors.source.Play();
    }
}