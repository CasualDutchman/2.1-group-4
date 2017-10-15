using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{

    public InputField nameInputField;

    //Buttons for the different profiles
    public Button profile1, profile2, profile3, profile4, profile5, profile6;

    //Buttons for loading and saving the data
    public Button save, load;

    //Sliders for the difficulties and the attention span
    public Slider bikeDifficultySlider, gameDifficultySlider, attentionSpanSlider;

    //Dropdown menu for the hand dominance
    public Dropdown handDominanceMenu;

    //Number of the save profile
    public int profileNumber;

    //Name of the player
    public string playerName;

    //0 = easy, 1 = medium, 2 = hard
    public int gameDifficulty;
    public int bikingDifficulty;

    //0 = right, 1 = left
    public int handDominance;

    //0 = little, 1 = medium, 2 = a lot
    public int attentionSpan;

    //Save the player settings
    public void SavePlayerSettings()
    {
        //TODO Write code to save the player settings
        UpdateName();
        UpdateGameDifficulty();
        UpdateBikingDifficulty();
        UpdateAttentionSpan();
        UpdateHandDominance();

        //Saves the player settings
        PlayerPrefs.SetString(profileNumber + "Name", playerName);
        PlayerPrefs.SetInt(profileNumber + "gameDifficulty", gameDifficulty);
        PlayerPrefs.SetInt(profileNumber + "bikingDificulty", bikingDifficulty);
        PlayerPrefs.SetInt(profileNumber + "handDominance", handDominance);
        PlayerPrefs.SetInt(profileNumber + "attentionSpan", attentionSpan);
    }

    //Get the player settings
    public void GetPlayerSettings()
    {
        //TODO Write code to get the player settings

        //Gets the player settings
        playerName = PlayerPrefs.GetString(profileNumber + "Name");
        gameDifficulty = PlayerPrefs.GetInt(profileNumber + "gameDifficulty");
        bikingDifficulty = PlayerPrefs.GetInt(profileNumber + "bikingDificulty");
        handDominance = PlayerPrefs.GetInt(profileNumber + "handDominance");
        attentionSpan = PlayerPrefs.GetInt(profileNumber + "attentionSpan");

        print(playerName);
        print(gameDifficulty);
        print(bikingDifficulty);
        print(handDominance);
        print(attentionSpan);

    }

    public void UpdateName()
    {
        playerName = nameInputField.text;
    }

    public void UpdateGameDifficulty()
    {
        //gameDifficulty = bikeDifficultySlider.value;
    }

    public void UpdateBikingDifficulty()
    {
        //bikingDifficulty = bikeDifficultySlider.value;
    }

    public void UpdateHandDominance()
    {
        handDominance = handDominanceMenu.value;
    }

    public void UpdateAttentionSpan()
    {
        //attentionSpan = attentionSpanSlider.value;
    }

    //TODO Write load profile screen

    //TODO write save profile screen
}