using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelectionController : MonoBehaviour
{
    public static LevelSelectionController Instance;

    [SerializeField] private Canvas levelCanvas;
    [SerializeField] private Button PrevStageBtn;
    [SerializeField] private Button NextStageBtn;
    
    private bool modalShown;

    private void Awake()
    {
        StartCustomSingleton(); 
    }

    public void SaveGame()
    {
        SaveSystem.SavePlayerData(); 
    }

    private void StartCustomSingleton()
    {
        // Used to keep this script as a static singleton, but only within the level selection scene
        // Needed for supporting multiple island prefabs, see LevelItem for example
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void ModifyChangeStageBtn(int position, int stageCount)
    {
        // 0 = last, 1 = mid, 2 = last
        int number = position - stageCount; 
        switch (number) {
            case -1: // Final
                ModifyNextStageButton(false);
                ModifyPrevStageButton(true); 
                break;
            case var value when value == (0-stageCount)://first
                ModifyPrevStageButton(false);
                ModifyNextStageButton(true); 
                break;  
            default: // Middle
                ModifyNextStageButton(true);
                ModifyPrevStageButton(true);
                break; 
        }
        //ModifyStageCount(); 
    }

    private void ModifyNextStageButton(bool enableNextStageBtn)
    {
        if (enableNextStageBtn)
            NextStageBtn.interactable = true;
        else
            NextStageBtn.interactable = false; 
    }

   

    private void ModifyPrevStageButton(bool enablePrevStageBtn)
    {
        if (enablePrevStageBtn)
            PrevStageBtn.interactable = true;
        else
            PrevStageBtn.interactable = false;
    }

}
