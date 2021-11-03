using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTimeline : MonoBehaviour
{
    [SerializeField]
    private GameObject barBackground = null;

    [SerializeField]
    private GameObject barIndicator = null;

    [SerializeField]
    private GameObject barPrefab = null;

    private List<double> timeStamps = new List<double>();

    float height;
    float width;
    Vector3 min;
    Vector3 max;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        var component = barIndicator.GetComponent<SpriteRenderer>();
        var relativePost = SongManager.Instance.GetCurrentAudioProgress();
        height = component.size.y;
        component.size = new Vector2((width * 2) * relativePost, height);
    }

    void Initialize()
    {
        // Calculate parent object width
        var component = barBackground.GetComponent<SpriteRenderer>();
        min = component.bounds.min;
        max = component.bounds.max;
        min.y = 0;
        max.y = 0;

        width = max.x - min.x;
    }

    public void PlaceTimestamp()
    {
        foreach (var time in timeStamps)
        {
            var bar = Instantiate(barPrefab, transform);
            var relativePost = time / SongManager.Instance.GetAudioSourceLength();
            bar.transform.localScale = new Vector2(0.25f, 1.25f);

            bar.transform.localPosition = Vector3.Lerp(min * 5, max * 5, (float)relativePost);
        }
    }

    public void SetTimeStamps(MIDI.Notes[] array)
    {
        foreach (var note in array)
        {
            timeStamps.Add(note.time);
        }
    }
}
