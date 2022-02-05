using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepAudioAwake : MonoBehaviour
{
    public static KeepAudioAwake Instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if(Instance != null) Destroy(gameObject);
        else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Start(){
        StartCoroutine(PlayMusic());
    }

    public IEnumerator PlayMusic()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log(SceneManager.GetActiveScene().name);
        if (!audioSource.isPlaying) {
            if(SceneManager.GetActiveScene().name != "Homepage" && SceneManager.GetActiveScene().name != "Achievements"){
                Destroy(gameObject);
            }
            else{
                if (PlayerPrefs.GetInt("IsFirstTime") == 1) audioSource.Play();
            }
        }
    }
 
    public void StopMusic()
    {
        audioSource.Stop();
    }
}
