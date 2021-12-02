using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Dictionary<string, LevelItemContainer> levelData = new Dictionary<string, LevelItemContainer>();
    //dict cannot be serialized into json using JsonUtility, so we use levelList to save
    public List<LevelItemContainer> levelList = new List<LevelItemContainer>();
}

    

