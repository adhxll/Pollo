using System;
using UnityEngine;

public class MIDI
{
    [System.Serializable]
    public class MidiFile
    {
        public Header header;
        public Tracks[] tracks;
    }

    [System.Serializable]
    public class Header
    {
        public string name;
        public int ppq; // pulse per quarter
        public Tempos[] tempos;
    }

    [System.Serializable]
    public class Tempos
    {
        public int bpm;
        public int ticks;
    }

    [System.Serializable]
    public class Tracks
    {
        public Notes[] notes;
    }

    [System.Serializable]
    public class Notes
    {
        public double duration;
        public int durationTicks;
        public int midi;
        public string name;
        public int ticks;
        public float time;
        public double velocity;
    }

    public static MidiFile CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<MidiFile>(jsonString);
    }
}
