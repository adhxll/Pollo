using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{

    public PitchDetector detectedPitch;
    public static SongManager Instance;
    
    public Lane lanes;
    public float songDelayInSeconds;
    public double marginOfError;    // In seconds
    public int inputDelayInMilliseconds;

    [SerializeField]
    TextAsset midiJSON = null;

    public static MIDI.MidiFile midiFile;

    public float noteTime;  // Time needed for the note spawn location to the tap location
    public float noteSpawnX;    // Note spawn position in world space
    public float noteTapX;  // Note tap position in world space

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

    void GetDataFromMidi()
    {
        var notes = midiFile.tracks[0].notes;
        var array = new MIDI.Notes[notes.Length];
        notes.CopyTo(array, 0);

        lanes.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void StartSong()
    {
        songPlayed = true;
        dspTimeSong = AudioSettings.dspTime;
        switch (SceneStateManager.Instance.GetSceneState())
        {
            case SceneStateManager.SceneState.Instruction:
                detectedPitch.GetComponent<FFTSystem>().StartPlaying(); // Play audio source if the current scene state is 'Instruction'
                break;
            case SceneStateManager.SceneState.Countdown:
                detectedPitch.GetComponent<FFTSystem>().StartRecording(); // Play through microphone if the current scene state is 'Countdown/Gameplay'
                break;
        }
        PolloController.Instance.SetActive(true);
        audioSource.PlayScheduled(0);
    }

    // Get current playback position in metric times
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
        //return AudioSettings.dspTime - Instance.dspTimeSong;
    }

    // Get current beat in float
    public static float GetCurrentBeat()
    {
        return (float)GetAudioSourceTime() / midiBPM;
    }

    void Update()
    {
        // Check if the song has ended
        // If the condition is true, it'll change current scene state 'Instruction' to 'Countdown/Gameplay'
        if (GetAudioSourceTime() == 0
            && songPlayed
            && (AudioSettings.dspTime - dspTimeSong) > songDelayInSeconds
            && SceneStateManager.Instance.GetSceneState() != SceneStateManager.SceneState.Pause)
        {
            songPlayed = false;
            ResetScene();
            SceneStateManager.Instance.ChangeSceneState(SceneStateManager.SceneState.Countdown);
        }
    }

    // Reset all instance to its default state
    public void ResetScene()
    {
        ScoreManager.Instace.Reset();
        Lane.Instance.Reset();
    }
}
