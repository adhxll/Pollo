using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events; 

public class SceneManagerScript : MonoBehaviour
{
    public enum SceneName
    {
        Homepage,
        LevelSelection,
        LSModal,
        GameScene,
        GSPianoScale,
        GSPause,
        ResultPage,
        Settings,
        Achievements,
        CharacterSelection
    }

    public static SceneManagerScript Instance;
    private string sceneIdentifier;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CheckFirstTime();
    }

    // Check whether it's user first time in opening the app, if true, go to onboarding
    void CheckFirstTime()
    {
        if (SceneManager.GetActiveScene().name == "Homepage" && PlayerPrefs.GetInt("IsFirstTime") == 0)
            SceneInvoke(SceneName.GameScene);
    }

    // Invoke scene with defined enum
    public void SceneInvoke(SceneName sceneName, bool additive = false, float delay = 0f)
    {

        sceneIdentifier = SceneToString(sceneName);

        if (additive)
            Invoke(nameof(AddScene), delay);
        else
            Invoke(nameof(GoToScene), delay);

    }

    // Unload additive scene with defined enum
    public void SceneUnload(SceneName sceneName, float delay = 0f)
    {
        sceneIdentifier = SceneToString(sceneName);
        Invoke(nameof(UnloadScene), delay);
    }

    // Invoke scene without delay
    public void SceneInvoke(string sceneName) 
    {
        sceneIdentifier = sceneName;
        Invoke(nameof(GoToScene), 0f);
    }

    // Invoke scene with delay
    public void DelayedSceneInvoke(string sceneName)
    {
        sceneIdentifier = sceneName;
        Invoke(nameof(GoToScene), 0.1f);
    }

    // Load scene - Normal
    private void GoToScene()
    {
        SceneManager.LoadScene(sceneIdentifier); 
    }

    // Load scene - Additive
    private void AddScene()
    {
        SceneManager.LoadScene(sceneIdentifier, LoadSceneMode.Additive);
    }

    // Unload additive scene
    private void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(sceneIdentifier);
    }

    private string SceneToString(SceneName sceneName)
    {
        switch (sceneName)
        {
            case SceneName.Homepage:
                return "Homepage";
            case SceneName.LevelSelection:
                return "LevelSelection";
            case SceneName.LSModal:
                return "LS-Modal";
            case SceneName.GameScene:
                return "GameScene";
            case SceneName.GSPianoScale:
                return "GS-PianoScale";
            case SceneName.GSPause:
                return "GS-Pause";
            case SceneName.ResultPage:
                return "ResultPage";
            case SceneName.Settings:
                return "Settings";
            case SceneName.Achievements:
                return "Achievements";
            case SceneName.CharacterSelection:
                return "CharacterSelect";
        }

        return null;

    }
}