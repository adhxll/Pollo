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
    private GameObject[] instructionObjects = null;

    [SerializeField]
    private GameObject[] countdownObjects = null;

    [SerializeField]
    private GameObject[] gameplayObjects = null;

    [SerializeField]
    private GameObject[] practiceObjects = null;

    [Header("Onboarding")]
    [SerializeField]
    private GameObject[] onboardingObjects = null;

    [SerializeField]
    private GameObject[] noteBar = null;

    [SerializeField]
    private Button actionButton = null;

    [SerializeField]
    private Button repeatButton = null;

    [SerializeField]
    private GameObject scoreObj = null;

    [SerializeField]
    private GameObject overlay = null;

    [SerializeField]
    private AudioMixer audioMixer = null;

    [Header("Practice Mode")]
    [SerializeField]
    private GameObject practiceObject = null;

    [SerializeField]
    private GameObject scoreObject = null;

    private GameObject titleButton;
    private GameObject countButton;
    private TMPro.TextMeshProUGUI title;
    private TMPro.TextMeshProUGUI countdown;

    //SceneState sceneState = SceneState.Onboarding;
    private SceneState sceneState = SceneState.Instruction;
    private SceneManagerScript sceneManager;

    private float delay = 1;
    private int onboardingSteps = 0;
    private bool practice = false;

    void Start()
    {
        Instance = this;
        Initialize();
        CheckSceneState();
    }

    void Update()
    {
        if (sceneState == SceneState.Onboarding && SongManager.Instance.IsAudioFinished())
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
        var array = instructionObjects.Concat(countdownObjects).Concat(gameplayObjects).ToArray();

        switch (sceneState)
        {
            case SceneState.Onboarding:
                SetActiveInactive(array, false);
                StartOnboarding();
                break;
            case SceneState.Instruction:
                SetActiveInactive(instructionObjects ,true);
                InstructionStart();
                break;
            case SceneState.Countdown:
                SetActiveInactive(array, false);
                StartCoroutine(CountdownStart());
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
        MoveY(actionButton.gameObject, 0.1f, 0f, true);
    }

    // Set Onboarding states
    public void SetOnboarding()
    {
        if (onboardingSteps < onboardingObjects.Length)
        {
            onboardingObjects[onboardingSteps].SetActive(false);

            if (onboardingSteps < onboardingObjects.Length - 1)
                onboardingObjects[onboardingSteps + 1].SetActive(true);

            onboardingSteps++;
            SetOnboardingObjects();
        }
    }

    // Set Onboarding objects on each state
    public void SetOnboardingObjects()
    {
        var button = actionButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        if (onboardingSteps == 1)
            button.SetText("Hmm, sure.");

        if (onboardingSteps == 2)
            button.SetText("Okay!");

        // Showing musical bar
        if (onboardingSteps == 3)
        {
            button.SetText("Next");
            SetActiveInactive(noteBar, false);
            StartCoroutine(AnimateObjects(noteBar, 0.2f, AnimationType.MoveY, 0f, 5f));
        }

        if (onboardingSteps == 6)
        {
            SongManager.Instance.StartSong();
            actionButton.interactable = false;
            repeatButton.gameObject.SetActive(true);
            MoveY(repeatButton.gameObject, 0.1f, 0f, true);
        }

        if (onboardingSteps == 7)
        {
            actionButton.interactable = true;
            repeatButton.gameObject.SetActive(false);
            StartCoroutine(AnimateObjects(noteBar, 0.2f, AnimationType.MoveY, 5f, 0f));
        }

        if (onboardingSteps == onboardingObjects.Length - 1)
            button.SetText("Finish");

        if (onboardingSteps == onboardingObjects.Length)
            Debug.Log("STAGE FINISHED");
    }

    void InstructionStart()
    {
        StartCoroutine(AnimateObjects(instructionObjects, 0.1f, AnimationType.MoveY, 0f, 5f));
    }

    // Countdown to Gameplay transition.
    // The code is pretty self explanatory since it's a hardcoded & sequential one.
    // Basically telling the sequence of animation that need to be played in transition.
    IEnumerator CountdownStart()
    {
        StartCoroutine(AnimateObjects(countdownObjects, 0.1f, AnimationType.MoveY, 0f, 5f));

        int count = 3;
        while (count > 0)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(AnimateObjects(countdownObjects, 0.2f, AnimationType.PunchScale, 0f, 0f));
            countdown.SetText(count.ToString());
            count --;
        }

        yield return new WaitForSeconds(delay);

        titleButton.SetActive(false);
        PunchScale(countButton);
        countdown.SetText("Go!");

        yield return new WaitForSeconds(delay);

        StartCoroutine(AnimateObjects(gameplayObjects, 0.1f, AnimationType.MoveY, 0f, 5f));
        countButton.SetActive(false);
        countdown.SetText("3");

        yield return new WaitForSeconds(2);

        SongManager.Instance.StartSong();
    }

    public IEnumerator EndOfSongAnimation()
    {
        yield return StartCoroutine(AnimateObjects(gameplayObjects, 0.1f, AnimationType.MoveY, 5f, 0f));
        sceneManager.SceneInvoke("ResultPage");
    }

    // Animate group of objects based on the given parameter (duration & animationType)
    public IEnumerator AnimateObjects(GameObject[] objects, float duration, AnimationType type, float target, float from)
    {
        foreach(var obj in objects)
        {
            // Set parent to active if it's inactive
            if (obj.transform.parent != null && !obj.transform.parent.gameObject.activeSelf)
            {
                var parentObj = obj.transform.parent.gameObject;
                parentObj.SetActive(true);
            }

            obj.SetActive(true);

            switch (type) {
                case AnimationType.MoveY:
                    MoveY(obj, target, from);
                    break;
                case AnimationType.PunchScale:
                    PunchScale(obj);
                    break;
            }
            yield return new WaitForSeconds(duration);
        }
    }

    // TODO - Move this to animation utilities
    // DoTween Animation
    void PunchScale(GameObject obj)
    {
        obj.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 0.25f), 0.2f, 1, 1);
    }

    void MoveY(GameObject obj, float target, float from, bool loop = false)
    {
        var post = obj.transform.position;
        obj.SetActive(true);
        if (loop)
            obj.transform.DOMoveY(post.y + target, 0.75f).SetEase(Ease.InOutQuad).From(post.y + from).SetLoops(-1, LoopType.Yoyo);

        else
            obj.transform.DOMoveY(post.y + target, 0.75f).SetEase(Ease.InOutQuad).From(post.y + from);
    }

    public void TogglePractice()
    {
        if (!Instance.practice)
        {
            practiceObject.SetActive(true);
            scoreObject.SetActive(false);
            Instance.practice = true;
        }
        else
        {
            practiceObject.SetActive(false);
            scoreObject.SetActive(true);
            Instance.practice = false;
        } 
    }

    // Enumeration
    public enum AnimationType
    {
        PunchScale,
        MoveY
    }

    [SerializeField]
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

