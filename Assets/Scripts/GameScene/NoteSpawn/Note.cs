using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private double timeInstantiated;    // Time telling when the object is instantiated
    [SerializeField]
    private double assignedTime; // Time telling when the object should arrive at the hit area
    [SerializeField]
    private float noteLength;

    [SerializeField]
    private Sprite noteWrong;
    [SerializeField]
    private Sprite noteRight;
    private GameObject noteObject;

    public double GetTimeInstantiated() { return this.timeInstantiated; }    // Time telling when the object is instantiated
    public void SetTimeInstantiated(double timeInstantiated) { this.timeInstantiated = timeInstantiated; }
    public double GetAssignedTime() { return this.assignedTime; } // Time telling when the object should arrive at the hit area
    public void SetAssignedTime(double assignedTime) { this.assignedTime = assignedTime; }
    public float GetNoteLength() { return this.noteLength; }
    public void SetNoteLength(float noteLength) { this.noteLength = noteLength; }

    public Sprite GetNoteWrong() { return this.noteWrong; }
    public void SetNoteWrong(Sprite noteWrong) { this.noteWrong = noteWrong; }
    public Sprite GetNoteRight() { return this.noteRight; }
    public void SetNoteRight(Sprite noteRight) { this.noteRight = noteRight; }
    public GameObject GetNoteObject() { return this.noteObject; }
    public void SetNoteObject(GameObject noteObject) { this.noteObject = noteObject; }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        noteObject = GetComponent<GameObject>();
        timeInstantiated = SongManager.GetAudioSourceTime();    // Set value to the current time (the exact moment when it's instantiated)
        GetComponent<SpriteRenderer>().size = new Vector2(noteLength * 2.5f, 0.6f); // Set the note size according to the note length (in midi file)
    }

    void Update()
    {
        double timeSinceInstantiated = CheckTime();
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.GetNoteTime() * 2));

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.Instance.GetNoteSpawnX(), Vector3.right * SongManager.Instance.GetNoteDespawnX(), t);
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }

    private double CheckTime()
    {
        // If noteTime (time that each notes needed to arrive at the hit area/noteTapX from the spawn location/noteSpawnX) is bigger than the assigned time, then the condition meets.
        // For example, in our case noteTime is 3s. If there are any notes that need to be spawned at less than 3s (which is lower than the travel time) then it will go through this condition.
        if (SongManager.Instance.GetNoteTime() >= assignedTime)
        {
            // If those condition meet, we'll calculate the interpolation where those note should be.
            // As you can see, at the moment we use Lerp function at line 36.
            // In case you're wondering how does Lerp works, basically it's interpolating where does the object position should be by the given parameter.
            // To make it easier, 't' is a value between 0 - 1.
            // To visualize ...
            // 1 --------------------------------------- 0 -> 't' value
            // ----------------------------------------- X -> Spawn position
            // A -------------------0------------------- B -> Start & end Position, 0 is hit location

            // This function below, basically telling/calculating where should the notes spawn if their assigned time is less then the travel time.
            // By calculating the difference between travel time & assigned time.
            // 1 --------------------------------------- 0 -> 't' value
            // --------------------------------X--------  -> Spawn position
            // A -------------------0------------------- B -> Start & end Position, 0 is hit location

            return SongManager.Instance.GetNoteTime() - assignedTime + SongManager.GetAudioSourceTime();
        }
        else
        {
            return SongManager.GetAudioSourceTime() - timeInstantiated;
        }
    }
}
