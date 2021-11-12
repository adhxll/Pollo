using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class LevelSelectionController : MonoBehaviour
{
    public GameObject[] Levels;
    public List<LevelItem> LevelData = new List<LevelItem>();
   
    void Awake()
    {
        SetupScene(); 
        LoadLevelData();
    }
    private void SetupScene() {
        Camera cam = Camera.main;

        for (int i = 0; i < Levels.Length; i++)
        {
            LevelData.Add(Levels[i].GetComponent<LevelItem>());
        }
    }
    private void LoadLevelData()
    {
        //TODO: Load data from binary here
        GameObject lvl; 
        for (int i = 0; i < Levels.Length; i++) {
            lvl = Levels[i];
            lvl.GetComponent<LevelItem>().data = GenerateDummy(i);  
        }
    }
     LevelItemContainer GenerateDummy(int idCount) {
        //generate dummy, for testing only
        LevelItemContainer data = new LevelItemContainer(idCount+1);
        data.starCount = 0; 
        data.isUnlocked = true; 
        return data;    
    }
   
    
    
    
  




}
