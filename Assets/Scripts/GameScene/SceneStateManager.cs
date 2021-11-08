using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager Instance;

    [SerializeField]
    private AudioSource[] audioSources = null;

    [SerializeReference]
    private GameObject[] instructionObjects = null;

    [SerializeReference]
    private GameObject[] countdownObjects = null;

    [SerializeReference]
    private GameObject[] gameplayObjects = null;

    [SerializeReference]
    private GameObject overlay = null;

    [SerializeReference]
    private GameObject pauseButton = null;

    [SerializeReference]
    private AudioMixer audioMixer = null;



    private GameObject comboIndicator;
    private GameObject titleButton;
    private GameObject countButton;
    private TMPro.TextMeshProUGUI title;
    private TMPro.TextMeshProUGUI countdown;

    //SceneState sceneState = SceneState.Instruction;
    SceneState sceneState = SceneState.Countdown;

    float delay = 1;

    void Start()
    {
        Instance = this;
        Initialize();
        CheckSceneState();
    }

    void Update()
    {
    }

    void Initialize()
    {
        // Get component named Combo, we need it to hide/show the object based on the screen state
        comboIndicator = gameplayObjects[0].transform.Find("Combo").gameObject;

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

    void CheckSceneState()
    {
        // Loop through the animated object list, and inactive them
        // Later, the inactived objects will show up based on the current scene state
        var array = instructionObjects.Concat(countdownObjects).Concat(gameplayObjects).ToArray();
        

        switch (sceneState)
        {
            case SceneState.Instruction:
                InstructionStart();
                break;
            case SceneState.Countdown:
                SetInactives(array);
                StartCoroutine(CountdownStart());
                break;
            case SceneState.EndOfSong:
                StartCoroutine(EndOfSongAnimation());
                break;

        }
    }

    // Set all objects to inactive.
    // This is needed, to reset all objects on the scene before transition to the next scene state begin.
    void SetInactives(GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(false);
        }
    }

    // At the moment, we use dspTime to control spawned notes and notes position.
    // Hence, all we need to do to pause/resume the game is just pausing/resuming the song.
    // Since we use audio playback reference to spawn our game object.
    public void PauseGame()
    {
        sceneState = SceneState.Pause;
        overlay.SetActive(true);


        foreach (var audio in audioSources)
        {
            audio.Pause();
        }
    }

    public void ResumeGame()
    {
        sceneState = SceneState.Countdown;
        overlay.SetActive(false);
        var pauseDelay = 3;
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "backsoundVolume", pauseDelay, FadeMixerGroup.Fade.In));

        foreach (var audio in audioSources)
        {
            var currentTime = audio.time;
            audio.time = currentTime - pauseDelay;
            audio.PlayScheduled(0);
        }
    }

    void InstructionStart()
    {
        comboIndicator.SetActive(false);
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
        comboIndicator.SetActive(true);
        countButton.SetActive(false);
        countdown.SetText("3");

        yield return new WaitForSeconds(2);

        SongManager.Instance.StartSong();
    }

    public IEnumerator EndOfSongAnimation()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(AnimateObjects(gameplayObjects, 0.1f, AnimationType.MoveY, 5f, 0f));

    }

    // Animate group of objects based on the given parameter (duration & animationType)
    IEnumerator AnimateObjects(GameObject[] objects, float duration, AnimationType type, float target, float from)
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

    // DoTween Animation
    void PunchScale(GameObject obj)
    {
        obj.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 0.25f), 0.2f, 1, 1);
    }

    void MoveY(GameObject obj, float target, float from)
    {
        var post = obj.transform.position;
        obj.transform.DOMoveY(post.y + target, 0.75f).SetEase(Ease.InOutQuad).From(post.y + from);
    }


    // Enumeration
    enum AnimationType
    {
        PunchScale,
        MoveY
    }

    public enum SceneState
    {
        Instruction,
        Countdown,
        Pause,
        EndOfSong
    }
}

