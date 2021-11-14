using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Database : MonoBehaviour
{
    private static Database Instance;
    [SerializeField]
    private LevelDatabase levels;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Level GetLevelByID(int ID)
    {
        return Instance.levels.allLevels.FirstOrDefault(i => i.GetLevelID() == ID);
    }
}
