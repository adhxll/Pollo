using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BarTimeline : MonoBehaviour
{
    [SerializeField] private GameObject barBackground = null;
    [SerializeField] private GameObject barIndicator = null;
    [SerializeField] private GameObject barPrefab = null;
    [Space]
    [SerializeField] private Button previousButton = null;
    [SerializeField] private Button nextButton = null;

    private List<GameObject> barList = new List<GameObject>();
    private List<double> timestamp = new List<double>();

    int currentSection = 0;
    bool repeatSection = false;

    RectTransform barSprite
    {
        get
        {
            return barIndicator.GetComponent<RectTransform>();
        }
    }

    float height
    {
        get
        {
            return barSprite.rect.height;
        }
    }

    float width
    {
        get
        {
            var rect = barBackground.GetComponent<RectTransform>();
            return rect.rect.width;
        }
    }

    private void Start()
    {
        CheckButton();
    }

    // Update is called once per frame
    void Update()
    {
        var relativePost = SongManager.Instance.GetCurrentAudioProgress();
        var progressWidth = width * relativePost;

        // Update sprite width based on current audio progress
        barSprite.sizeDelta = new Vector2(progressWidth, height);

        CheckRepeatSection();
        SectionManager();
    }

    // Populate section with 'C1' note from midi as separator
    public void SetTimeStamps(MIDI.Notes[] array)
    {
        foreach (var note in array)
        {
            timestamp.Add(note.time + SongManager.Instance.GetNoteDelay());
        }
    }

    // Place separator/divider according to their position in audioSourceLength
    public void PlaceTimestamp()
    {
        foreach (var time in timestamp)
        {
            var left = new Vector2(- width / 2, 0);
            var right = new Vector2(width / 2, 0);
            var bar = Instantiate(barPrefab, transform);
            var relativePost = time / SongManager.Instance.GetAudioSourceLength();

            // Add instantiated bar to list
            barList.Add(bar);

            // Set bar width and height
            bar.transform.localScale = new Vector2(1f, 16f);

            // Place bar based on linear interpolation between start point, end point, and audio progress
            bar.transform.localPosition = Vector3.Lerp(left, right, (float)relativePost);
        }
    }

    // Configuring inputIndex and spawnIndex for each section
    public void ConfigureSection()
    {
        var musicNotes = Lane.Instance.GetTimeStamps();
        var noteTime = SongManager.Instance.GetInputDelayInMilliseconds() / 1000;
        var i = 0;
        var j = 0;

        // Loop through each section timestamp
        foreach (var section in timestamp)
        {
            var barObj = barList[j].GetComponent<Timestamp>();

            // Loop through all the notes timestamp
            for (; i < musicNotes.Count; i++)
            {
                // If section timestamp is equal to note timestamp
                // Or note timestamp at index [i] is larger than the section timestamp
                // Then [i] is the inputIndex
                if (section == musicNotes[i] || section < musicNotes[i])
                {
                    barObj.SetInputIndex(i);
                }

                // After getting the inputIndex, continue the loop to search for spawnIndex
                // If section timestamp + noteTime (note travel time) is lower than note timestamp at index [i]
                // Then [i] is the spawnIndex
                if (section < musicNotes[i] + noteTime)
                {
                    barObj.SetSpawnIndex(i);
                    j++;
                    break;
                }
            }
        }
    }

    void SectionManager()
    {
        // Determine the current section that the player's at
        if (currentSection < timestamp.Count - 1)
        {
            if (SongManager.GetAudioSourceTime() >= timestamp[currentSection + 1])
            {
                if (repeatSection)
                {
                    RepeatSection();
                }
                else
                {
                    currentSection++;
                    SetTimestampStyle();
                }
            }
        }
    }

    void CheckButton()
    {
        previousButton.interactable = true;
        nextButton.interactable = true;

        if (currentSection == 0)
            previousButton.interactable = false;

        if (currentSection == timestamp.Count - 1)
            nextButton.interactable = false;
    }

    public void RepeatSection()
    {
        ResetLane();
        Lane.Instance.ClearRest();
    }

    public void NextSection()
    {
        if (currentSection < timestamp.Count - 1)
            currentSection++;

        ResetLane();
        Lane.Instance.FillRest();
    }

    public void PreviousSection()
    {
        if (currentSection > 0)
            currentSection--;

        ResetLane();
        Lane.Instance.ClearRest();
    }

    void CheckRepeatSection()
    {
        var isRepeat = PlayerPrefs.GetInt(SettingsList.RepeatSection.ToString());
        var currScene = SceneStateManager.Instance.GetSceneState();

        if (isRepeat > 0 && currScene == SceneStateManager.SceneState.Practice)
            repeatSection = true;
        else
            repeatSection = false;
    }

    void SetTimestampStyle()
    {
        CheckButton();

        for (int i = 0; i < timestamp.Count; i++)
        {
            barList[i].GetComponent<Image>().color = Color.white;

            if (i <= currentSection)
                barList[i].GetComponent<Image>().color = new Color32(255, 200, 113, 255); 
        }
    }

    // Assign some lane input index variables to the saved index
    void ResetLane()
    {
        var barObj = barList[currentSection].GetComponent<Timestamp>();
        SongManager.Instance.SetAudioPosition((float)timestamp[currentSection]);
        Lane.Instance.SetIndexValue(barObj.GetSpawnIndex(), barObj.GetInputIndex());
        Lane.Instance.DestroyChild();
        SetTimestampStyle();

        SongManager.Instance.PlaySong();
    }
}
