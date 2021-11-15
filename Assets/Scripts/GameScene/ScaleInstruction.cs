using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleInstruction : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip[] audioClips = null;
    [SerializeField]
    private TextMeshProUGUI instructionText = null;
    [SerializeField]
    private GameObject[] instructionBoard;
    [SerializeField]
    private GameObject[] pianoTiles = null;

    [SerializeField]
    private float playbackSpeed = 1.0f;
    [SerializeField]
    private string keySignature = "C";

    //A major range scale multiplied by 2
    private int[] majorRange = { 2, 2, 1, 2, 2, 2, 1 };
    private int startIndex = 0;
    
    private Color yellowColor = new Color32 (255, 200, 113, 255);

    // Start is called before the first frame update
    void Start()
    {
        StartScale();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void StartScale()
    {
        if (SceneStateManager.Instance.GetSceneState() == SceneStateManager.SceneState.Instruction)
        {
            SetupKeySignature();
            StartCoroutine(ScaleInstructionStart());
        }
    }

    IEnumerator ScaleInstructionStart()
    {
        yield return new WaitForSeconds(2);

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
        }
        yield return new WaitForSeconds(1);
    }

    void SetupKeySignature()
    {
        keySignature = SongManager.Instance.GetMidiFile().header.keySignatures[0].key;

        instructionText.text = $"For this level, you are going to play on <color=#7E2684>{keySignature} Major</color> Scale.";

        foreach(GameObject tiles in pianoTiles)
        {
            if (tiles.name == keySignature) break;
            else startIndex++;
        }
    }

    public void AudioSourceStop(){
        Destroy(gameObject);
    }

}
