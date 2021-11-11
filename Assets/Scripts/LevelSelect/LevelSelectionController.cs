using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class LevelSelectionController : MonoBehaviour
{
    public GameObject[] Levels;
    public List<LevelItem> LevelData = new List<LevelItem>();
    public GameObject modal;
    public GameObject overlay;
    

    void Awake()
    {
        Camera cam = Camera.main; 
        
        for (int i = 0; i < Levels.Length;  i++) {
            LevelData.Add(Levels[i].GetComponent<LevelItem>()); 
}
        Debug.Log("Loading Level Data..."); 
        loadLevels();
       
    }
    private void loadLevels()
    {
        //TODO: Load data from binary here
        GameObject lvl; 
        for (int i = 0; i < Levels.Length; i++) {
            lvl = Levels[i];
            lvl.GetComponent<LevelItem>().data = generateDummy(i);  
        }
    }
  
   

     LevelItemContainer generateDummy(int idCount) {
        //generate dummy, for testing only
        LevelItemContainer data = new LevelItemContainer(idCount+1);
        data.starCount = 0; 
        data.isUnlocked = true; 
        return data;    
    }
   
    public void ShowLevelModal(GameObject sourceLevel)
    {
        //set modal data menjadi level data
        var levelData = sourceLevel.GetComponent<LevelItem>().data;
        var modalData = modal.GetComponent<ModalController>();
        modal.GetComponent<StarCounter>().StarCount = levelData.starCount;
        modalData.scoreValue = levelData.highScore.ToString();
        modalData.levelValue = "Level " + levelData.getLevelCount();
        //TODO: - get level ID then set modal data menjadi level ID
       
        if (!modal.activeSelf)
        {
            modal.GetComponent<StarCounter>().FillStars();
            modalData.SetValues();
            AnimationUtilities.AnimatePopUp(modal);
            modal.SetActive(true);
            overlay.SetActive(true); 
        }
    }
    public void CloseModal() {
        AnimationUtilities.AnimatePopUpDisappear(modal);
        Invoke(nameof(DeactivateModal), 0.2f);  //biar animasinya keplay dulu sebelom diclose
    }
    private void DeactivateModal() {
        modal.GetComponent<StarCounter>().EmptyStars();
        modal.SetActive(false);
        overlay.SetActive(false); 
    }
    
  




}
