using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Assets/Level")]
public class Level : ScriptableObject
{
    [SerializeField]
    private int levelID = 0;
    [SerializeField]
    private int stageID = 0; 
    [SerializeField]
    private AudioClip leadTrack = null;
    [SerializeField]
    private AudioClip backingTrack = null;
    [SerializeField]
    private TextAsset midiJson = null;

    public int GetLevelID() { return this.levelID; }
    public int GetStageID() { return this.stageID; }
    public AudioClip GetLeadTrack() { return this.leadTrack; }
    public AudioClip GetBackingTrack() { return this.backingTrack; }
    public TextAsset GetMidiJson() { return this.midiJson; }
}
