using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class LevelSelectionController : MonoBehaviour
{
    //beberapa function buat save system itu lagi on progress jadi hiraukan aja
    public GameObject[] Levels;
    public List<LevelItem> LevelData = new List<LevelItem>();
    public GameObject modal;
    public GameObject overlay;
    

    void Awake()
    {
        Camera cam = Camera.main; 
        overlay.GetComponent<SpriteRenderer>().bounds.SetMinMax(new Vector3(), new Vector3(cam.orthographicSize, cam.orthographicSize * cam.aspect)); 
        for (int i = 0; i < Levels.Length;  i++) {
            LevelData.Add(Levels[i].GetComponent<LevelItem>()); 
}
        Debug.Log("Loading Level Data..."); 
        loadLevels();
       
    }
    private void loadLevels()
    {
        //TODO: Load data from binary here
        //PlayerData data = SaveSystem.LoadLevelData();
        GameObject lvl; 
        for (int i = 0; i < Levels.Length; i++) {
            lvl = Levels[i];
            lvl.GetComponent<LevelItem>().data = generateDummy(i);  
            //lvl.GetComponent<LevelItem>().data = data.levelData[i];
            // lvl.GetComponent<LevelItem>().data.starCount = items[i].data.starCount; 
        }
    }
    private void debug2DArray(int[,] rawNodes)
    {
        int rowLength = rawNodes.GetLength(0);
        int colLength = rawNodes.GetLength(1);
        string arrayString = "";
        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < colLength; j++)
            {
                arrayString += string.Format("{0} ", rawNodes[i, j]);
            }
            arrayString += System.Environment.NewLine + System.Environment.NewLine;
        }

        Debug.Log(arrayString);
    }
 
    private void SaveLevels() {
        //SaveSystem.SaveLevelData(this); 
    }

     LevelItemContainer generateDummy(int idCount) {
        //generate dummy, for testing only
        LevelItemContainer data = new LevelItemContainer(idCount+1);
        data.starCount = Random.Range(0, 4);
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
