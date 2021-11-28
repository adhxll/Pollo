using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public GameObject[] levels;
    public List<LevelItem> LevelData = new List<LevelItem>();
    private void Awake()
    {
        LoadLevelData();
        SetupScene(); 
    }
    private void LoadLevelData()
    {
        int currentStage = GameController.Instance.currentStage;
        GameObject lvl;
        for (int i = 0; i < levels.Length; i++)
        { //TODO: change levels.length menjadi something else untuk support new islands
            lvl = levels[i];
            string dictKey = currentStage + "-" + (i + 1);
            lvl.GetComponent<LevelItem>().data = DataController.Instance.playerData.levelData[dictKey];
        }
    }
    private void SetupScene() {
        for (int i = 0; i < levels.Length; i++)
        {
            LevelData.Add(levels[i].GetComponent<LevelItem>());
        }
    }
}
