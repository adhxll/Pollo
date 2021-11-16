using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pitch.Algorithm
{
    public class PitchUtilities
    {
        // Taken from PitchTracker
        // https://pitchtracker.codeplex.com/

        public static readonly double InverseLog2 = 1.0 / Math.Log10(2.0);

        private const int kMinMidiNote = 21;  // A0
        private const int kMaxMidiNote = 108; // C8

        private float m_minPitch;
        private float m_maxPitch = 0;

        /// <summary>
        /// Get the MIDI note and cents of the pitch 
        /// </summary>
        /// <param name="pitch"></param>
        /// <param name="note"></param>
        /// <param name="cents"></param>
        /// <returns></returns>
        public static bool PitchToMidiNote(float pitch, out int note, out int cents)
        {
            if (pitch < 20.0f)
            {
                note = 0;
                cents = 0;
                return false;
            }

            var fNote = (float)((12.0 * Math.Log10(pitch / 55.0) * InverseLog2)) + 33.0f;
            note = (int)(fNote + 0.5f);
            cents = (int)((note - fNote) * 100);
            return true;
        }

        /// <summary>
        /// Get the pitch from the MIDI note
        /// </summary>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public static float PitchToMidiNote(float pitch)
        {
            if (pitch < 20.0f)
                return 0.0f;

            return (float)(12.0 * Math.Log10(pitch / 55.0) * InverseLog2) + 33.0f;
        }

        /// <summary>
        /// Get the pitch from the MIDI note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public float MidiNoteToPitch(float note)
        {
            if (note < 33.0f)
                return 0.0f;

            var pitch = (float)Math.Pow(10.0, (note - 33.0f) / InverseLog2 / 12.0f) * 55.0f;
            return pitch <= m_maxPitch ? pitch : 0.0f;
        }

        /// <summary>
        /// Format a midi note to text
        /// </summary>
        /// <param name="note"></param>
        /// <param name="sharps"></param>
        /// <param name="showOctave"></param>
        /// <returns></returns>
        public static string GetNoteName(int note, bool sharps, bool showOctave)
        {
            if (note < kMinMidiNote || note > kMaxMidiNote)
                return null;

            note -= kMinMidiNote;

            int octave = (note + 9) / 12;
            note = note % 12;
            string noteText = null;

            switch (note)
            {
                case 0:
                    noteText = "A";
                    break;

                case 1:
                    noteText = sharps ? "A#" : "Bb";
                    break;

                case 2:
                    noteText = "B";
                    break;

                case 3:
                    noteText = "C";
                    break;

                case 4:
                    noteText = sharps ? "C#" : "Db";
                    break;

                case 5:
                    noteText = "D";
                    break;

                case 6:
                    noteText = sharps ? "D#" : "Eb";
                    break;

                case 7:
                    noteText = "E";
                    break;

                case 8:
                    noteText = "F";
                    break;

                case 9:
                    noteText = sharps ? "F#" : "Gb";
                    break;

                case 10:
                    noteText = "G";
                    break;

                case 11:
                    noteText = sharps ? "G#" : "Ab";
                    break;
            }

            if (showOctave)
                noteText += " " + octave.ToString();

            return noteText;
        }
    }
}