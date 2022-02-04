using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    private int currentCharacter = 0; // currentCharacter is a variable that holds current characterId
    // Start is called before the first frame update

    void Start()
    {

    }

    void Initialize()
    {
        SetCurrentCharacter(PlayerPrefs.GetInt(PlayerDataNames.Character.ToString(), 0));
    }

    public void SetCurrentCharacter(int skinId)
    {
        this.currentCharacter = skinId;
        PlayerPrefs.SetInt(PlayerDataNames.Character.ToString(), skinId); // Automatically saves the new value to PlayerPrefs
    }

    public int GetCurrentCharacter()
    {
        return this.currentCharacter;
    }
}
