using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class LevelSelectionController : MonoBehaviour
{
    //i would like a break
    public Button[] levels;
    public List<LevelItem> items = new List<LevelItem>();
    TouchPhase phase = TouchPhase.Began;
    Vector2 touchPosition;


    void Awake()
    {
        for (int i = 0; i < levels.Length;  i++) {
            items.Add(levels[i].GetComponent<LevelItem>()); 
}
        Debug.Log("Loading Level Data..."); 
        loadLevels();
       
    }
    private void loadLevels()
    {
        //TODO: Load data from binary here
      //  PlayerData data = SaveSystem.LoadLevelData();
        Button lvl = GetComponent<UnityEngine.UI.Button>();
        for (int i = 0; i < levels.Length; i++) {
            lvl = levels[i];
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
        //generate dummy
            LevelItemContainer data = new LevelItemContainer(idCount+1);
            data.starCount = 2; 

        return data; 
 
         //initialize isi attribute, nanti kalo udah load dari binary aja
        
    }
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == phase)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            RaycastHit2D hitInfo = Physics2D.Raycast(touchPosition, Camera.main.transform.forward);

            if (hitInfo.collider != null)
            {
                GameObject touchedObject = hitInfo.transform.gameObject;
                Debug.Log("Touched" + touchedObject.transform.name);

            }

        }
    }



}
