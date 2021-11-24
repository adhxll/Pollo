using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public List<LevelItemContainer> levelData = new List<LevelItemContainer>();
    
    // Start is called before the first frame update
    public PlayerData(LevelDatabase database)
    {
        //populate levels with empty values
        for (int i = 0; i < database.allLevels.Count; i++)
        {
            LevelItemContainer level = new LevelItemContainer();
            level.levelID = i + 1; 
            level.starCount = Random.Range(0, 4);
            levelData.Add(level);
            Debug.Log("Container ID : " + i + " Star Count : " + levelData[i].starCount);
        }
       
    }

   


}

    

