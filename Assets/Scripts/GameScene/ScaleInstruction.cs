using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleInstruction : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] audioClips;
    [SerializeField]
    private TextMeshProUGUI instructionText;
    [SerializeField]
    private GameObject[] instructionBoard;
    [SerializeField]
    private GameObject[] pianoTiles;

    [SerializeField]
    private float playbackSpeed;
    [SerializeField]
    private string keySignature;

    //A major range scale multiplied by 2
    private int[] majorRange = { 2, 2, 1, 2, 2, 2, 1 };
    private int startIndex = 0;
    

    private Color yellowColor = new Color32 (255, 200, 113, 255);

    // Start is called before the first frame update
    void Start()
    {
        setupKeySignature();
        StartCoroutine(ScaleInstructionStart());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator ScaleInstructionStart()
    {
        yield return new WaitForSeconds(2);

        Debug.Log($"Current index : {startIndex}");

        int currentIndex = startIndex;
        for (int index = 0; index <= majorRange.Length; index++)
        {
            Color baseColor = pianoTiles[currentIndex].GetComponent<Image>().color;
            pianoTiles[currentIndex].GetComponent<Image>().color = yellowColor;
            audioSource.clip = audioClips[currentIndex];
            audioSource.Play();

            yield return new WaitForSeconds(playbackSpeed);
            pianoTiles[currentIndex].GetComponent<Image>().color = baseColor;
            if (index != majorRange.Length) currentIndex += majorRange[index];
            else
            {
                index = -1;
                currentIndex = startIndex;
            }

            Debug.Log($"Current index : {currentIndex}");
        }
        yield return new WaitForSeconds(1);
    }

    void setupKeySignature()
    {
        //var keySignature = SongManager.midiFile.header.keySignatures[0].key;

        instructionText.text = $"For this level, you are going to play on <color=#7E2684>{keySignature} Major</color> Scale.";

        foreach(GameObject tiles in pianoTiles)
        {
            if (tiles.name == keySignature) break;
            else startIndex++;
        }
        Debug.Log(keySignature);
    }

    public void AudioSourceStop(){
        Destroy(gameObject);
    }

}
