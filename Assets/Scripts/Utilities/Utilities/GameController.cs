using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

// GameController for now only contains some objects and data that are carried around the game and needs to be referenced and present in other classes
// yes, the name is misleading. Consider changing it to GameModel one day once all of the code is nicely refactored
public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public CoinController coinController;
    public CharacterModel character;
    [SerializeField] public AudioMixer masterMixer;

    // Scene data
    public int selectedLevel = 0;
    public int currentStage = 0; 
    public SceneStateManager.SceneState sceneState = SceneStateManager.SceneState.Onboarding; // this refer more to the 


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

    // Initialize values from PlayerPrefs
    private void InitializeVariable()
    {
        // Both currentSkin and totalCoin default value is 0
        coinController = gameObject.AddComponent<CoinController>();
        character = gameObject.AddComponent<CharacterModel>();
    }

    public void ResetMixer()
    {
        // Reset the mixer
        masterMixer.SetFloat("soundEffects", Mathf.Log10(PlayerPrefs.GetFloat(SettingsList.SoundEffects.ToString())) * 20);
        masterMixer.SetFloat("musicVolume", Mathf.Log10(PlayerPrefs.GetFloat(SettingsList.Music.ToString())) * 20);
    }

    // a function that detects when the scene has changed
    // but it doesn't detect when an additive scene is loaded, so it's kinda useless :/
    // but for now, whenever a scene changes, it will set the settings button automatically
    // for a settings button in an additive scene, i have to set it manually
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
