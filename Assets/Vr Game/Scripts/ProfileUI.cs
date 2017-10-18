using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
