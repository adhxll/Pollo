using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Assets/Level")]
public class Level : ScriptableObject
{
    [SerializeField]
    private int levelID;
    [SerializeField]
    private AudioClip leadTrack;
    [SerializeField]
    private AudioClip backingTrack;
    [SerializeField]
    private TextAsset midiJson;

    public int GetLevelID() { return this.levelID; }
    public AudioClip GetLeadTrack() { return this.leadTrack; }
    public AudioClip GetBackingTrack() { return this.backingTrack; }
    public TextAsset GetMidiJson() { return this.midiJson; }
}
