using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

// enum to define keys for using playerprefs


// GameController is our Singleton Class that serves as the game's controller (duh)
// It stores the game's global variable such as game currency and player's current skin (future development)
public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private int totalCoin;
    private int currentCharacter; // currentSkin is a variable that holds current characterId
    public GameObject[] coinAmount;
    public GameObject[] coinChangeIndicator;
    public int selectedLevel = 0;
    public int currentStage = 0; 
    public SceneStateManager.SceneState sceneState = SceneStateManager.SceneState.Onboarding;
    [SerializeField] public AudioMixer masterMixer;

    private enum PlayerDataKey
    {
        CoinAmount,
        Character,
    };


    private void Awake()
    {
        StartSingleton();
        InitializeVariable();
        SceneManager.activeSceneChanged += ChangedActiveScene; // subscribe to an event that alerts us whenever the scene has changed
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initializing master audio mixer
        // This must be put in the Start() function because of unity's bug
        ResetMixer();
    }

    // Singleton pattern
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
    // Update is called once per frame
    void Update()
    {
        // going to delete this later when we figure out the gamescene state
        SettingsController.SetSettingsButton(); // search for any GameObject that has  the tag 'SettingsButton'
    }

    // Initialize values from PlayerPrefs
    private void InitializeVariable()
    {
        // Both currentSkin and totalCoin default value is 0
        this.currentCharacter = PlayerPrefs.GetInt(PlayerDataKey.Character.ToString(), 0);
        this.totalCoin = PlayerPrefs.GetInt(PlayerDataKey.CoinAmount.ToString(), 0);
        ShowCoinAmount();

        

    }

    public void ResetMixer()
    {
        // Reset the mixer
        masterMixer.SetFloat("soundEffects", Mathf.Log10(PlayerPrefs.GetFloat(SettingsList.SoundEffects.ToString())) * 20);
        masterMixer.SetFloat("musicVolume", Mathf.Log10(PlayerPrefs.GetFloat(SettingsList.Music.ToString())) * 20);
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
        ShowCoinAmount();
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
        AudioController.Instance.PlayCoinAddSound();
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

    // a function that detects when the scene has changed
    // but it doesn't detect when an additive scene is loaded, so it's kinda useless :/
    private void ChangedActiveScene(Scene current, Scene next)
    {
        string currentName = current.name;

        if (currentName == null)
        {
            // Scene1 has been removed
            currentName = "Replaced";
        }
        SettingsController.SetSettingsButton(); // when the scene changes, search for any GameObject that has  the tag 'SettingsButton'
        Debug.Log("Scenes: " + currentName + ", " + next.name);
    }
}
