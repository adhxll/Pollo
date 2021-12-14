using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementPopupController : MonoBehaviour //buat munculin scene achievement di scene yg diinginkan
{
    public static AchievementPopupController Instance;
    public List<int> achievementList = new List<int>(); //achievement queue

    public void LoadAchievementPopup() //call this function buat manggil id achievement yg mau dipanggil
    {
        if (achievementList.Count > 0)
        {
            StartCoroutine(ShowPopup());
        }
    }

    private IEnumerator ShowPopup() //kalo achievementnya ke trigger lebih dari satu, settingan munculnya
    {
        for (int i = 0; i < achievementList.Count; i++)
        {
            AchievementPopup.SetAchievementId(achievementList[i]);
            SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.AchievementNotif, true);
            UnloadAchievementPopup();
            yield return new WaitForSeconds(3f);
        }

        StopCoroutine(ShowPopup());
    }

    public void UnloadAchievementPopup()
    {
        try
        {
            StartCoroutine(RemoveScene(5f));
        }
        catch
        {
            Debug.Log("Popup unload cancelled because it has already been destroyed.");
        }
    }

    private IEnumerator RemoveScene(float time)
    {
        yield return new WaitForSeconds(time);
        if (SceneManager.GetActiveScene().name == "AchievementNotif")
            SceneManagerScript.Instance.SceneUnload(SceneManagerScript.SceneName.AchievementNotif);
        StopCoroutine(RemoveScene(0f));
    }

    private void StartSingleton()
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
    void Awake()
    {
        StartSingleton();
    }
}
