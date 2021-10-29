using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Melanchall.DryWetMidi.Core;
//using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{

    public PitchDetector detectedPitch;
    public static SongManager Instance;
    
    public Lane lanes;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds
    public int inputDelayInMilliseconds;

    public TextAsset midiJSON;
    public static MIDI.MidiFile midiFile;

    public float noteTime; //Time needed for the note spawn location to the tap location
    public float noteSpawnX;
    public float noteTapX;

    public float noteDespawnX
    {
        get
        {
            return noteTapX - (noteSpawnX - noteTapX);
        }
    }

    AudioSource audioSource;

    // Position Tracking
    double dspTimeSong;
    public bool songPlayed = false;

    static float midiBPM = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        midiFile = MIDI.CreateFromJSON(midiJSON.text);
        GetDataFromMidi();
    }

    private void GetDataFromMidi()
    {
        var notes = midiFile.tracks[0].notes;
        var array = new MIDI.Notes[notes.Length];
        notes.CopyTo(array, 0);

        lanes.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    private void StartSong()
    {
        songPlayed = true;
        dspTimeSong = AudioSettings.dspTime;
        //detectedPitch.source.PlayScheduled(0);
        detectedPitch.GetComponent<FFTSystem>().StartRecording();
        audioSource.PlayScheduled(0);
    }

    // Get current playback position in metric times
    public static double GetAudioSourceTime()
    {
        //return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
        return AudioSettings.dspTime - Instance.dspTimeSong;
    }

    // Get current beat in float
    public static float GetCurrentBeat()
    {
        return (float)GetAudioSourceTime() / midiBPM;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
