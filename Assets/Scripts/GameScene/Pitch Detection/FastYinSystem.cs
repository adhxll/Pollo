using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastYinSystem : MonoBehaviour
{
    PitchDetector pitchDetector;
    FastYin fastYin;

    AudioSource audioSource;
    string microphone = null;
    int tempMidi = 0;
    double dspTime;

    private void Awake()
    {
        pitchDetector = GetComponent<PitchDetector>();
        audioSource = pitchDetector.source;
    }

    // Start is called before the first frame update
    void Start()
    {
        fastYin = new FastYin(pitchDetector.source.clip.frequency, 1024);
    }

    // Update is called once per frame
    void Update()
    {
        AnalyzeSound();
    }

    private void AnalyzeSound()
    {
        var buffer = new float[1024];
        pitchDetector.source.GetOutputData(buffer, 0);

        var result = fastYin.getPitch(buffer);

        var pitch = result.getPitch();
        var midiNote = 0;
        var midiCents = 0;

        Pitch.PitchDsp.PitchToMidiNote(pitch, out midiNote, out midiCents);

        pitchDetector.pitch = pitch;
        pitchDetector.midiNote = midiNote;

        if (midiNote != 0 && midiNote != tempMidi)
        {
            tempMidi = midiNote;
            Debug.Log($"FASTYIN Transcribed : {midiNote}, time : {SongManager.GetAudioSourceTime()}");
            //Debug.Log($"FASTYIN Transcribed : {midiNote}, time : {AudioSettings.dspTime - dspTime}");
        }
    }

    //void StartPlaying()
    //{
    //    dspTime = AudioSettings.dspTime;
    //    pitchDetector.source.PlayScheduled(0);
    //}

    public void StarRecording()
    {
        audioSource.clip = Microphone.Start(null, true, 60, 44100);

        audioSource.loop = true;
        audioSource.mute = false;

        if (Microphone.IsRecording(microphone))
        {
            while (!(Microphone.GetPosition(microphone) > 0)) { }
            audioSource.Play();
        }
    }
}
