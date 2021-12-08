using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class StageConstructor : MonoBehaviour
{ // used to select a stage from the stages and instantiate it onto the level
    public GameObject stagePrefab;
    public static StageConstructor Instance; 
    int selectedStageID; //change this to change stage
    public Stages stages;
    public string stageName = ""; 
    public TMPro.TextMeshProUGUI stageNameText;
    public Vector3 defaultCameraPosition;
    //dump positions for islands to go out of camera render space
    public Vector3 dumpPositionPrev = new Vector3(-50, 0, 0);
    public Vector3 dumpPositionNext = new Vector3(100, 0, 0);
    private Vector3 defaultIslandPosition = new Vector3(0, 0, 0); 
    private void Start()
    {
        InstantiateStage();
        StartCustomSingleton(); 
    }

    private void StartCustomSingleton()
    {
        // used to keep this script as a static singleton, but only within the level selection scene
        // needed for supporting multiple island prefabs, see LevelItem for example
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
    private void InstantiateStage() {
        
        defaultCameraPosition = Camera.main.transform.position;
        selectedStageID = 0;
        LevelSelectionController.Instance.ModifyChangeStageBtn(selectedStageID, stages.stagesList.Count);  
        stagePrefab = Instantiate(stages.stagesList[0], new Vector3(0, 0, 0),
            Quaternion.identity, gameObject.transform.parent);
        //set scale here because one change applies to all
        stagePrefab.transform.localScale = new Vector3(40f, 40f, 1); //awaiting for ayas change,
        ChangeStageName(stagePrefab.GetComponent<StageController>().stageName);


    }
    public void ChangeStageNext() {
        if (selectedStageID != stages.stagesList.Count-1)
        {
            StartCoroutine(ChangeIsland(0.3f, true));
            GameController.Instance.currentStage = selectedStageID;
            
        } 
    }
    public void ChangeStagePrev()
    {
        if (selectedStageID != 0)
        {
            StartCoroutine(ChangeIsland(0.3f, false));
            Camera.main.transform.position = defaultCameraPosition;
            GameController.Instance.currentStage = selectedStageID;
           
        }
    }
    private IEnumerator ChangeIsland(float countTime, bool toNext)
    { // used as a coroutine to animate the movement of islands. 
        Vector3 destination = new Vector3(0, 0, 0);
        Vector3 start =  new Vector3(0, 0, 0); ;
        if (toNext)
        {
            selectedStageID++;
            destination = dumpPositionPrev;
            start = dumpPositionNext;
        }
        else {
            selectedStageID--;
            destination = dumpPositionNext;
            start = dumpPositionPrev; 
        }
        //Move the camera to default place and island to dump position
        AnimationUtilities.Instance.MoveX(stagePrefab, destination.x, stagePrefab.transform.position.x);
        AnimationUtilities.Instance.MoveCameraX(Camera.main, defaultCameraPosition.x, Camera.main.transform.position.x);
        yield return new WaitForSeconds(countTime);

        //destroy the island after delay then instantiate a new one and move it into place
        Destroy(stagePrefab);
        stagePrefab = Instantiate(stages.stagesList[selectedStageID], defaultIslandPosition, Quaternion.identity, gameObject.transform.parent);
        stagePrefab.transform.localScale = new Vector3(40f, 40f, 1); 
        AnimationUtilities.Instance.MoveX(stagePrefab, defaultIslandPosition.x, start.x);
        LevelSelectionController.Instance.ModifyChangeStageBtn(selectedStageID, stages.stagesList.Count); // change the buttons in levelselectoin
        ChangeStageName(stagePrefab.GetComponent<StageController>().stageName);
        StopCoroutine(ChangeIsland(0.2f, true));
    }
    private void ChangeStageName(string StageName) {
        stageNameText.text = StageName; 
    }
   
}
