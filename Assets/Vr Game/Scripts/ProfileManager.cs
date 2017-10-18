using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{

    public static ProfileManager profileManager;

    //Values for the actual storing of the data
    //Number of the save profile
   
    public static int profileNumber;

    //Name of the player
    public static string playerName;

    //0 = easy, 1 = medium, 2 = hard
   
    public static int gameDifficulty;
    
    public static int bikingDifficulty;

    //0 = right, 1 = left
    public static int handDominance;

    //0 = little, 1 = medium, 2 = a lot
    public static int attentionSpan;

    private void Awake()
    {
        if(profileManager == null)
        {
            DontDestroyOnLoad(gameObject);
            profileManager = this;
        }
        else if (profileManager != this)
        {
            Destroy(gameObject);
        }
    }

    //Save the player settings
    public static void SavePlayerSettings()
    {
        //TODO Write code to save the player settings
        //UpdatePreferences(true);

        //Saves the player settings
        PlayerPrefs.SetString(profileNumber + "Name", playerName);
        PlayerPrefs.SetInt(profileNumber + "gameDifficulty", gameDifficulty);
        PlayerPrefs.SetInt(profileNumber + "bikingDificulty", bikingDifficulty);
        PlayerPrefs.SetInt(profileNumber + "handDominance", handDominance);
        PlayerPrefs.SetInt(profileNumber + "attentionSpan", attentionSpan);

        PlayerPrefs.Save();

        print("saving");
    }

    //Get the player settings
    public static void GetPlayerSettings()
    {
        //Gets the player settings
        playerName = PlayerPrefs.GetString(profileNumber + "Name");
        gameDifficulty = PlayerPrefs.GetInt(profileNumber + "gameDifficulty");
        bikingDifficulty = PlayerPrefs.GetInt(profileNumber + "bikingDificulty");
        handDominance = PlayerPrefs.GetInt(profileNumber + "handDominance");
        attentionSpan = PlayerPrefs.GetInt(profileNumber + "attentionSpan");

        //UpdatePreferences(false);

        print(playerName);
        print(gameDifficulty);
        print(bikingDifficulty);
        print(handDominance);
        print(attentionSpan);

    }

    //Getters for some of the values
    public string GetPlayerName(){return playerName;}
    public int GetGameDifficulty(){return gameDifficulty;}
    public int GetBikingDifficulty(){return bikingDifficulty;}
    public int GetHandDominance(){return handDominance;}
    public int GetAttentionSpan(){return attentionSpan;}

    //setters for the variables used
    public void SetPlayerName(string name) {playerName = name; }
    public void SetGameDifficulty(int gamingDifficulty) {gameDifficulty = gamingDifficulty; }
    public void SetBikingDifficulty(int bikeDifficulty) {bikingDifficulty = bikeDifficulty; }
    public void SetHandDominance(int dominantHand) {handDominance = dominantHand; }
    public void SetAttentionSpan(int attention) {attentionSpan = attention; }

}