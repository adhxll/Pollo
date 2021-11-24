using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class LevelSelectionController : MonoBehaviour
{
    public GameObject[] levels;
    public List<LevelItem> LevelData = new List<LevelItem>();
    public Canvas levelCanvas;
    private bool modalShown; 
    void Awake()
    {
        SetupScene(); 
        LoadLevelData();
    }
    private void SetupScene() {
        Camera cam = Camera.main;

        for (int i = 0; i < levels.Length; i++)
        {
            LevelData.Add(levels[i].GetComponent<LevelItem>());
        }
    }
   
    private void LoadLevelData()
    {
        //TODO: Load data from binary here
        GameObject lvl; 
        for (int i = 0; i < levels.Length; i++) {
            lvl = levels[i];
            lvl.GetComponent<LevelItem>().data = DataController.Instance.playerData.levelData[i]; 
        }
    }
     LevelItemContainer GenerateDummy(int idCount) {
        //generate dummy, for testing only
        LevelItemContainer data = new LevelItemContainer();
        data.levelID = idCount + 1; 
        data.starCount = 0; 
        data.isUnlocked = true; 
        return data;    
    }
    public void ModifyLevelInput() { //disables level button input when modal is shown
        if (!modalShown) 
        {
            levelCanvas.GetComponent<GraphicRaycaster>().enabled = false;
            modalShown = true; 
        }
        else
        {
            levelCanvas.GetComponent<GraphicRaycaster>().enabled = true;
            modalShown = false; 
        }
    }
    public void SaveGame() {
        SaveSystem.SavePlayerData(); 
    }
   
  
   
    
    
    
  




}
