using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

// TODO: - RENAME DI DEVELOPMENT
public class SceneStateManager : MonoBehaviour
{
    public enum SceneState
    {
        Onboarding,
        Instruction,
        Practice,
        Countdown,
        Pause,
        EndOfSong
    }

    public static SceneStateManager Instance;

    [SerializeField] private SceneState sceneState = SceneState.Instruction;
    [Space]
    [SerializeField] private GameObject[] countdownObjects = null;
    [SerializeField] private GameObject[] gameplayObjects = null;
    [SerializeField] private GameObject[] practiceObjects = null;
    [SerializeField] private GameObject[] onboardingObjects = null;
    [SerializeField] private GameObject[] noteBar = null;

    [Header("Button")]
    [SerializeField] private Button actionButton = null;
    [SerializeField] private Button repeatButton = null;
    [SerializeField] private Button practiceButton = null;

    [Header("Others")]
    [SerializeField] private GameObject comboObject = null;
    [SerializeField] private GameObject accuracyObject = null;

    private GameObject[] allParents = null;

    private GameObject titleButton;
    private GameObject countButton;
    private TMPro.TextMeshProUGUI title;
    private TMPro.TextMeshProUGUI countdown;

    private float delay = 1;
    private int onboardingSteps = 0;
    private bool isOnboardingGameStarted = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize();
        CheckSceneState();
    }

    void Update()
    {
        CheckOnboarding();
    }

    void Initialize()
    {
        // Initialize title & countdown object
        // Title is static, countdown is dynamic
        titleButton = countdownObjects[1];
        countButton = countdownObjects[0];
        title = countdownObjects[1].GetComponentInChildren<TMPro.TextMeshProUGUI>();
        countdown = countdownObjects[0].GetComponentInChildren<TMPro.TextMeshProUGUI>();

        var onboardingParent = onboardingObjects[0].transform.parent.gameObject;

        allParents = new GameObject[1] { onboardingParent };

        if (PlayerPrefs.GetInt("IsFirstTime") == 1 && GameController.Instance != null)
            sceneState = GameController.Instance.sceneState;

    }

    #region General Function

    public SceneState GetSceneState()
    {
        return sceneState;
    }

    public void ChangeSceneState(SceneState scene, bool reload = true)
    {
        sceneState = scene;

        if (reload)
            CheckSceneState();
    }

    public void ChangeSceneState(string scene)
    {
        sceneState = (SceneState) System.Enum.Parse (typeof(SceneState), scene);
        CheckSceneState();
    }

    void CheckSceneState()
    {
        // Loop through the animated object list, and inactive them
        // Later, the inactived objects will show up based on the current scene state
        var array = countdownObjects.Concat(gameplayObjects).Concat(onboardingObjects).ToArray();

        switch (sceneState)
        {
            case SceneState.Onboarding:
                SetActiveInactive(array, false);
                SetActiveInactive(allParents, false);
                StartOnboarding();
                break;
            case SceneState.Instruction:
                SetActiveInactive(array, false);
                SetActiveInactive(allParents, false);
                InstructionStart();
                break;
            case SceneState.Countdown:
                SetActiveInactive(array, false);
                StartCoroutine(CountdownStart());
                break;
            case SceneState.Practice:
                SetActiveInactive(array, false);
                StartCoroutine(PracticeStart());
                break;
            case SceneState.EndOfSong:
                StartCoroutine(EndOfSongAnimation());
                break;
        }
    }

    // Set all objects to inactive.
    // This is needed, to reset all objects on the scene before transition to the next scene state begin.
    void SetActiveInactive(GameObject[] objects, bool tf)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(tf);
        }
    }

    #endregion

    #region Onboarding

    private void StartOnboarding()
    {
        if (onboardingObjects.Length > 0)
        {
            onboardingObjects[0].transform.parent.gameObject.SetActive(true);
            onboardingObjects[0].SetActive(true);
        }

        AnimationUtilities.Instance.MoveY(actionButton.gameObject, 0.1f, 0f, true);
    }

    // Set Onboarding states
    public void SetOnboarding()
    {
        if (onboardingSteps < onboardingObjects.Length - 1)
        {
            onboardingObjects[onboardingSteps].SetActive(false);
            onboardingObjects[onboardingSteps + 1].SetActive(true);

            onboardingSteps++;
            SetOnboardingObjects();
        } else { // if the user finishes onboarding
            PlayerPrefs.SetInt("IsFirstTime", 1); // isFirstTime == False 
            SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.Homepage);
            ReportTutorialComplete(); // analytics stuffs
        }
    }

    // Set Onboarding objects on each state
    public void SetOnboardingObjects()
    {
        var button = actionButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        if (onboardingSteps == 1)
            button.SetText("Sure!");

        if (onboardingSteps == 2)
        {
            ReportFirstInteraction();

            button.SetText("...");
        }
        if (onboardingSteps == 3)
        {
            button.SetText("Okay!");

            // Request Microphone
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = Microphone.Start(null, false, 1, AudioSettings.outputSampleRate);
            audioSource.Play();
            audioSource.Stop();
        }

        if (onboardingSteps == 4)
            button.SetText("I'm ready!");

        // Showing musical bar
        if (onboardingSteps == 5)
        {
            button.SetText("Next");
            SetActiveInactive(noteBar, false);
            StartCoroutine(AnimationUtilities.Instance.AnimateObjects(noteBar, 0.2f, AnimationUtilities.AnimationType.MoveY, 0f, 5f));
        }

        if (onboardingSteps == 8)
        {
            SongManager.Instance.StartSong();
            isOnboardingGameStarted = true;
            actionButton.interactable = false;
            repeatButton.gameObject.SetActive(true);
            AnimationUtilities.Instance.MoveY(repeatButton.gameObject, 0.1f, 0f, true);
        }

        if (onboardingSteps == 9)
        {
            isOnboardingGameStarted = false;
            actionButton.interactable = true;
            repeatButton.gameObject.SetActive(false);
            StartCoroutine(AnimationUtilities.Instance.AnimateObjects(noteBar, 0.2f, AnimationUtilities.AnimationType.MoveY, 5f, 0f));
        }

        if (onboardingSteps == onboardingObjects.Length - 1)
            button.SetText("Finish");

    }

    // Check whether onboarding gameplay is finished
    void CheckOnboarding()
    {
        if (sceneState == SceneState.Onboarding && SongManager.Instance.IsAudioFinished() && isOnboardingGameStarted)
        {
            SetOnboarding();
        }
    }

    #endregion

    void InstructionStart()
    {
        scaleInstructionStart = Time.time;
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GSPianoScale, true);
    }

    IEnumerator PracticeStart()
    {
        ReportLevelPracticed(); // sends analytics if the user is choosing to practice a level
        comboObject.SetActive(false);
        accuracyObject.SetActive(false);

        yield return new WaitForSeconds(0);

        practiceButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText("Play Mode");
        StartCoroutine(AnimationUtilities.Instance.AnimateObjects(practiceObjects, 0.2f, AnimationUtilities.AnimationType.MoveY, 0f, 5f));

        if (SongManager.Instance.GetSongPlayed())
        {
            SongManager.Instance.PauseSong();
        } else {
            yield return new WaitForSeconds(delay);
            SongManager.Instance.StartSong();
        }
    }

    // Countdown to Gameplay transition.
    // The code is pretty self explanatory since it's a hardcoded & sequential one.
    // Basically telling the sequence of animation that need to be played in transition.
    IEnumerator CountdownStart()
    {
        // send analytics report if a user has started a normal level
        ReportLevelStarted();
        ReportTimeOnInstruction();
        StartCoroutine(AnimationUtilities.Instance.AnimateObjects(countdownObjects, 0.1f, AnimationUtilities.AnimationType.MoveY, 0f, 5f));

        int count = 3;
        while (count > 0)
        {
            yield return new WaitForSeconds(delay);
            AudioController.Instance.PlaySound(SoundNames.countdown);
            StartCoroutine(AnimationUtilities.Instance.AnimateObjects(countdownObjects, 0.2f, AnimationUtilities.AnimationType.PunchScale, 0f, 0f));

            countdown.SetText(count.ToString());
            count --;
        }

        yield return new WaitForSeconds(delay);

        titleButton.SetActive(false);
        AnimationUtilities.Instance.PunchScale(countButton);
        AudioController.Instance.PlaySound(SoundNames.countdown);
        countdown.SetText("Go!");

        yield return new WaitForSeconds(delay);

        StartCoroutine(AnimationUtilities.Instance.AnimateObjects(gameplayObjects, 0.1f, AnimationUtilities.AnimationType.MoveY, 0f, 5f));
        SongManager.Instance.StartSong();

        countButton.SetActive(false);
        countdown.SetText("3");

    }

    public IEnumerator EndOfSongAnimation()
    {
        yield return StartCoroutine(AnimationUtilities.Instance.AnimateObjects(gameplayObjects, 0.1f, AnimationUtilities.AnimationType.MoveY, 5f, 0f));
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.ResultPage);
        // send analytics if the user finishes a normal level
        ReportLevelFinished();
    }

    public void TogglePractice()
    {
        var practiceText = practiceButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        if (practiceText == "Practice Mode")
        {
            ChangeSceneState(SceneState.Practice);
        } else {
            GameController.Instance.sceneState = SceneState.Instruction;
            SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GameScene);
        } 
    }

    #region Analytics Functions

    private float scaleInstructionStart = 0.0f;

    Dictionary<string, object> GetLevelParameters()
    {
        Dictionary<string, object> customParams = new Dictionary<string, object>();
        customParams.Add("stage", GameController.Instance.currentStage);
        customParams.Add("level", GameController.Instance.selectedLevel);

        return customParams;
    }

    void ReportFirstInteraction()
    {
        // Tracks whether the user did the first step on onboarding or not
        // This event is actually available as a standard event, but for some reason I can't load the AnalyticsEvent class.
        var analytics = Analytics.CustomEvent("FirstInteractionTrue");
        //Debug.Log("ReportFirstInteraction: "+ analytics);
    }

    void ReportTutorialComplete()
    {
        var analytics = Analytics.CustomEvent("TutorialCompleteTrue");
        //Debug.Log("ReportTutorialComplete: " + analytics);
    }

    void ReportLevelStarted()
    {
        var analytics = Analytics.CustomEvent("LevelStarted" , GetLevelParameters());
        //Debug.Log("Level Started: " + analytics);
    }

    void ReportLevelFinished()
    {
       var analytics = Analytics.CustomEvent("LevelFinished" , GetLevelParameters());
        //Debug.Log("Level Finished: " + analytics);
    }

    void ReportLevelPracticed()
    {
        var analytics = Analytics.CustomEvent("LevelPracticed" , GetLevelParameters());
        //Debug.Log("Level Practiced: " + analytics);
    }

    void ReportTimeOnInstruction()
    {
        Dictionary<string, object> customParams = new Dictionary<string, object>();
        customParams.Add("timePassed", Time.time - scaleInstructionStart);
        var analytics = Analytics.CustomEvent("TimeSpentOnScaleInstruction" , customParams);
    }

    #endregion
}

