using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField]
    private double timeInstantiated;
    [SerializeField]
    private double assignedTime;

    //Getter Setter
    public double GetTimeInstantiated() { return this.timeInstantiated; }    // Time telling when the object is instantiated
    public void SetTimeInstantiated(double timeInstantiated) { this.timeInstantiated = timeInstantiated; }
    public double GetAssignedTime() { return this.assignedTime; } // Time telling when the object should arrive at the hit area
    public void SetAssignedTime(double assignedTime) { this.assignedTime = assignedTime; }

    // Start is called before the first frame update
    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    //Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
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

    //private double CheckTime()
    //{
    //    if (SongManager.Instance.noteTime >= assignedTime)
    //    {
    //        return SongManager.Instance.noteTime - assignedTime + SongManager.GetAudioSourceTime();
    //    }
    //    else
    //    {
    //        return SongManager.GetAudioSourceTime() - timeInstantiated;
    //    }
    //}
}
