using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// GameController is our Singleton Class that serves as the game's controller (duh)
// It stores the game's global variable such as game currency and player's current skin (future development)
public class GameController : MonoBehaviour
{
    public static GameController instance;
    private int totalCoin;
    private int currentSkin; // currentSkin is a variable that holds skinId

    private void Awake()
    {
        StartSingleton();
        InitializeVariable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Singleton pattern
    void StartSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    // Initialize values from PlayerPrefs
    private void InitializeVariable()
    {
        // Both currentSkin and totalCoin default value is 0
        this.currentSkin = PlayerPrefs.GetInt("CurrentSkin", 0);
        this.totalCoin = PlayerPrefs.GetInt("TotalCoin", 0);
    }

    void setTotalCoin(int newTotalCoinValue)
    {
        this.totalCoin = newTotalCoinValue;
        PlayerPrefs.SetInt("TotalCoin", newTotalCoinValue); // Automatically saves the new value to PlayerPrefs
    }

    void setCurrentSkin(int newCurrentSkin)
    {
        this.currentSkin = newCurrentSkin;
        PlayerPrefs.SetInt("CurrentSkin", newCurrentSkin); // Automatically saves the new value to PlayerPrefs
    }

    int getCurrentSkin()
    {
        return this.currentSkin;
    }

    int getTotalCoin()
    {
        return this.totalCoin;
    }

}
