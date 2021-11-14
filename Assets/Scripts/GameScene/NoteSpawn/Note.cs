using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;    // Time telling when the object is instantiated
    public double assignedTime; // Time telling when the object should arrive at the hit area
    public float noteLength;

    public Sprite noteWrong;
    public Sprite noteRight;

    private GameObject noteObject;

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
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.Instance.noteSpawnX, Vector3.right * SongManager.Instance.noteDespawnX, t);
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }

    private double CheckTime()
    {
        // If noteTime (time that each notes needed to arrive at the hit area/noteTapX from the spawn location/noteSpawnX) is bigger than the assigned time, then the condition meets.
        // For example, in our case noteTime is 3s. If there are any notes that need to be spawned at less than 3s (which is lower than the travel time) then it will go through this condition.
        if (SongManager.Instance.noteTime >= assignedTime)
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

            return SongManager.Instance.noteTime - assignedTime + SongManager.GetAudioSourceTime();
        }
        else
        {
            return SongManager.GetAudioSourceTime() - timeInstantiated;
        }
    }
}
