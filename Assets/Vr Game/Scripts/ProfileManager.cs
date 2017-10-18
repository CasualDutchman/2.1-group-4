using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{

    public static ProfileManager profileManager;

    //Values for the actual storing of the data
    //Number of the save profile
   
    static int profileNumber;

    //Name of the player
    static string playerName;

    //0 = easy, 1 = medium, 2 = hard
   
    static int gameDifficulty;
    
    static int bikingDifficulty;

    //0 = right, 1 = left
    static int handDominance;

    //0 = little, 1 = medium, 2 = a lot
    static int attentionSpan;

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

        //Saves the player settings
        PlayerPrefs.SetString(profileNumber + "Name", playerName);
        PlayerPrefs.SetInt(profileNumber + "gameDifficulty", gameDifficulty);
        PlayerPrefs.SetInt(profileNumber + "bikingDificulty", bikingDifficulty);
        PlayerPrefs.SetInt(profileNumber + "handDominance", handDominance);
        PlayerPrefs.SetInt(profileNumber + "attentionSpan", attentionSpan);

        PlayerPrefs.Save();
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
    }

    //Getters for some of the values
    public static int GetProfileNumber() {return profileNumber;}
    public static string GetPlayerName(){return playerName;}
    public static int GetGameDifficulty(){return gameDifficulty;}
    public static int GetBikingDifficulty(){return bikingDifficulty;}
    public static int GetHandDominance(){return handDominance;}
    public static int GetAttentionSpan(){return attentionSpan;}

    //setters for the variables used
    public static void SetProfileNumber(int profileNo) {profileNumber = profileNo;}
    public static void SetPlayerName(string name) {playerName = name; }
    public static void SetGameDifficulty(int gamingDifficulty) {gameDifficulty = gamingDifficulty; }
    public static void SetBikingDifficulty(int bikeDifficulty) {bikingDifficulty = bikeDifficulty; }
    public static void SetHandDominance(int dominantHand) {handDominance = dominantHand; }
    public static void SetAttentionSpan(int attention) {attentionSpan = attention; }

}