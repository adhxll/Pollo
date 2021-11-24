using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public static DataController Instance;
    public LevelDatabase levelDatabase;
    //TODO: Add databases for Achievements and Skins
    public PlayerData playerData;
    private void Awake()
    {
        StartSingleton(); 
    }
    void Start()
    {
        InitializePlayerData(); 
    }

    void StartSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void InitializePlayerData() {
        if (SaveSystem.LoadPlayerData() != null) playerData = SaveSystem.LoadPlayerData();
        else
        {
            playerData = new PlayerData(levelDatabase);
        }
    } 
}
