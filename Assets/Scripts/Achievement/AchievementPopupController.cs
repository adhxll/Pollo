using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementPopupController : MonoBehaviour //buat munculin scene achievement di scene yg diinginkan
{
    
    public static AchievementPopupController Instance;
    public List<int> AchievementList = new List<int>(); //achievement queue

    public void LoadAchievementPopup() //call this function buat manggil id achievement yg mau dipanggil
    {
        if (AchievementList.Count > 0)
        {
            StartCoroutine(ShowPopup());
        }
    }

    private IEnumerator ShowPopup() //kalo achievementnya ke trigger lebih dari satu, settingan munculnya
    {
        for (int i = 0; i < AchievementList.Count; i++)
        {
            AchievementPopup.SetAchievementId(AchievementList[i]);
            SceneManager.LoadScene("AchievementNotif", LoadSceneMode.Additive);
            UnloadAchievementPopup();
            yield return new WaitForSeconds(3f);
        }

        StopCoroutine(ShowPopup());
    }

    public void UnloadAchievementPopup()
    {
        StartCoroutine(RemoveScene(5f));
    }

    private IEnumerator RemoveScene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.UnloadScene("AchievementNotif");
        StopCoroutine(RemoveScene(3f));
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
