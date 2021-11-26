using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public List<LevelItemContainer> levelData = new List<LevelItemContainer>();
    
    public PlayerData(LevelDatabase database)
    {
        //populate onboarding and first level
        LevelItemContainer level = new LevelItemContainer();
        level.levelID = 0;
        level.starCount = 0;
        levelData.Add(level);

        level = new LevelItemContainer();
        level.levelID = 1;
        level.isUnlocked = true;
        levelData.Add(level); 

        //populate levels with empty values
        for (int i = 2; i < database.allLevels.Count; i++)
        {
            LevelItemContainer newLevel = new LevelItemContainer();
            newLevel.levelID = i;
            newLevel.starCount = 0; 
            levelData.Add(newLevel);
            Debug.Log("Container ID : " + i + " Star Count : " + levelData[i].starCount);
        }
       
    }
   

   


}

    

