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

    List<Note> notes = new List<Note>();

    [HideInInspector]
    public List<double> timeStamps = new List<double>();    // A list that store each notes timestamp (telling the exact time when its need to be spawned)

    [HideInInspector]
    public List<float> noteDurations = new List<float>();   // A list that store each notes ticks duration

    [HideInInspector]
    public List<int> midiNotes = new List<int>();   // A list that store what MIDI notes need to be played at the given note

    int spawnIndex = 0;
    int inputIndex = 0;
    int barIndex = 0;
    int averageCount = 0; // Variable to count in exact midi note to prevent Hit() function called accidentally

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
            noteDurations.Add((float)note.durationTicks / SongManager.midiFile.header.ppq);
            midiNotes.Add(note.midi);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SongManager.Instance.songPlayed)
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
                SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
                SpawnMusicNote();

            // Configure what note that need to be hit at the given time, based on the spawn note
            if (inputIndex < timeStamps.Count)
            {
                double timeStamp = timeStamps[inputIndex];
                double marginOfError = SongManager.Instance.marginOfError;
                double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

                // If detected pitch (played note by user) is equal to the current MIDI note or one octave's lower/higher
                // And the hit time is still within the range of marginOfError
                // Then the note will be considered correct
                if ((SongManager.Instance.detectedPitch.midiNote == midiNotes[inputIndex] ||
                   SongManager.Instance.detectedPitch.midiNote + 12 == midiNotes[inputIndex] ||
                   SongManager.Instance.detectedPitch.midiNote - 12 == midiNotes[inputIndex]) &&
                   Math.Abs(audioTime - timeStamp) < marginOfError)
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
        note.GetComponent<Note>().assignedTime = timeStamps[spawnIndex];
        note.GetComponent<Note>().noteLength = noteDurations[spawnIndex];
        notes.Add(note.GetComponent<Note>());
        spawnIndex++;
    }

    private void SpawnMusicBar()
    {
        var bar = Instantiate(barPrefab, transform);
        bar.GetComponent<Bar>().assignedTime = SongManager.GetAudioSourceTime();
        barIndex++;
    }

    private void Hit()
    {
        var note = notes[inputIndex];
        ScoreManager.Hit();
        note.GetComponent<SpriteRenderer>().sprite = note.noteRight;
        AnimationManager.Instace.AnimateHit(note.gameObject, -0.1f);
        inputIndex++;
    }

    private void Miss()
    {
        var note = notes[inputIndex];
        ScoreManager.Miss();
        notes[inputIndex].GetComponent<SpriteRenderer>().sprite = note.noteWrong;
        AnimationManager.Instace.AnimateHit(note.gameObject, -0.1f);
        inputIndex++;
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
