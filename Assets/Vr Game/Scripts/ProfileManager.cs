using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public string playerName;

    //0 = easy, 1 = medium, 2 = hard
    public int gameDifficulty;
    public int bikingDifficulty;

    //0 = left, 1 = right
    public int handDominance;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    
    //Save the player settings
    void SavePlayerSettings()
    {
        //TODO Write code to save the player settings

        //Saves the player settings
        PlayerPrefs.SetString(playerName + "Name", playerName);
        PlayerPrefs.SetInt(playerName + "gameDifficulty", gameDifficulty);
        PlayerPrefs.SetInt(playerName + "bikingDificulty", bikingDifficulty);
        PlayerPrefs.SetInt(playerName + "handDominance", handDominance);

    }

    //Get the player settings
    void GetPlayerSettings()
    {
        //TODO Write code to get the player settings

        //Gets the player settings
        PlayerPrefs.GetString(playerName + "Name", playerName);
        PlayerPrefs.GetInt(playerName + "gameDifficulty", gameDifficulty);
        PlayerPrefs.GetInt(playerName + "bikingDificulty", bikingDifficulty);
        PlayerPrefs.GetInt(playerName + "handDominance", handDominance);
    }
}

[System.Serializable]
public class SaveData
{

}