using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// enum to define keys for using playerprefs


// GameController is our Singleton Class that serves as the game's controller (duh)
// It stores the game's global variable such as game currency and player's current skin (future development)
public class GameController : MonoBehaviour
{
    public static GameController instance;
    private int totalCoin;
    private int currentCharacter; // currentSkin is a variable that holds current characterId
    public GameObject[] coinAmount;
    public GameObject[] coinChangeIndicator;

    private enum PlayerDataKey
    {
        CoinAmount,
        Character,
    };


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
        ShowCoinAmount();
    }

    // Initialize values from PlayerPrefs
    private void InitializeVariable()
    {
        // Both currentSkin and totalCoin default value is 0
        this.currentCharacter = PlayerPrefs.GetInt(PlayerDataKey.Character.ToString(), 0);
        this.totalCoin = PlayerPrefs.GetInt(PlayerDataKey.CoinAmount.ToString(), 0);

        ShowCoinAmount();
    }

    void ShowCoinAmount()
    {
        // setting the value into the game object 
        coinAmount = GameObject.FindGameObjectsWithTag("CoinAmount");

        foreach (GameObject c in coinAmount)
        {
            c.GetComponent<TMP_Text>().text = totalCoin.ToString();
        }

    }

    // animate the decrease or increase in coinAmount
    // parameter:
    // - string sign => a "+" or "-"
    // - int amount => an integer that represents the amount increased or decreased
    private void AnimateCoinChange(string sign, int amount)
    {
        coinChangeIndicator = GameObject.FindGameObjectsWithTag("CoinChangeIndicator");
        foreach (GameObject c in coinChangeIndicator)
        {
            c.GetComponent<TMP_Text>().text = sign +""+amount.ToString();
            AnimationUtilities.AnimateAddMoney(c); // calls on AnimationUtilities class
            break;
        }

    }

    // public function where you can add any amount of coin
    public void CoinAdd(int coinAmount)
    {
        AnimateCoinChange("+", coinAmount);
        this.totalCoin += coinAmount;
        PlayerPrefs.SetInt(PlayerDataKey.CoinAmount.ToString(), totalCoin); // Automatically saves the new value to PlayerPrefs
    }
    // public function where you can substract any amount of coin
    public void CoinSubstract(int coinAmount)
    {
        AnimateCoinChange("-", coinAmount);
        this.totalCoin -= coinAmount;
        PlayerPrefs.SetInt(PlayerDataKey.CoinAmount.ToString(), totalCoin); // Automatically saves the new value to PlayerPrefs
    }

    public void SetCurrentSkin(int skinId)
    {
        this.currentCharacter = skinId;
        PlayerPrefs.SetInt(PlayerDataKey.Character.ToString(), skinId); // Automatically saves the new value to PlayerPrefs
    }

    public int GetCurrentSkin()
    {
        return this.currentCharacter;
    }

    public int GetTotalCoin()
    {
        return this.totalCoin;
    }

}
