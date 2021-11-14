using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public PitchDetector detectedPitch;

    // Selected Level ID, see level items & database for reference
    private int levelID = 0;
    // Level object, to load level item based on level ID
    private Level selectedLevel;

    [SerializeField]
    private Lane lanes = null;

    [SerializeField]
    private BarTimeline barTimeline = null;

    private AudioSource leadTrack;
    private AudioSource backingTrack;
    public static MIDI.MidiFile midiFile;

    [Space]
    public float songDelayInSeconds;
    public double marginOfError;    // In seconds
    public int inputDelayInMilliseconds;

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

    TextAsset midiJSON
    {
        get
        {
            return selectedLevel.midiJson;
        }
    }

    static float midiBPM
    {
        get
        {
            return (float)60 / (float)midiFile.header.tempos[0].bpm;
        }
    }

    [HideInInspector]
    public bool songPlayed = false;

    // Position Tracking
    double dspTimeSong;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        Instance = this;
        selectedLevel = Database.GetLevelByID(levelID);
        midiFile = MIDI.CreateFromJSON(midiJSON.text);

        InitializeTrack();
        GetDataFromMidi();
    }

    void GetDataFromMidi()
    {
        var tracks = midiFile.tracks;
        int i = 0;
        foreach(var track in tracks)
        {
            var notes = track.notes;

            if (i == 0)
                lanes.SetTimeStamps(notes);

            if (i == 1)
                barTimeline.SetTimeStamps(notes);

            i++;
        }

        barTimeline.PlaceTimestamp();

    }

    public void StartSong()
    {
        songPlayed = true;
        dspTimeSong = AudioSettings.dspTime;

        switch (SceneStateManager.Instance.GetSceneState())
        {
            case SceneStateManager.SceneState.Instruction:
                detectedPitch.GetComponent<PitchDetectionSystem>().StartPlaying(); // Play audio source if the current scene state is 'Instruction'
                break;
            case SceneStateManager.SceneState.Countdown:
                detectedPitch.GetComponent<PitchDetectionSystem>().StartRecording(); // Play through microphone if the current scene state is 'Countdown/Gameplay'
                break;
            case SceneStateManager.SceneState.Onboarding:
                detectedPitch.GetComponent<PitchDetectionSystem>().StartRecording(); // Play through microphone if the current scene state is 'Countdown/Gameplay'
                break;
        }

        PolloController.Instance.SetActive(true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        barTimeline.ConfigureSection();
        PlaySong();

    }
    
    void Update()
    {
        // Check if the song has ended
        // If the condition is true, it'll change current scene state 'Instruction' to 'Countdown/Gameplay'
        if (songPlayed
            && (AudioSettings.dspTime - dspTimeSong) > songDelayInSeconds
            && SceneStateManager.Instance.GetSceneState() != SceneStateManager.SceneState.Pause)
        {
            switch (SceneStateManager.Instance.GetSceneState())
            {
                case SceneStateManager.SceneState.Instruction:
                    songPlayed = false;
                    ResetScene();
                    SceneStateManager.Instance.ChangeSceneState(SceneStateManager.SceneState.Countdown);
                    break;
                case SceneStateManager.SceneState.Countdown:
                    CheckEndOfSong();
                    break;
            }
        }
    }

    void InitializeTrack()
    {
        leadTrack = GetComponents<AudioSource>()[0];
        backingTrack = GetComponents<AudioSource>()[1];

        if (selectedLevel.leadTrack != null)
            leadTrack.clip = selectedLevel.leadTrack;

        if (selectedLevel.backingTrack != null)
            backingTrack.clip = selectedLevel.backingTrack;
    }

    void CheckEndOfSong()
    {
        if (IsAudioFinished())
            SceneStateManager.Instance.ChangeSceneState(SceneStateManager.SceneState.EndOfSong);
    }

    public void PlaySong()
    {
        leadTrack.Play();
        backingTrack.Play();
    }

    public void PauseSong()
    {
        leadTrack.Pause();
        backingTrack.Pause();
    }

    public void ResumeSong()
    {
        leadTrack.UnPause();
        backingTrack.UnPause();
    }

    public void SetAudioPosition(float time)
    {
        leadTrack.time = time;
        backingTrack.time = time;
    }

    // Get current playback position in metric times
    public static double GetAudioSourceTime()
    {
        return (double)Instance.leadTrack.timeSamples / Instance.leadTrack.clip.frequency;
        //return AudioSettings.dspTime - Instance.dspTimeSong;
    }

    // Get current beat in float
    public static float GetCurrentBeat()
    {
        return (float)GetAudioSourceTime() / midiBPM;
    }

    // Get current audio progress in percentage, current position relatives to the clip length
    public float GetCurrentAudioProgress()
    {
        return (float)(GetAudioSourceTime() / leadTrack.clip.length);
    }

    public float GetAudioSourceLength()
    {
        return leadTrack.clip.length;
    }

    public bool IsAudioFinished()
    {
        if (GetCurrentAudioProgress() > 0.99)
            return true;

        return false;
    }

    // Reset all instance to its default state
    public void ResetScene()
    {
        ScoreManager.Instace.Reset();
        Lane.Instance.Reset();
    }
}
