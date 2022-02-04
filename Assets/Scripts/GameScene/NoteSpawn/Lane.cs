using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using TMPro;
using UnityEngine.Analytics;

public class Lane : MonoBehaviour
{
    public static Lane Instance;

    [SerializeField] private GameObject notePrefab = null;
    [SerializeField] private GameObject barPrefab = null;
    [SerializeField] private List<Note> notes = new List<Note>();

    private List<double> timeStamps = new List<double>();   // A list that store each notes timestamp (telling the exact time when its need to be spawned)
    private List<float> noteDurations = new List<float>();  // A list that store each notes ticks duration
    private List<int> midiNotes = new List<int>();          // A list that store what MIDI notes need to be played at the given note

    private int spawnIndex = 0;
    private int inputIndex = 0;
    private int barIndex = 0;
    private int averageCount = 0;                           // Variable to count in exact midi note to prevent Hit() function called accidentally
    private int isForcedPitch { get { return PlayerPrefs.GetInt(SettingsList.ForcePitch.ToString()); } }

    public GameObject GetNotePrefab() { return Instance.notePrefab; }
    public GameObject GetBarPrefab() { return Instance.barPrefab; }
    public List<Note> GetNotes() { return Instance.notes; }
    public List<double> GetTimeStamps() { return Instance.timeStamps; }
    public List<float> GetNoteDurations() { return Instance.noteDurations; }
    public List<int> GetMidiNotes() { return Instance.midiNotes; }
    public int GetSpawnIndex() { return Instance.spawnIndex; }
    public int GetInputIndex() { return Instance.inputIndex; }
    public int GetBarIndex() { return Instance.barIndex; }
    public int GetAverageCount() { return Instance.averageCount; }

    private void Awake()
    {
        Instance = this;
    }

