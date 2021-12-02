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
    public void ModifyChangeStageBtn(int position, int stageCount) {
        //0 = last, 1 = mid, 2 = last
        int number = position - stageCount; 
        switch (number) {
            case -1: //final
                ModifyNextStageButton(false);
                ModifyPrevStageButton(true); 
                break;
            case var value when value == (0-stageCount)://first
                ModifyPrevStageButton(false);
                ModifyNextStageButton(true); 
                break;  
            default: // middle
                ModifyNextStageButton(true);
                ModifyPrevStageButton(true);
                break; 
        }
    }
    private void ModifyNextStageButton( bool enableNextStageBtn) {
        if (enableNextStageBtn)
        {
            enableNextStageBtn = true;
            NextStageBtn.SetActive(true);
        }
        else {
            enableNextStageBtn = false;
            NextStageBtn.SetActive(false); 
        }
    }
    private void ModifyPrevStageButton(bool enablePrevStageBtn)
    {
        if (enablePrevStageBtn)
        {
            enablePrevStageBtn = true;
            PrevStageBtn.SetActive(true); 
        }
        else
        {
            enablePrevStageBtn = false;
            PrevStageBtn.SetActive(false); 
        }
    }

}
