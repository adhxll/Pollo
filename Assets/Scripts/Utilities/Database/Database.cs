using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Database : MonoBehaviour
{
    private static Database Instance;
    [SerializeField]
    private LevelDatabase levels = null;

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

    public static Level GetLevelAndStage(int stageID, int levelID)
    {
        return Instance.levels.allLevels.FirstOrDefault(i => i.GetLevelID() == levelID && i.GetStageID() == stageID);
    }
}
