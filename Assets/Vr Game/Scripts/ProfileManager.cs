using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{

    public static ProfileManager profileManager;

    //UI Elements
    public Canvas profileCanvas = null;

    //drop down for the profile number
    public Dropdown profileNumberDropdown;

    //input field for the name
    public InputField nameInputField;

    //Buttons for loading and saving the data
    public Button save, load;

    //Sliders for the difficulties and the attention span
    public Slider bikeDifficultySlider, gameDifficultySlider, attentionSpanSlider;

    //Dropdown menu for the hand dominance
    public Dropdown handDominanceMenu;


    //Values for the actual storing of the data
    //Number of the save profile
    [HideInInspector]
    public int profileNumber;

    //Name of the player
    [HideInInspector]
    public string playerName;

    //0 = easy, 1 = medium, 2 = hard
    [HideInInspector]
    public int gameDifficulty;
    [HideInInspector]
    public int bikingDifficulty;

    //0 = right, 1 = left
    [HideInInspector]
    public int handDominance;

    //0 = little, 1 = medium, 2 = a lot
    [HideInInspector]
    public int attentionSpan;

    private void Awake()
    {
        if(profileManager == null)
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(profileCanvas);
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
        UpdatePreferences(true);

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
        //Gets the player settings
        playerName = PlayerPrefs.GetString(profileNumber + "Name");
        gameDifficulty = PlayerPrefs.GetInt(profileNumber + "gameDifficulty");
        bikingDifficulty = PlayerPrefs.GetInt(profileNumber + "bikingDificulty");
        handDominance = PlayerPrefs.GetInt(profileNumber + "handDominance");
        attentionSpan = PlayerPrefs.GetInt(profileNumber + "attentionSpan");

        UpdatePreferences(false);

        print(playerName);
        print(gameDifficulty);
        print(bikingDifficulty);
        print(handDominance);
        print(attentionSpan);

    }

    //Updates the data in the variables and the UI elements
    public void UpdatePreferences(bool saving)
    {
        if(saving)
        {
            //assign all of the variables from the UI
            playerName = nameInputField.text;
            gameDifficulty = Mathf.RoundToInt(gameDifficultySlider.value);
            bikingDifficulty = Mathf.RoundToInt(bikeDifficultySlider.value);
            handDominance = handDominanceMenu.value;
            attentionSpan = Mathf.RoundToInt(attentionSpanSlider.value);
        }

        if (!saving)
        {
            //TODO Write code to update the UI elements
            nameInputField.text = playerName;
            gameDifficultySlider.value = gameDifficulty;
            bikeDifficultySlider.value = bikingDifficulty;
            handDominanceMenu.value = handDominance;
            attentionSpanSlider.value= attentionSpan;
        }

    }

    public void UpdateProfileNumber()
    {
        profileNumber = profileNumberDropdown.value;
    }
    
    public void ExitProfileEditor()
    {
        profileCanvas.enabled = false;
    }

    public void EnterProfileEditor()
    {
        profileCanvas.enabled = true;
    }
}