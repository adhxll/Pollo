using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;
using System;
using DG.Tweening;

public class Lane : MonoBehaviour
{
    public TMPro.TextMeshProUGUI accuracyScore;
    public TMPro.TextMeshProUGUI accuracyPercentage;

    public KeyCode input;
    public GameObject notePrefab;
    public GameObject barPrefab;

    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    public List<float> noteDurations = new List<float>();
    public List<int> midiNotes = new List<int>();

    int spawnIndex = 0;
    int inputIndex = 0;
    int barIndex = 0;
    int correctNotes = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

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
                //Debug.Log($"Current Time = {SongManager.GetAudioSourceTime()}, Current Beat = {SongManager.GetCurrentBeat()}, Current Index {barIndex}");
                SpawnMusicBar();
            }

            if (spawnIndex < timeStamps.Count)
            {
                if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
                {
                    SpawnMusicNote();
                }
            }

            if (inputIndex < timeStamps.Count)
            {
                double timeStamp = timeStamps[inputIndex];
                double marginOfError = SongManager.Instance.marginOfError;
                double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

                if (SongManager.Instance.detectedPitch.midiNote == midiNotes[inputIndex])
                {
                    if (Math.Abs(audioTime - timeStamp) < marginOfError)
                    {
                        Hit();
                    }
                    else
                    {
                        //print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                    }
                }

                if (timeStamp + marginOfError <= audioTime)
                {
                    Miss();
                }
            }

            accuracyScore.text = $"{correctNotes} / {inputIndex}";
            accuracyPercentage.text = ((float)correctNotes / inputIndex * 100).ToString("0.00") + " %";
            //Debug.Log($"ACCURACY {(float)correctNotes / inputIndex * 100}%");
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
        AnimationManager.Instace.AnimateHit(note.gameObject, 0.1f);

        correctNotes++;
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
}
