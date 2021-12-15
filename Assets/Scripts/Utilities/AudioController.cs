using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundNames
{
    click,
    countdown,
    coinadd,
    star1, star2, star3
}

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip[] sounds = null;
    private Dictionary<SoundNames, AudioClip> soundBank = new Dictionary<SoundNames, AudioClip>();

    private void Awake()
    {
        if(Instance != null) Destroy(gameObject);
        else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        SetupDictionary();
    }

    private void SetupDictionary(){
        for (int i=0; i<sounds.Length; i++){
            soundBank.Add((SoundNames)i, sounds[i]);
        }
    }

    public void PlaySound(SoundNames sound)
    {
        //soundNames parsed_enum = (soundNames)System.Enum.Parse( typeof(soundNames), sound);
        audioSource.clip = soundBank[sound];
        audioSource.Play();
    }

}
