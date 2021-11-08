using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BarTimeline : MonoBehaviour
{
    [SerializeField]
    private GameObject barBackground = null;

    [SerializeField]
    private GameObject barIndicator = null;

    [SerializeField]
    private GameObject barPrefab = null;

    private List<double> timeStamps = new List<double>();
    private List<int> spawnIndex = new List<int>();
    private List<int> inputIndex = new List<int>();

    SpriteRenderer barSprite
    {
        get
        {
            return barIndicator.GetComponent<SpriteRenderer>();
        }
    }

    float barSpriteScale
    {
        get
        {
            return barSprite.transform.localScale.x;
        }
    }

    float height
    {
        get
        {
            return barSprite.size.y;
        }
    }

    float width;
    int currentSection = 0;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        var relativePost = SongManager.Instance.GetCurrentAudioProgress();
        var progressWidth = width / barSpriteScale * relativePost;

        // Determin sprite height & width, also setting the minimum width
        if (progressWidth < 0.6f)
            progressWidth = 0.6f;

        // Update sprite width based on current audio progress
        barSprite.size = new Vector2(progressWidth, height);

        SectionManager();
    }

    void Initialize()
    {
        // Calculate parent object width
        var rect = barBackground.GetComponent<RectTransform>();
        width = rect.sizeDelta.x;

        // Initialize spawnCount first value
        spawnIndex.Add(0);
        inputIndex.Add(0);
    }

    public void PlaceTimestamp()
    {
        foreach (var time in timeStamps)
        {
            var left = new Vector2(- width / 2, 0);
            var right = new Vector2(width / 2, 0);
            var bar = Instantiate(barPrefab, transform);
            var relativePost = time / SongManager.Instance.GetAudioSourceLength();

            // Set bar width and height
            bar.transform.localScale = new Vector2(0.25f, 1.25f);

            // Place bar based on linear interpolation between start point, end point, and audio progress
            bar.transform.localPosition = Vector3.Lerp(left, right, (float)relativePost);
        }
    }

    // Populate section with 'C1' note from midi as separator
    public void SetTimeStamps(MIDI.Notes[] array)
    {
        foreach (var note in array)
        {
            timeStamps.Add(note.time);
        }
    }

    void SectionManager()
    {
        // Determine current section that the player's at
        if (currentSection < timeStamps.Count - 1)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[currentSection + 1])
            {
                currentSection++;
                spawnIndex.Add(Lane.Instance.spawnIndex);
                inputIndex.Add(Lane.Instance.inputIndex);
            }
        }

        // Restart current section
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneStateManager.Instance.SetAudioTime((float)timeStamps[currentSection]);
            Lane.Instance.spawnIndex = spawnIndex[currentSection];
            Lane.Instance.inputIndex = inputIndex[currentSection];
        }

        // Go to previous section
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (currentSection > 0)
                currentSection--;

            SceneStateManager.Instance.SetAudioTime((float)timeStamps[currentSection]);
            Lane.Instance.Reset();
            Lane.Instance.spawnIndex = spawnIndex[currentSection];
            Lane.Instance.inputIndex = inputIndex[currentSection];
        }

        // Go to next section
        if (Input.GetKeyUp(KeyCode.T))
        {
            if (currentSection < timeStamps.Count)
                currentSection++;

            SceneStateManager.Instance.SetAudioTime((float)timeStamps[currentSection]);
            Lane.Instance.Reset();
            Lane.Instance.spawnIndex = spawnIndex[currentSection];
            Lane.Instance.inputIndex = inputIndex[currentSection];
        }
    }
}
