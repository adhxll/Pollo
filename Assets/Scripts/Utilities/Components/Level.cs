using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Assets/Level")]
public class Level : ScriptableObject
{
    public int levelID;
    public AudioClip leadTrack;
    public AudioClip backingTrack;
    public TextAsset midiJson;
}
