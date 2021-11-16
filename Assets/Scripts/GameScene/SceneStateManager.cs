using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;

// TODO: - RENAME DI DEVELOPMENT
public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager Instance;

    [SerializeField]
    private SceneState sceneState = SceneState.Instruction;

    [Space]

    [SerializeField]
    private GameObject[] instructionObjects = null;

    [SerializeField]
    private GameObject[] countdownObjects = null;

    [SerializeField]
    private GameObject[] gameplayObjects = null;

    [SerializeField]
    private GameObject[] practiceObjects = null;

    [SerializeField]
    private GameObject[] onboardingObjects = null;

    [SerializeField]
    private GameObject[] noteBar = null;

    [Header("Button")]
    [SerializeField]
    private Button actionButton = null;

    [SerializeField]
    private Button repeatButton = null;

    [SerializeField]
    private Button practiceButton = null;

    [Header("Others")]
    [SerializeField]
    private GameObject overlay = null;

    [SerializeField]
    private AudioMixer audioMixer = null;

    [SerializeField]
    private GameObject comboObject = null;

    [SerializeField]
    private GameObject accuracyObject = null;

    private GameObject[] allParents = null;

    private GameObject titleButton;
    private GameObject countButton;
    private TMPro.TextMeshProUGUI title;
    private TMPro.TextMeshProUGUI countdown;

    private SceneManagerScript sceneManager;

    private float delay = 1;
    private int onboardingSteps = 0;
    private bool practice = false;
    private bool instructionEnd = false;

    void Start()
    {
        Instance = this;
        Initialize();
        CheckSceneState();
    }

    void Update()
    {
        if (sceneState == SceneState.Onboarding && SongManager.Instance.IsAudioFinished() && onboardingSteps == 8)
        {
            SetOnboarding();
        }
    }

    void Initialize()
    {
        sceneManager = GetComponent<SceneManagerScript>();
        // Initialize title & countdown object
        // Title is static, countdown is dynamic
        titleButton = countdownObjects[1];
        countButton = countdownObjects[0];
        title = countdownObjects[1].GetComponentInChildren<TMPro.TextMeshProUGUI>();
        countdown = countdownObjects[0].GetComponentInChildren<TMPro.TextMeshProUGUI>();

        var onboardingParent = onboardingObjects[0].transform.parent.gameObject;
        var pianoScaleParent = instructionObjects[0].transform.parent.gameObject;

        allParents = new GameObject[2] { onboardingParent, pianoScaleParent };

    }

    public SceneState GetSceneState()
    {
        return sceneState;
    }

    public void ChangeSceneState(SceneState scene)
    {
        sceneState = scene;
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
        var array = instructionObjects.Concat(countdownObjects).Concat(gameplayObjects).Concat(onboardingObjects).ToArray();

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
                SetActiveInactive(instructionObjects ,true);
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

    // At the moment, we use dspTime to control spawned notes and notes position.
    // Hence, all we need to do to pause/resume the game is just pausing/resuming the song.
    // Since we use audio playback reference to spawn our game object.
    public void PauseGame()
    {
        sceneState = SceneState.Pause;
        overlay.SetActive(true);

        SongManager.Instance.PauseSong();
    }

    public void ResumeGame()
    {
        sceneState = SceneState.Countdown;
        overlay.SetActive(false);
        var pauseDelay = 3;
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "backsoundVolume", pauseDelay, FadeMixerGroup.Fade.In));

        SongManager.Instance.ResumeSong();
    }

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
        }
        else
        {
            PlayerPrefs.SetInt("IsFirstTime", 1);
            sceneManager.SceneInvoke("Homepage");
        }
    }

    // Set Onboarding objects on each state
    public void SetOnboardingObjects()
    {
        var button = actionButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        if (onboardingSteps == 1)
            button.SetText("Sure!");

        if (onboardingSteps == 2)
            button.SetText("...");

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
            actionButton.interactable = false;
            repeatButton.gameObject.SetActive(true);
            AnimationUtilities.Instance.MoveY(repeatButton.gameObject, 0.1f, 0f, true);
        }

        if (onboardingSteps == 9)
        {
            actionButton.interactable = true;
            repeatButton.gameObject.SetActive(false);
            StartCoroutine(AnimationUtilities.Instance.AnimateObjects(noteBar, 0.2f, AnimationUtilities.AnimationType.MoveY, 5f, 0f));
        }

        if (onboardingSteps == onboardingObjects.Length - 1)
            button.SetText("Finish");

    }

    void InstructionStart()
    {
        StartCoroutine(AnimationUtilities.Instance.AnimateObjects(instructionObjects, 0.1f, AnimationUtilities.AnimationType.MoveY, 0f, 10f));
    }

    public void InstructionEnd()
    {
        if (instructionObjects.Length > 0 && instructionEnd == false)
            instructionObjects[0].transform.parent.GetComponent<Image>().CrossFadeAlpha(0f, 0.5f, false);

        instructionEnd = true;
    }

    IEnumerator PracticeStart()
    {
        InstructionEnd();

        comboObject.SetActive(false);
        accuracyObject.SetActive(false);

        yield return new WaitForSeconds(0);

        practiceButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText("Play Mode");
        StartCoroutine(AnimationUtilities.Instance.AnimateObjects(practiceObjects, 0.2f, AnimationUtilities.AnimationType.MoveY, 0f, 5f));

        if (SongManager.Instance.GetSongPlayed())
        {
            SongManager.Instance.PauseSong();
        }
        else
        {
            yield return new WaitForSeconds(delay);
            SongManager.Instance.StartSong();
        }
    }

    // Countdown to Gameplay transition.
    // The code is pretty self explanatory since it's a hardcoded & sequential one.
    // Basically telling the sequence of animation that need to be played in transition.
    IEnumerator CountdownStart()
    {
        InstructionEnd();

        StartCoroutine(AnimationUtilities.Instance.AnimateObjects(countdownObjects, 0.1f, AnimationUtilities.AnimationType.MoveY, 0f, 5f));

        int count = 3;
        while (count > 0)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(AnimationUtilities.Instance.AnimateObjects(countdownObjects, 0.2f, AnimationUtilities.AnimationType.PunchScale, 0f, 0f));
            countdown.SetText(count.ToString());
            count --;
        }

        yield return new WaitForSeconds(delay);

        titleButton.SetActive(false);
        AnimationUtilities.Instance.PunchScale(countButton);
        countdown.SetText("Go!");

        yield return new WaitForSeconds(delay);

        StartCoroutine(AnimationUtilities.Instance.AnimateObjects(gameplayObjects, 0.1f, AnimationUtilities.AnimationType.MoveY, 0f, 5f));
        countButton.SetActive(false);
        countdown.SetText("3");

        yield return new WaitForSeconds(2);

        SongManager.Instance.StartSong();
    }

    public IEnumerator EndOfSongAnimation()
    {
        yield return StartCoroutine(AnimationUtilities.Instance.AnimateObjects(gameplayObjects, 0.1f, AnimationUtilities.AnimationType.MoveY, 5f, 0f));
        sceneManager.SceneInvoke("ResultPage");
    }

    public void TogglePractice()
    {
        if (!Instance.practice)
        {
            Instance.practice = true;
            ChangeSceneState(SceneState.Practice);
        }
        else
        {
            sceneManager.SceneInvoke("GameScene");
        } 
    }

    public enum SceneState
    {
        Onboarding,
        Instruction,
        Practice,
        Countdown,
        Pause,
        EndOfSong
    }
}

