using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Lane : MonoBehaviour
{
    //public TMPro.TextMeshProUGUI accuracyScore;
    //public TMPro.TextMeshProUGUI accuracyPercentage;

    public static Lane Instance;

    [SerializeField]
    private GameObject notePrefab = null;

    [SerializeField]
    private GameObject barPrefab = null;

    [SerializeField]
    private List<Note> notes = new List<Note>();

    [HideInInspector]
    private List<double> timeStamps = new List<double>();    // A list that store each notes timestamp (telling the exact time when its need to be spawned)

    [HideInInspector]
    private List<float> noteDurations = new List<float>();   // A list that store each notes ticks duration

    [HideInInspector]
    private List<int> midiNotes = new List<int>();   // A list that store what MIDI notes need to be played at the given note

    private int spawnIndex = 0;
    private int inputIndex = 0;
    private int barIndex = 0;
    private int averageCount = 0; // Variable to count in exact midi note to prevent Hit() function called accidentally

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

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Iterate through the given MIDI (or JSON in this case)
    // and set timeStamps, noteDurations, and midiNotes to the list.
    public void SetTimeStamps(MIDI.Notes[] array)
    {
        foreach (var note in array)
        {
            timeStamps.Add(note.time);
            noteDurations.Add((float)note.durationTicks / SongManager.Instance.GetMidiFile().header.ppq);
            midiNotes.Add(note.midi);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SongManager.Instance.GetSongPlayed())
        {
            if (SongManager.GetCurrentBeat() >= barIndex)
            {
                // Handle fast forward, if the clip is being fast forwarded, then barIndex will be equal to the integer value of current beat
                // So it'll not be spawned multiple time (since the default increment is 1)
                barIndex = (int)SongManager.GetCurrentBeat();
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

                if (SceneStateManager.Instance.GetSceneState() == SceneStateManager.SceneState.Onboarding)
                {
                    if (inputIndex < notes.Count && notes[inputIndex].transform.localPosition.x < -0.5f)
                    {
                        SongManager.Instance.PauseSong();

                        if (CheckPitch())
                        {
                            SongManager.Instance.ResumeSong();
                        }
                    }
                }

                if (CheckPitch() && Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    averageCount++;
                    if (averageCount >= 3)
                    {
                        Hit();
                        averageCount = 0;
                    }
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
        inputIndex++;
    }

    // If detected pitch (played note by user) is equal to the current MIDI note or one octave's lower/higher
    // And the hit time is still within the range of marginOfError
    // Then the note will be considered correct
    private bool CheckPitch()
    {
        if (SongManager.Instance.GetDetectedPitch().GetMidiNote() == midiNotes[inputIndex] ||
            SongManager.Instance.GetDetectedPitch().GetMidiNote() + 12 == midiNotes[inputIndex] ||
            SongManager.Instance.GetDetectedPitch().GetMidiNote() - 12 == midiNotes[inputIndex])
            return true;

        return false;
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
}
