using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage Database", menuName = "Assets/Database/Stage Database")]
//used to store the stage prefabs (islands) in the game using a scriptableobject that can be referenced anywhere
public class Stages : ScriptableObject
{
    public List<GameObject> stagesList; 
}
