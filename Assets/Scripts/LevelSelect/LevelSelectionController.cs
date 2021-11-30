using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class LevelSelectionController : MonoBehaviour
{
    public static LevelSelectionController Instance; 
    public Canvas levelCanvas;
    private bool modalShown;
    public GameObject PrevStageBtn;
    public GameObject NextStageBtn;
    public bool enablePrevStageBtn, enableNextStageBtn;
    private void Awake()
    {
        StartCustomSingleton(); 
    }
    public void ModifyLevelInput() { //disables level button input when modal is shown
        if (!modalShown) 
        {
            levelCanvas.GetComponent<GraphicRaycaster>().enabled = false;
            modalShown = true; 
        }
        else
        {
            levelCanvas.GetComponent<GraphicRaycaster>().enabled = true;
            modalShown = false; 
        }
    }
    public void SaveGame() {
        SaveSystem.SavePlayerData(); 
    }
    private void StartCustomSingleton() {
        // used to keep this script as a static singleton, but only within the level selection scene
        // needed for supporting multiple island prefabs, see LevelItem for example
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
    public void ModifyNextStageButton() {
        if (!enableNextStageBtn)
        {
            enableNextStageBtn = true;
            NextStageBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else {
            enableNextStageBtn = false;
            NextStageBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
    }
    public void ModifyPrevStageButton()
    {
        if (!enablePrevStageBtn)
        {
            enablePrevStageBtn = true;
            PrevStageBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            enablePrevStageBtn = false;
            PrevStageBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
    }
}
