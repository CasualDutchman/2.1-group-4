using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    //Number of the save profile
    public int profileNumber;

    //Name of the player
    public string playerName;

    //0 = easy, 1 = medium, 2 = hard
    public int gameDifficulty;
    public int bikingDifficulty;

    //0 = left, 1 = right
    public int handDominance;

    //0 = little, 1 = medium, 2 = a lot
    public int attentionSpan;

    //Save the player settings
    void SavePlayerSettings()
    {
        //TODO Write code to save the player settings

        //Saves the player settings
        PlayerPrefs.SetString(profileNumber + "Name", playerName);
        PlayerPrefs.SetInt(profileNumber + "gameDifficulty", gameDifficulty);
        PlayerPrefs.SetInt(profileNumber + "bikingDificulty", bikingDifficulty);
        PlayerPrefs.SetInt(profileNumber + "handDominance", handDominance);
        PlayerPrefs.SetInt(profileNumber + "attentionSpan", attentionSpan);
    }

    //Get the player settings
    void GetPlayerSettings()
    {
        //TODO Write code to get the player settings

        //Gets the player settings
        PlayerPrefs.GetString(profileNumber + "Name", playerName);
        PlayerPrefs.GetInt(profileNumber + "gameDifficulty", gameDifficulty);
        PlayerPrefs.GetInt(profileNumber + "bikingDificulty", bikingDifficulty);
        PlayerPrefs.GetInt(profileNumber + "handDominance", handDominance);
        PlayerPrefs.GetInt(profileNumber + "attentionSpan", attentionSpan);
    }
}