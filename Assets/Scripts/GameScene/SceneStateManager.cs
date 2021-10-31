using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager Instance;

    [SerializeReference]
    private GameObject[] instructionObjects = null;

    [SerializeReference]
    private GameObject[] countdownObjects = null;

    [SerializeReference]
    private GameObject[] gameplayObjects = null;

    [SerializeReference]
    private GameObject overlay = null;

    private GameObject titleButton;
    private GameObject countButton;
    private TMPro.TextMeshProUGUI title;
    private TMPro.TextMeshProUGUI countdown;

    SceneState sceneState = SceneState.Instruction;

    float delay = 1;

    private void Awake()
    {
        titleButton = countdownObjects[1];
        countButton = countdownObjects[0];
        title = countdownObjects[1].GetComponentInChildren<TMPro.TextMeshProUGUI>();
        countdown = countdownObjects[0].GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    void Start()
    {
        Instance = this;
        CheckSceneState();
    }

    void Update()
    {
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
        var array = instructionObjects.Concat(countdownObjects).Concat(gameplayObjects).ToArray();
        SetInactives(array);

        switch (sceneState)
        {
            case SceneState.Instruction:
                InstructionStart();
                break;
            case SceneState.Countdown:
                StartCoroutine(CountdownStart());
                break;
            case SceneState.Gameplay:
                overlay.SetActive(true);
                break;
        }
    }

    void SetInactives(GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(false);
        }
    }

    void InstructionStart()
    {
        StartCoroutine(AnimateObjects(instructionObjects, 0.1f, AnimationType.MoveY));
    }

    // Countdown to Gameplay transition
    IEnumerator CountdownStart()
    {
        StartCoroutine(AnimateObjects(countdownObjects, 0.1f, AnimationType.MoveY));

        int count = 3;
        while (count > 0)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(AnimateObjects(countdownObjects, 0.2f, AnimationType.PunchScale));
            countdown.SetText(count.ToString());
            count --;
        }

        yield return new WaitForSeconds(delay);

        titleButton.SetActive(false);
        PunchScale(countButton);
        countdown.SetText("Go!");

        yield return new WaitForSeconds(delay);

        StartCoroutine(AnimateObjects(gameplayObjects, 0.1f, AnimationType.MoveY));
        countButton.SetActive(false);
        countdown.SetText("3");

        yield return new WaitForSeconds(2);

        SongManager.Instance.StartSong();
    }

    IEnumerator AnimateObjects(GameObject[] objects, float duration, AnimationType type)
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
                    MoveY(obj);
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

    void MoveY(GameObject obj)
    {
        var post = obj.transform.position;
        obj.transform.DOMoveY(post.y, 0.75f).SetEase(Ease.InOutQuad).From(post.y + 5f);
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
        Gameplay
    }
}

