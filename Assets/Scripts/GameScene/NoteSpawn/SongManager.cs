using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;

    [SerializeField] private int levelID = 0; // Selected Level ID, see level items & database for reference
    [SerializeField] private int stageID = 0; 
    private Level selectedLevel;                // Level object, to load level item based on level ID

    private AudioSource leadTrack;
    private AudioSource backingTrack;

    [Space]
    [SerializeField] private PitchDetector detectedPitch = null;
    [SerializeField] private Lane lanes = null;
    [SerializeField] private BarTimeline barTimeline = null;
    [SerializeField] private AudioMixer audioMixer = null;

    private static MIDI.MidiFile midiFile;

    [Space]
    [SerializeField] private float songDelayInSeconds = 3;
    [SerializeField] private double marginOfError = 0.3f;           // In seconds
    [SerializeField] private int inputDelayInMilliseconds = 250;
    [SerializeField] private float noteTime = 3;                    // Time needed for the note spawn location to the tap location
    [SerializeField] private float noteSpawnX = 16;                 // Note spawn position in world space
    [SerializeField] private float noteTapX = 0;                    // Note tap position in world space

    private float noteDelay { get { return PlayerPrefs.GetFloat("NoteDelay"); } }
    private float noteDespawnX { get { return noteTapX - (noteSpawnX - noteTapX); } }
    private TextAsset midiJSON { get { return selectedLevel.GetMidiJson(); } }
    private static float midiBPM { get { return (float)60 / (float)midiFile.header.tempos[0].bpm; } }

    // Position Tracking
    private double dspTimeSong;
    private bool songPlayed = false;

    //Getter Setter
    public int GetLevelID() { return Instance.levelID; }
    public Level GetSelectedLevel() { return Instance.selectedLevel; }
    public AudioSource GetLeadTrack() { return Instance.leadTrack; }
    public AudioSource GetBackingTrack() { return Instance.backingTrack; }
    public PitchDetector GetDetectedPitch() { return Instance.detectedPitch; }
    public Lane GetLanes() { return Instance.lanes; }
    public BarTimeline GetBarTimeline() { return Instance.barTimeline; }
    public MIDI.MidiFile GetMidiFile() { return midiFile; }

    public float GetSongDelayInSeconds() { return Instance.songDelayInSeconds; }
    public double GetMarginOfError() { return Instance.marginOfError; } 
    public int GetInputDelayInMilliseconds() { return Instance.inputDelayInMilliseconds; }

    public float GetNoteTime() { return Instance.noteTime; }
    public float GetNoteSpawnX() { return Instance.noteSpawnX; }
    public float GetNoteTapX() { return Instance.noteTapX; }
    public float GetNoteDelay() { return Instance.noteDelay;  }

    public float GetNoteDespawnX() { return Instance.noteDespawnX; }
    public TextAsset GetMidiJSON() { return Instance.midiJSON; }
    public float GetMidiBPM() { return midiBPM; }
    public bool GetSongPlayed() { return Instance.songPlayed; }

    // Position Tracking
    public double GetDspTimeSong() { return Instance.dspTimeSong; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        if (PlayerPrefs.GetInt("IsFirstTime") == 1 && GameController.Instance != null) {
            levelID = GameController.Instance.selectedLevel;
            stageID = GameController.Instance.currentStage; 
           }

        selectedLevel = Database.GetLevelAndStage(stageID, levelID);
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

        detectedPitch.GetComponent<PitchDetectionSystem>().StartRecording();

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
            && SceneStateManager.Instance.GetSceneState() != SceneStateManager.SceneState.Pause
            && SceneStateManager.Instance.GetSceneState() != SceneStateManager.SceneState.Practice)
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

        if (selectedLevel.GetLeadTrack() != null)
            leadTrack.clip = selectedLevel.GetLeadTrack();

        if (selectedLevel.GetBackingTrack() != null)
            backingTrack.clip = selectedLevel.GetBackingTrack();
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

    public void ResumeSong(float delay = 0)
    {
        leadTrack.time -= delay;
        backingTrack.time -= delay;

        leadTrack.UnPause();
        backingTrack.UnPause();

        // Audio fade in, if delay > 0
        if (delay > 0)
            StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "backsoundVolume", delay, FadeMixerGroup.Fade.In));
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

    // Reset all Instance to its default state
    public void ResetScene()
    {
        ScoreManager.Instace.Reset();
        Lane.Instance.Reset();
    }
}