    // Iterate through the given MIDI (or JSON in this case)
    // and set timeStamps, noteDurations, and midiNotes to the list.
    public void SetTimeStamps(MIDI.Notes[] array)
    {
        foreach (var note in array)
        {
            timeStamps.Add(note.time + SongManager.Instance.GetNoteDelay());
            noteDurations.Add((float)note.durationTicks / SongManager.Instance.GetMidiFile().header.ppq);
            midiNotes.Add(note.midi);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SongManager.Instance.GetSongPlayed())
        {
            if (SongManager.GetCurrentBeat() > barIndex)
            {
                // Handle fast forward, if the clip is being fast forwarded, then barIndex will be equal to the integer value of current beat
                // So it'll not be spawned multiple time (since the default increment is 1)
                barIndex = (int)Math.Round(SongManager.GetCurrentBeat());
                SpawnMusicBar();
            }

            // If current clip time is equal or larger than the current noteTimestamp - noteTime (note travel time)
            // Then the note will be spawned
            if (spawnIndex < timeStamps.Count &&
                SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.GetNoteTime())
                SpawnMusicNote();

            // Configure what note that need to be hit at the given time, based on the spawn note
            if (inputIndex < timeStamps.Count)
            {
                double timeStamp = timeStamps[inputIndex];
                double marginOfError = SongManager.Instance.GetMarginOfError();
                double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.GetInputDelayInMilliseconds() / 1000.0);

                if (SceneStateManager.Instance.GetSceneState() == SceneStateManager.SceneState.Onboarding ||
                    SceneStateManager.Instance.GetSceneState() == SceneStateManager.SceneState.Practice && isForcedPitch == 1)
                {
                    if (inputIndex < notes.Count && notes[inputIndex].transform.localPosition.x < -0.25f)
                    {
                        SongManager.Instance.PauseSong();

                        if (CheckPitch())
                        {
                            SongManager.Instance.ResumeSong();
                        }
                    }
                }
                // if you want to use the function in real time, use this
                ComparePitch();
                if (CheckPitch())
                {
                    if (Math.Abs(audioTime - timeStamp) < marginOfError)
                    {
                        averageCount++;
                        if (averageCount >= 3)
                        {
                            Hit();
                            averageCount = 0;
                        }
                    }
                }
                else
                {
                    
                }
                // If the note that should be played [inputIndex] is exceeding the timeStamp + marginOfError value
                // Then the note will be considered miss, and the next note will be the [inputIndex]
                if (timeStamp + marginOfError <= audioTime)
                {
                    Miss();
                    averageCount = 0;
                }

            }
        }
    }

    private void SpawnMusicNote()
    {
        var note = Instantiate(notePrefab, transform);
        note.GetComponent<Note>().SetAssignedTime(timeStamps[spawnIndex]);
        note.GetComponent<Note>().SetNoteLength(noteDurations[spawnIndex]);
        notes.Add(note.GetComponent<Note>());
        spawnIndex++;
    }

    private void SpawnMusicBar()
    {
        var bar = Instantiate(barPrefab, transform);
        bar.GetComponent<Bar>().SetAssignedTime(SongManager.GetAudioSourceTime());
        barIndex++;
    }

    private void Hit()
    {
        var note = notes[inputIndex];
        ScoreManager.Hit();
        note.GetComponent<SpriteRenderer>().sprite = note.GetNoteRight();
        AnimationUtilities.Instance.AnimateHit(note.gameObject);
        inputIndex++;
    }

    private void Miss()
    {
        var note = notes[inputIndex];
        ScoreManager.Miss();
        notes[inputIndex].GetComponent<SpriteRenderer>().sprite = note.GetNoteWrong();
        AnimationUtilities.Instance.AnimateHit(note.gameObject);
        ReportMissedNote();
        inputIndex++;
    }

    // If detected pitch (played note by user) is equal to the current MIDI note or one octave's lower/higher
    // And the hit time is still within the range of marginOfError
    // Then the note will be considered correct
    private bool CheckPitch()
    {
        if (SongManager.Instance.IsAutoCorrect())
            return true;

        if (SongManager.Instance.GetDetectedPitch().GetMidiNote() == midiNotes[inputIndex] ||
            SongManager.Instance.GetDetectedPitch().GetMidiNote() + 12 == midiNotes[inputIndex] ||
            SongManager.Instance.GetDetectedPitch().GetMidiNote() - 12 == midiNotes[inputIndex])
            return true;

        return false;
    }

    // this function will get the note, but not the octave
    //private int GetIgnoredOctaveValue(double midiNote)
    //{
    //    // C1 midinote is 24
    //    if (midiNote < 24) return -1; // for now if it's less than C1, we'll just output it as too low hence the negative value
    //    return (int)((midiNote - 24) % 12); // 0 => C, 1 => C#, 2 => D, etc
    //}

    // a function to compare pitch, will return the difference between target pitch and the recorded pitch
    private int ComparePitch()
    {
        double currentMidiNote = midiNotes[inputIndex];
        double recordedMidiNote = SongManager.Instance.GetDetectedPitch().GetMidiNote();
        //double currentNote = GetIgnoredOctaveValue(midiNotes[inputIndex]); 
        //double recordedNote = GetIgnoredOctaveValue(recordedMidiNote);
        //if (recordedNote < currentNote && recordedMidiNote < currentMidiNote)
        //{
        //    ScoreManager.missMessage = "Too Low"; // if it's lower than the target pitch, it will return a negative value
        //}
        //else if (recordedNote > currentNote && recordedMidiNote > currentMidiNote)
        //{
        //    ScoreManager.missMessage = "Too High"; // if it's higher than the target pitch, it will return a non-zero positive value
        //}
        //else
        if (recordedMidiNote < currentMidiNote)
        {
            ScoreManager.missMessage = "Too Low";
        }
        else if (recordedMidiNote > currentMidiNote)
        {
            ScoreManager.missMessage = "Too High";
        }
        return (int)(recordedMidiNote - currentMidiNote);
    }

    // Destroy all spawned child objects in lane
    // In case of seeking clip to previous of next section, all instantiated game object will be stored as child elements of Lane objects
    // In order to not having multiple instance of the same notes/value stored and showed at the same time, we will destroy all spawned child objects before seeking audio clip
    public void DestroyChild()
    {
        var obj = GetComponent<Transform>();

        foreach (Transform child in obj)
        {
            Destroy(child.gameObject);
        }
    }

    // Clear/destroy rest of notes to match it with current inputIndex & spawnIndex
    public void ClearRest()
    {
        var clearCount = notes.Count - inputIndex;
        notes.RemoveRange(inputIndex, clearCount);
    }

    // Fill rest of notes with empty notes (in case of advancing the audio) to match with the correct inputIndex & spawnIndex
    public void FillRest()
    {
        var fillCount = Math.Abs(notes.Count - inputIndex);
        for (var i = 0; i < fillCount; i++)
        {
            var note = Instantiate(notePrefab, transform);
            notes.Add(note.GetComponent<Note>());
        }
    }

    public void SetIndexValue(int spawn, int input)
    {
        Instance.spawnIndex = spawn;
        Instance.inputIndex = input;
        Instance.barIndex = 0;
    }

    // Reset Lane to the initial state
    public void Reset()
    {
        spawnIndex = 0;
        inputIndex = 0;
        barIndex = 0;
        notes.Clear();
    }

    #region Analytics Function

    // This function analytics output is formatted in the funnels section of our dashboard. 
    void ReportMissedNote()
    {
        if (PlayerPrefs.GetInt(DeveloperMode.DisableAnalytics.ToString()) == 1)
        {
            Dictionary<string, object> customParams = new Dictionary<string, object>();
            customParams.Add("stage", GameController.Instance.currentStage);
            customParams.Add("level", GameController.Instance.selectedLevel);
            customParams.Add("step", inputIndex);

            var analytics = Analytics.CustomEvent("MissedNote", customParams);
            //Debug.Log("MissedNotes: "+analytics);
        }
        else
        {
            Debug.Log("Missed Note not Uploaded to Analytics");
        }
    }

    #endregion
}
