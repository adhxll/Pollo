using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementPopupController : MonoBehaviour //buat munculin scene achievement di scene yg diinginkan
{
    
    public static AchievementPopupController Instance;

    public void LoadAchievementPopup(int achievementId) //call this function buat manggil id achievement yg mau dipanggil
    {
        AchievementPopup.SetAchievementId(achievementId);
        SceneManager.LoadScene("AchievementNotif", LoadSceneMode.Additive);
        UnloadAchievementPopup();
    }

    public void UnloadAchievementPopup()
    {
        StartCoroutine(RemoveScene(3f));
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Awake()
    {
        StartSingleton();
    }
}
