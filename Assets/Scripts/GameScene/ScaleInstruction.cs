using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleInstruction : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI instructionText;
    //public List<GameObject> pianoTiles = new List<GameObject>();

    [SerializeField]
    private GameObject[] instructionBoard = null;
    [SerializeField]
    private GameObject[] pianoTiles = null;

    private int[] majorRange = { 2, 2, 1, 2, 2, 2, 1};
    private int startIndex = 0;
    private string keySignature;

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
        //StartCoroutine(SceneStateManager.Instance.AnimateObjects(instructionBoard, 0.1f, SceneStateManager.AnimationType.MoveY, 0f, 5f));
        //yield return new WaitForSeconds(1);
        //StartCoroutine(SceneStateManager.Instance.AnimateObjects(pianoTiles, 0.1f, SceneStateManager.AnimationType.MoveY, 0f, 5f));
        //yield return new WaitForSeconds(1);
        Debug.Log($"Current index : {startIndex}");

        int currentIndex = startIndex;
        for (int index = 0; index <= majorRange.Length; index++)
        {
            Color baseColor = pianoTiles[currentIndex].GetComponent<Image>().color;
            pianoTiles[currentIndex].GetComponent<Image>().color = yellowColor;
            yield return new WaitForSeconds(1f);
            pianoTiles[currentIndex].GetComponent<Image>().color = baseColor;
            if (index != majorRange.Length) currentIndex += majorRange[index];
            else
            {
                index = -1;
                currentIndex = startIndex;
            }
            //Debug.Log(index);
            Debug.Log($"Current index : {currentIndex}");
        }
        yield return new WaitForSeconds(1);
    }

    void setupKeySignature()
    {
        //var keySignature = SongManager.midiFile.header.name;
        keySignature = "F#";

        instructionText.text = $"For this level, you are going to play on <color=#7E2684>{keySignature} Major</color> Scale.";

        foreach(GameObject tiles in pianoTiles)
        {
            if (tiles.name == keySignature) break;
            else startIndex++;
        }
        Debug.Log(keySignature);
    }
}
