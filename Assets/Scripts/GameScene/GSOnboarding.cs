using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GSOnboarding : MonoBehaviour
{
    // Onboarding messages
    [SerializeField] private GameObject[] onboardingObjects = null;

    // Onboarding action button
    [SerializeField] private Button actionButton = null;
    private TMPro.TextMeshProUGUI actionButtonText = null;

    private string[] buttonText = { "Okay", "Sure!", "...", "Okay!", "I'm ready!", "Next", "Finish"};

    private int onboardingSteps = 0;
    private bool isOnboardingGameStarted = false;

    void Start()
    {
        
    }

    void Update()
    {
        CheckOnboarding();
    }

    public void StartOnboarding()
    {
        if (onboardingObjects.Length > 0)
        {
            actionButtonText = actionButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();

            // Activate the root (parent) of the first onboarding object
            onboardingObjects.First().transform.parent.gameObject.SetActive(true);

            // Activate the first onboarding object
            onboardingObjects.First().SetActive(true);
        }

        // Apply bouncy animation on the action button
        AnimationUtilities.Instance.MoveY(actionButton.gameObject, 0.1f, 0f, true);

    }

    // Set onboarding states
    private void SetOnboarding()
    {
        if (onboardingSteps < onboardingObjects.Length - 1)
        {
            // Hide current object and show the next object
            onboardingObjects[onboardingSteps].SetActive(false);
            onboardingObjects[onboardingSteps + 1].SetActive(true);

            if (onboardingSteps <= 5)
            {
                // Set button text according to onboarding steps
                actionButtonText.SetText(buttonText[onboardingSteps]);
            }
        } else
        {
            // Player finished the onboarding
            // TODO: Create enum for "isFirstTime" PlayerPrefs Key
            PlayerPrefs.SetInt("isFirstTime", 1);
            SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.Homepage);
        }
    }

    // Set each onboarding objects according to the current step
    private void SetOnboardingObjects()
    {
        // Request microphone
        if (onboardingSteps == 3)
            RequestMicrophoneAccess();

        // Show musical bar
        if (onboardingSteps == 5)
        {

        }

        if (onboardingSteps == 8)
        {
            SongManager.Instance.StartSong();
            isOnboardingGameStarted = true;
            actionButton.interactable = false;
        }

        if (onboardingSteps == 9)
        {
            isOnboardingGameStarted = false;
            actionButton.interactable = true;
        }
    }

    private void CheckOnboarding()
    {
        // TODO: Create enum for "isFirstTime" PlayerPrefs Key
        var isOnboarding = PlayerPrefs.GetInt("isFirstTime");

        if (isOnboarding == 0 && SongManager.Instance.IsAudioFinished())
        {
            SetOnboarding();
        }
    }

    private void RequestMicrophoneAccess()
    {
        // Trigger microphone access by starting the device microphone for 1 sec, then stop it immediately
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, false, 1, AudioSettings.outputSampleRate);
        audioSource.Play();
        audioSource.Stop();
    }
}
