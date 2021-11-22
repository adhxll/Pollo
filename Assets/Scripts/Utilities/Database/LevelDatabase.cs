using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Database", menuName = "Assets/Database/Level")]
public class LevelDatabase : ScriptableObject
{
    public List<Level> allLevels;
}
