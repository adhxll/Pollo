using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class StageConstructor : MonoBehaviour
{ // used to select a stage from the stages and instantiate it onto the level
    public GameObject stagePrefab;
    public List<GameObject> stagePrefabList; 
    int selectedStageID; //change this to change stage
    public Stages stages;
    public Vector3 defaultCameraPosition;
    //dump positions for islands to go out of camera render space
    public Vector3 dumpPositionPrev = new Vector3(-100, 0, 0);
    public Vector3 dumpPositionNext = new Vector3(100, 0, 0);
    private void Start()
    {
        InstantiateStage(); 
    }
    //TODO: - function to change stage ID based on selected stage on GameController
    private void InstantiateStage() {
        
        defaultCameraPosition = Camera.main.transform.position;
        selectedStageID = 0;
        LevelSelectionController.Instance.PrevStageBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);  
        
        Vector3 stagePosition = new Vector3(0, 0, 0); 
        SpriteRenderer stageRenderer = new SpriteRenderer();
       
        //only for 1st stage
        stagePrefab = Instantiate(stages.stagesList[0], new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform.parent);
        stagePrefabList.Add(stagePrefab);
        for (int i = 1; i < stages.stagesList.Count; i++) {
            stageRenderer = stagePrefab.GetComponent<SpriteRenderer>();
            //instantiate prefab right next to previous prefab (not going to be rendered by camera)
            stagePrefab = Instantiate(stages.stagesList[i], dumpPositionNext, Quaternion.identity, gameObject.transform.parent);
            stagePrefabList.Add(stagePrefab);

        }
    
    }
    public void ChangeStageNext() {
        if (selectedStageID != stagePrefabList.Count - 1)
        {
            selectedStageID++;
            //move previous stage into dump position and move current into view
            stagePrefabList[selectedStageID - 1].GetComponent<Transform>().position = dumpPositionPrev;
            stagePrefabList[selectedStageID].GetComponent<Transform>().position = new Vector3(0, 0, 0);
            Camera.main.transform.position = defaultCameraPosition;
        } 
        

    }
    public void ChangeStagePrev()
    {
        if (selectedStageID != 0)
        {
            selectedStageID--;
            //move previous stage into dump position and move current into view
            stagePrefabList[selectedStageID + 1].GetComponent<Transform>().position = dumpPositionNext;
            stagePrefabList[selectedStageID].GetComponent<Transform>().position = new Vector3(0, 0, 0);
            Camera.main.transform.position = defaultCameraPosition;
        }
    }

}
