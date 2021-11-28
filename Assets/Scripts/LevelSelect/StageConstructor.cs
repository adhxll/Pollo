using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageConstructor : MonoBehaviour
{ // used to select a stage from the database and instantiate it onto the level
    public GameObject stagePrefab; 
    int selectedStageID; //change this to change stage
    public Stages stages; 
    private void Awake()
    {
        InstantiateStage(); 
    }
    //TODO: - function to change stage ID based on selected stage on GameController
    private void InstantiateStage() {
        selectedStageID = 0;
        stagePrefab = stages.stagesList[selectedStageID];
        Instantiate(stagePrefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform.parent);
    }
}
