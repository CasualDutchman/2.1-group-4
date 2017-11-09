using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{

    public static ProfileManager profileManager;

    public WorldManager manager;

    //Values for the actual storing of the data
    //Number of the save profile
   
    int profileNumber;

    //Name of the player
    string playerName;

    //0 = easy, 1 = medium, 2 = hard
    int gameDifficulty;
    int bikingDifficulty;

    //0 = right, 1 = left
    int handDominance;

    //0 = little, 1 = medium, 2 = a lot
    float attentionSpan;

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
    public void SavePlayerSettings()
    {
        //TODO Write code to save the player settings

        //Saves the player settings
        PlayerPrefs.SetString(profileNumber + "Name", playerName);
        PlayerPrefs.SetInt(profileNumber + "gameDifficulty", gameDifficulty);
        PlayerPrefs.SetInt(profileNumber + "bikingDificulty", bikingDifficulty);
        PlayerPrefs.SetInt(profileNumber + "handDominance", handDominance);
        PlayerPrefs.SetFloat(profileNumber + "attentionSpan", attentionSpan);

        PlayerPrefs.Save();
    }

    //Get the player settings
    public void GetPlayerSettings()
    {
        //Gets the player settings
        playerName = PlayerPrefs.GetString(profileNumber + "Name");
        gameDifficulty = PlayerPrefs.GetInt(profileNumber + "gameDifficulty");
        bikingDifficulty = PlayerPrefs.GetInt(profileNumber + "bikingDificulty");
        handDominance = PlayerPrefs.GetInt(profileNumber + "handDominance");
        attentionSpan = PlayerPrefs.GetFloat(profileNumber + "attentionSpan");
        SetAttentionSpan(attentionSpan);
    }

    //Getters for the variables used
    public int GetProfileNumber() {return profileNumber;}
    public string GetPlayerName(){return playerName;}
    public int GetGameDifficulty(){return gameDifficulty;}
    public int GetBikingDifficulty(){return bikingDifficulty;}
    public int GetHandDominance(){return handDominance;}
    public float GetAttentionSpan(){return attentionSpan;}

    //setters for the variables used
    public void SetProfileNumber(int profileNo) {profileNumber = profileNo;}
    public void SetPlayerName(string name) {playerName = name; }
    public void SetGameDifficulty(int gamingDifficulty) {gameDifficulty = gamingDifficulty; }
    public void SetBikingDifficulty(int bikeDifficulty) {bikingDifficulty = bikeDifficulty; }
    public void SetHandDominance(int dominantHand) {handDominance = dominantHand; }
    public void SetAttentionSpan(float attention) {attentionSpan = attention; manager.percentageSpawn = attentionSpan; }

}