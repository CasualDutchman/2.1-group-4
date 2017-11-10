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
            ProfileManager.profileManager.SetPlayerName(nameInputField.text);
            ProfileManager.profileManager.SetGameDifficulty(Mathf.RoundToInt(gameDifficultySlider.value));
            ProfileManager.profileManager.SetBikingDifficulty(Mathf.RoundToInt(bikeDifficultySlider.value));
            ProfileManager.profileManager.SetHandDominance(handDominanceMenu.value);
            ProfileManager.profileManager.SetAttentionSpan(attentionSpanSlider.value);

            ProfileManager.profileManager.SavePlayerSettings();
        }

        if (!saving)
        {
            ProfileManager.profileManager.GetPlayerSettings();

            //TODO Write code to update the UI elements
            nameInputField.text = ProfileManager.profileManager.GetPlayerName();
            gameDifficultySlider.value = ProfileManager.profileManager.GetGameDifficulty();
            bikeDifficultySlider.value = ProfileManager.profileManager.GetBikingDifficulty();
            handDominanceMenu.value = ProfileManager.profileManager.GetHandDominance();
            attentionSpanSlider.value = ProfileManager.profileManager.GetAttentionSpan();
        }

    }

    public void UpdateProfileNumber()
    {
        ProfileManager.profileManager.SetProfileNumber(profileNumberDropdown.value);
    }
}
