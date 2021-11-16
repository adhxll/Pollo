using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchDetector : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private float pitch;
    [SerializeField]
    private int midiNote;

    public AudioSource GetSource() { return this.source; }
    public void SetSource(AudioSource source) { this.source = source; }
    public float GetPitch() { return this.pitch; }
    public void SetPitch(float pitch) { this.pitch = pitch; }
    public int GetMidiNote() { return this.midiNote; }
    public void SetMidiNote(int midiNote) { this.midiNote = midiNote; }
}
