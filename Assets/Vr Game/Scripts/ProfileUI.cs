using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour {
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


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Updates the data in the variables and the UI elements
    public void UpdatePreferences(bool saving)
    {
        if (saving)
        {
            

            //assign all of the variables from the UI
            ProfileManager.playerName = nameInputField.text;
            ProfileManager.gameDifficulty = Mathf.RoundToInt(gameDifficultySlider.value);
            ProfileManager.bikingDifficulty = Mathf.RoundToInt(bikeDifficultySlider.value);
            ProfileManager.handDominance = handDominanceMenu.value;
            ProfileManager.attentionSpan = Mathf.RoundToInt(attentionSpanSlider.value);

            ProfileManager.SavePlayerSettings();
        }

        if (!saving)
        {
            ProfileManager.GetPlayerSettings();

            //TODO Write code to update the UI elements
            nameInputField.text = ProfileManager.playerName;
            gameDifficultySlider.value = ProfileManager.gameDifficulty;
            bikeDifficultySlider.value = ProfileManager.bikingDifficulty;
            handDominanceMenu.value = ProfileManager.handDominance;
            attentionSpanSlider.value = ProfileManager.attentionSpan;
        }

    }

    public void UpdateProfileNumber()
    {
        ProfileManager.profileNumber = profileNumberDropdown.value;
    }
}
