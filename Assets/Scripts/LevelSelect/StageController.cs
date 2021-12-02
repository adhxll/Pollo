using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public List<GameObject> levels;
    public List<LevelItem> LevelData = new List<LevelItem>();
    public int stageID; 
    private void Awake()
    {
        LoadLevelData();
        SetupScene(); 
    }
    private void LoadLevelData()
    {
        GameObject lvl;
        for (int i = 0; i < levels.Count; i++)
        { 
            lvl = levels[i];
            //TODO: Change stageID dari 0 menjadi stageID yang diatas setelah bikin stage2
            string dictKey = DataController.Instance.FormatKey(0, (i + 1));
            lvl.GetComponent<LevelItem>().data = DataController.Instance.playerData.levelData[dictKey];
        }
    }
    private void SetupScene() {
        for (int i = 0; i < levels.Count; i++)
        {
            LevelData.Add(levels[i].GetComponent<LevelItem>());
        }
    }
}
