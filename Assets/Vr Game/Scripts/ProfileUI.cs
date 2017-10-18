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

    //Updates the data in the variables and the UI elements
    public void UpdatePreferences(bool saving)
    {
        if (saving)
        {
            //assign all of the variables from the UI
            ProfileManager.SetPlayerName(nameInputField.text);
            ProfileManager.SetGameDifficulty(Mathf.RoundToInt(gameDifficultySlider.value));
            ProfileManager.SetBikingDifficulty(Mathf.RoundToInt(bikeDifficultySlider.value));
            ProfileManager.SetHandDominance(handDominanceMenu.value);
            ProfileManager.SetAttentionSpan(Mathf.RoundToInt(attentionSpanSlider.value));

            ProfileManager.SavePlayerSettings();
        }

        if (!saving)
        {
            ProfileManager.GetPlayerSettings();

            //TODO Write code to update the UI elements
            nameInputField.text = ProfileManager.GetPlayerName();
            gameDifficultySlider.value = ProfileManager.GetGameDifficulty();
            bikeDifficultySlider.value = ProfileManager.GetBikingDifficulty();
            handDominanceMenu.value = ProfileManager.GetHandDominance();
            attentionSpanSlider.value = ProfileManager.GetAttentionSpan();
        }

    }

    public void UpdateProfileNumber()
    {
        ProfileManager.SetProfileNumber(profileNumberDropdown.value);
    }
}
