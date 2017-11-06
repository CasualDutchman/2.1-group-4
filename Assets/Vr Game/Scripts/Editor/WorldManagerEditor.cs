using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(WorldManager))]
public class WorldManagerEditor : Editor {

    bool editWorlds;
    bool editWorldTypes;

    int expanded = 0;

    bool pressedRemove;

    Rect buttonRect;
    WorldManager manager;

    void OnEnable() {
        manager = (WorldManager)target;
        manager.transform.hideFlags = HideFlags.None;
    }

    public override void OnInspectorGUI() {
        //using brackets at some points to make it clear for others and myself when a BeginHorizontal() or BeginVertical() begins or ends
        //everytime a Begin(Horizontal/Vertical)() starts, there will be a comment to show what is in there

        
        GUILayout.BeginHorizontal(); //buttons to select which you want to edit
        {
            if (GUILayout.Button(editWorlds ? "Hide Worlds" : "Show Worlds"))
                ToggleEditWorlds();

            if (GUILayout.Button(editWorldTypes ? "Hide World types" : "Show World types"))
                ToggleEditWorldTypes();
        }
        GUILayout.EndHorizontal();

        if(editWorlds || editWorldTypes)
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        #region editWorlds
        if (editWorlds) {
            GUILayout.BeginHorizontal(); // displays the label 'Worlds' and a button '+' to add a world
            {
                EditorGUILayout.LabelField(new GUIContent("Worlds", "A world contains the worldtype for spawning items, a skybox for the world's skybox (also portals), allowing certain gamemodes will make for interesting worlds"), EditorStyles.boldLabel);
                if (GUILayout.Button(new GUIContent("+", "Add a World"), EditorStyles.miniButton)) {
                    Undo.RecordObject(manager, "add world");
                    manager.AddWorld();
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical(); // displays a list off worlds to edit
            {
                for (int i = 0; i < manager.worldList.Count; i++) {
                    GUILayout.BeginVertical("", EditorStyles.helpBox); // displays the box where a world can be edited
                    {
                        GUILayout.BeginHorizontal(); // displays the editable label for the name of the world and a button to hide or expand a world to edit it
                        {
                            EditorGUI.BeginChangeCheck();
                            string tempstr = EditorGUILayout.TextField(manager.worldList[i].name, EditorStyles.boldLabel);
                            if (EditorGUI.EndChangeCheck()) {
                                Undo.RecordObject(manager, "change world name");
                                manager.worldList[i].name = tempstr;
                            }

                            if (GUILayout.Button(new GUIContent(expanded == i ? "Hide" : "Expand", "Expand or hide the World information"), EditorStyles.miniButtonLeft)) {
                                expanded = expanded == i ? -1 : i;
                            }

                            pressedRemove = GUILayout.Button(new GUIContent("-", "Remove World"), EditorStyles.miniButtonRight);
                        }
                        GUILayout.EndHorizontal();
                        EditorGUILayout.Space();

                        if (expanded == i) {

                            GUILayout.BeginHorizontal(); // displays the label 'Type' and a popup to select a worldType
                            {
                                string[] options = new string[manager.worldTypes.Count];
                                for (int j = 0; j < manager.worldTypes.Count; j++) {
                                    options[j] = manager.worldTypes[j].name;
                                }
                                EditorGUILayout.LabelField(new GUIContent("Type", "The world's type for spawning certain objects"));

                                EditorGUI.BeginChangeCheck();
                                int changeworldtypeindex = EditorGUILayout.Popup(manager.worldList[i].typeID, options, EditorStyles.popup);
                                if (EditorGUI.EndChangeCheck()) {
                                    Undo.RecordObject(manager, "changed worldtype");
                                    manager.worldList[i].typeID = changeworldtypeindex;
                                    manager.worldList[i].worldType = manager.worldTypes[manager.worldList[i].typeID];
                                }
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal(); // displays the label 'Sun Color' and a Colorfield to pick a color for the sun
                            {
                                EditorGUILayout.LabelField(new GUIContent("Sun Color", "The color of the world's sun"));

                                EditorGUI.BeginChangeCheck();
                                Color tempcol = EditorGUILayout.ColorField(manager.worldList[i].sunColor);
                                if (EditorGUI.EndChangeCheck()) {
                                    Undo.RecordObject(manager, "changed sun color");
                                    manager.worldList[i].sunColor = tempcol;
                                }
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal(); // displays the label 'World Texturemap' and a ObjectField to select a texture for the world to use and paint all the spawned items
                            {
                                EditorGUILayout.LabelField(new GUIContent("World Texturemap", "The texturemap used to color the world"));

                                EditorGUI.BeginChangeCheck();
                                Texture2D tex = (Texture2D)EditorGUILayout.ObjectField(manager.worldList[i].texturemap, typeof(Texture2D), false, GUILayout.ExpandWidth(false));
                                if (EditorGUI.EndChangeCheck()) {
                                    Undo.RecordObject(manager, "changed texturemap");
                                    manager.worldList[i].texturemap = tex;
                                }
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal(); // displays the label 'Gamemodes' and a popupwindow to display all the scripts from the gamemode's folder that can be used for gamemodes
                            {
                                EditorGUILayout.LabelField("   ", GUILayout.ExpandWidth(false));
                                if (GUILayout.Button(new GUIContent("Change texture", "Change the World Texturemap, every color matches the color of the current texturemap and changes it"), EditorStyles.miniButton)) {
                                    PopupWindow.Show(buttonRect, new PopupColors(manager, i));
                                }
                                if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal(); // displays the label 'Gamemodes' and a popupwindow to display all the scripts from the gamemode's folder that can be used for gamemodes
                            {
                                EditorGUILayout.LabelField(new GUIContent("Gamemodes", "Select which gamemodes are able to be played for this world"), GUILayout.ExpandWidth(false));
                                if (GUILayout.Button(manager.worldList[i].availableGamemodes.Count + " selected", EditorStyles.miniButton)) {
                                    PopupWindow.Show(buttonRect, new PopupGamemode(manager, i));
                                }
                                if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndVertical();
                    if (pressedRemove) { // this happens when the buttom for removing a world is pressed, when call earlier it displayed a nullpointerexception
                        Undo.RecordObject(manager, "remove world");
                        manager.RemoveWorld(i);
                    }
                }
            }
            GUILayout.EndVertical();
        }
        #endregion

        #region editWorldtypes
        if (editWorldTypes) {
            GUILayout.BeginHorizontal(); // displays the label 'Worldtypes' and a button '+' to add a worldtype
            {
                EditorGUILayout.LabelField(new GUIContent("Worldtypes", "Worldtypes hold all the information about the spawnable items"), EditorStyles.boldLabel);
                if (GUILayout.Button(new GUIContent("+", "Add WorldType"), EditorStyles.miniButton)) {
                    Undo.RecordObject(manager, "add worldtype");
                    manager.AddWorldType();
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical(); // displays a list off worlds to edit
            {
                for (int i = 0; i < manager.worldTypes.Count; i++) {
                    GUILayout.BeginVertical("", EditorStyles.helpBox); // displays the box where a world can be edited
                    {
                        GUILayout.BeginHorizontal(); // displays the editable label for the name of the world and a button to hide or expand a world to edit it
                        {
                            EditorGUI.BeginChangeCheck();
                            string tempstr = EditorGUILayout.TextField(manager.worldTypes[i].name, EditorStyles.boldLabel);
                            if (EditorGUI.EndChangeCheck()) {
                                Undo.RecordObject(manager, "change worldtype name");
                                manager.worldTypes[i].name = tempstr;
                            }

                            if (GUILayout.Button(new GUIContent(expanded == i ? "Hide" : "Expand", "Expand or hide the Worldtype information"), EditorStyles.miniButtonLeft)) {
                                expanded = expanded == i ? -1 : i;
                            }
                            bool pressedRemove = GUILayout.Button("-", EditorStyles.miniButtonRight);
                        }
                        GUILayout.EndHorizontal();
                        EditorGUILayout.Space();

                        if (expanded == i) {
                            if (!manager.worldTypes[i].hasTrees && manager.worldTypes[i].hasRocks) {
                                GUILayout.BeginHorizontal(); // displays the label and a toggle to toggle if spawnpoint for a tree can be used for rocks
                                {
                                    EditorGUILayout.LabelField("Place rocks on tree spawn points");

                                    EditorGUI.BeginChangeCheck();
                                    bool tempb = EditorGUILayout.Toggle("", manager.worldTypes[i].treeSpawnAsRock);
                                    if (EditorGUI.EndChangeCheck()) {
                                        Undo.RecordObject(manager, "worldtype - change pref");
                                        manager.worldTypes[i].treeSpawnAsRock = tempb;
                                    }
                                }
                                GUILayout.EndHorizontal();
                            }

                            //trees
                            GUILayout.BeginVertical();
                            {
                                GUILayout.BeginVertical(); //displays a label and starts the togglegroup, everything in this group can be shown or not, based on the toggle
                                {
                                    EditorGUI.BeginChangeCheck();
                                    bool tempb1 = EditorGUILayout.BeginToggleGroup("Trees", manager.worldTypes[i].hasTrees);
                                    if (EditorGUI.EndChangeCheck()) {
                                        Undo.RecordObject(manager, "worldtype - add trees");
                                        manager.worldTypes[i].hasTrees = tempb1;
                                    }

                                    if (manager.worldTypes[i].hasTrees) {
                                        GUILayout.BeginHorizontal(); // displays a list of objects and buttons to add to or remove from the list
                                        {
                                            GUILayout.BeginVertical(); // displays a list to place trees
                                            {
                                                for (int j = 0; j < manager.worldTypes[i].trees.Count; j++) {
                                                    EditorGUI.BeginChangeCheck();
                                                    GameObject tempgo1 = (GameObject)EditorGUILayout.ObjectField(manager.worldTypes[i].trees[j], typeof(GameObject), false);
                                                    if (EditorGUI.EndChangeCheck()) {
                                                        Undo.RecordObject(manager, "worldtype - add trees");
                                                        manager.worldTypes[i].trees[j] = tempgo1;
                                                    }
                                                }
                                            }
                                            GUILayout.EndVertical();

                                            //buttons
                                            if (GUILayout.Button(new GUIContent("+", "Add to list"), EditorStyles.miniButtonLeft, GUILayout.MaxWidth(20)))
                                                if (manager.worldTypes[i].trees.Count < 10) {
                                                    Undo.RecordObject(manager, "worldtype - add tree");
                                                    manager.worldTypes[i].trees.Add(null);
                                                }
                                            if (GUILayout.Button(new GUIContent("-", "Remove last"), EditorStyles.miniButtonRight, GUILayout.MaxWidth(20))) {
                                                if (manager.worldTypes[i].trees.Count > 1) {
                                                    Undo.RecordObject(manager, "worldtype - remove tree");
                                                    manager.worldTypes[i].trees.RemoveAt(manager.worldTypes[i].trees.Count - 1);
                                                }
                                            }
                                        }
                                        GUILayout.EndHorizontal();
                                        EditorGUILayout.Space();
                                    }
                                    EditorGUILayout.EndToggleGroup(); // ends the togglegroup
                                }
                                GUILayout.EndVertical();
                                //end trees
                                //bushes
                                GUILayout.BeginVertical(); //displays a label and starts the togglegroup, everything in this group can be shown or not, based on the toggle
                                {
                                    EditorGUI.BeginChangeCheck();
                                    bool tempb2 = EditorGUILayout.BeginToggleGroup("Bushes", manager.worldTypes[i].hasBushes);
                                    if (EditorGUI.EndChangeCheck()) {
                                        Undo.RecordObject(manager, "worldtype - add bush");
                                        manager.worldTypes[i].hasBushes = tempb2;
                                    }

                                    if (manager.worldTypes[i].hasBushes) {
                                        GUILayout.BeginHorizontal(); // displays a list of objects and buttons to add to or remove from the list
                                        {
                                            GUILayout.BeginVertical(); // displays a list to place rocks
                                            {
                                                for (int j = 0; j < manager.worldTypes[i].bushes.Count; j++) {
                                                    EditorGUI.BeginChangeCheck();
                                                    GameObject tempgo1 = (GameObject)EditorGUILayout.ObjectField(manager.worldTypes[i].bushes[j], typeof(GameObject), false);
                                                    if (EditorGUI.EndChangeCheck()) {
                                                        Undo.RecordObject(manager, "worldtype - add bush");
                                                        manager.worldTypes[i].bushes[j] = tempgo1;
                                                    }
                                                }
                                            }
                                            GUILayout.EndVertical();

                                            //buttons
                                            if (GUILayout.Button(new GUIContent("+", "Add to list"), EditorStyles.miniButtonLeft, GUILayout.MaxWidth(20)))
                                                if (manager.worldTypes[i].bushes.Count < 10) {
                                                    Undo.RecordObject(manager, "worldtype - add bush");
                                                    manager.worldTypes[i].bushes.Add(null);
                                                }
                                            if (GUILayout.Button(new GUIContent("-", "Remove last"), EditorStyles.miniButtonRight, GUILayout.MaxWidth(20))) {
                                                if (manager.worldTypes[i].bushes.Count > 1) {
                                                    Undo.RecordObject(manager, "worldtype - remove bush");
                                                    manager.worldTypes[i].bushes.RemoveAt(manager.worldTypes[i].bushes.Count - 1);
                                                }
                                            }
                                        }
                                        GUILayout.EndHorizontal();
                                        EditorGUILayout.Space();
                                    }
                                    EditorGUILayout.EndToggleGroup(); // ends the togglegroup
                                }
                                GUILayout.EndVertical();
                                //end bushes
                                //grass
                                GUILayout.BeginVertical(); //displays a label and starts the togglegroup, everything in this group can be shown or not, based on the toggle
                                {
                                    EditorGUI.BeginChangeCheck();
                                    bool tempb2 = EditorGUILayout.BeginToggleGroup("Grass", manager.worldTypes[i].hasGrass);
                                    if (EditorGUI.EndChangeCheck()) {
                                        Undo.RecordObject(manager, "worldtype - add grass");
                                        manager.worldTypes[i].hasGrass = tempb2;
                                    }

                                    if (manager.worldTypes[i].hasGrass) {
                                        GUILayout.BeginHorizontal(); // displays a list of objects and buttons to add to or remove from the list
                                        {
                                            GUILayout.BeginVertical(); // displays a list to place rocks
                                            {
                                                for (int j = 0; j < manager.worldTypes[i].grass.Count; j++) {
                                                    EditorGUI.BeginChangeCheck();
                                                    GameObject tempgo1 = (GameObject)EditorGUILayout.ObjectField(manager.worldTypes[i].grass[j], typeof(GameObject), false);
                                                    if (EditorGUI.EndChangeCheck()) {
                                                        Undo.RecordObject(manager, "worldtype - add grass");
                                                        manager.worldTypes[i].grass[j] = tempgo1;
                                                    }
                                                }
                                            }
                                            GUILayout.EndVertical();

                                            //buttons
                                            if (GUILayout.Button(new GUIContent("+", "Add to list"), EditorStyles.miniButtonLeft, GUILayout.MaxWidth(20)))
                                                if (manager.worldTypes[i].grass.Count < 10) {
                                                    Undo.RecordObject(manager, "worldtype - add rock");
                                                    manager.worldTypes[i].grass.Add(null);
                                                }
                                            if (GUILayout.Button(new GUIContent("-", "Remove last"), EditorStyles.miniButtonRight, GUILayout.MaxWidth(20))) {
                                                if (manager.worldTypes[i].grass.Count > 1) {
                                                    Undo.RecordObject(manager, "worldtype - remove rock");
                                                    manager.worldTypes[i].grass.RemoveAt(manager.worldTypes[i].grass.Count - 1);
                                                }
                                            }
                                        }
                                        GUILayout.EndHorizontal();
                                        EditorGUILayout.Space();
                                    }
                                    EditorGUILayout.EndToggleGroup(); // ends the togglegroup
                                }
                                GUILayout.EndVertical();
                                //end grass
                                //rocks
                                GUILayout.BeginVertical(); //displays a label and starts the togglegroup, everything in this group can be shown or not, based on the toggle
                                {
                                    EditorGUI.BeginChangeCheck();
                                    bool tempb2 = EditorGUILayout.BeginToggleGroup("Rocks", manager.worldTypes[i].hasRocks);
                                    if (EditorGUI.EndChangeCheck()) {
                                        Undo.RecordObject(manager, "worldtype - add rocks");
                                        manager.worldTypes[i].hasRocks = tempb2;
                                    }

                                    if (manager.worldTypes[i].hasRocks) {
                                        GUILayout.BeginHorizontal(); // displays a list of objects and buttons to add to or remove from the list
                                        {
                                            GUILayout.BeginVertical(); // displays a list to place rocks
                                            {
                                                for (int j = 0; j < manager.worldTypes[i].rocks.Count; j++) {
                                                    EditorGUI.BeginChangeCheck();
                                                    GameObject tempgo1 = (GameObject)EditorGUILayout.ObjectField(manager.worldTypes[i].rocks[j], typeof(GameObject), false);
                                                    if (EditorGUI.EndChangeCheck()) {
                                                        Undo.RecordObject(manager, "worldtype - add rocks");
                                                        manager.worldTypes[i].rocks[j] = tempgo1;
                                                    }
                                                }
                                            }
                                            GUILayout.EndVertical();

                                            //buttons
                                            if (GUILayout.Button(new GUIContent("+", "Add to list"), EditorStyles.miniButtonLeft, GUILayout.MaxWidth(20)))
                                                if (manager.worldTypes[i].rocks.Count < 10) {
                                                    Undo.RecordObject(manager, "worldtype - add rock");
                                                    manager.worldTypes[i].rocks.Add(null);
                                                }
                                            if (GUILayout.Button(new GUIContent("-", "Remove last"), EditorStyles.miniButtonRight, GUILayout.MaxWidth(20))) {
                                                if (manager.worldTypes[i].rocks.Count > 1) {
                                                    Undo.RecordObject(manager, "worldtype - remove rock");
                                                    manager.worldTypes[i].rocks.RemoveAt(manager.worldTypes[i].rocks.Count - 1);
                                                }
                                            }
                                        }
                                        GUILayout.EndHorizontal();
                                        EditorGUILayout.Space();
                                    }
                                    EditorGUILayout.EndToggleGroup(); // ends the togglegroup
                                }
                                GUILayout.EndVertical();
                                //end rocks
                                //buildings
                                GUILayout.BeginVertical(); //displays a label and starts the togglegroup, everything in this group can be shown or not, based on the toggle
                                {
                                    EditorGUI.BeginChangeCheck();
                                    bool tempb3 = EditorGUILayout.BeginToggleGroup("Buildings", manager.worldTypes[i].hasBuildings);
                                    if (EditorGUI.EndChangeCheck()) {
                                        Undo.RecordObject(manager, "worldtype - add building");
                                        manager.worldTypes[i].hasBuildings = tempb3;
                                    }

                                    if (manager.worldTypes[i].hasBuildings) {
                                        GUILayout.BeginHorizontal(); // displays a list of objects and buttons to add to or remove from the list
                                        {
                                            GUILayout.BeginVertical(); // displays a list to place buildings
                                            {
                                                for (int j = 0; j < manager.worldTypes[i].buildings.Count; j++) {
                                                    EditorGUI.BeginChangeCheck();
                                                    GameObject tempgo1 = (GameObject)EditorGUILayout.ObjectField(manager.worldTypes[i].buildings[j], typeof(GameObject), false);
                                                    if (EditorGUI.EndChangeCheck()) {
                                                        Undo.RecordObject(manager, "worldtype - add buildings");
                                                        manager.worldTypes[i].buildings[j] = tempgo1;
                                                    }
                                                }
                                            }
                                            GUILayout.EndVertical();

                                            //buttons
                                            if (GUILayout.Button(new GUIContent("+", "Add to list"), EditorStyles.miniButtonLeft, GUILayout.MaxWidth(20)))
                                                if (manager.worldTypes[i].buildings.Count < 10) {
                                                    Undo.RecordObject(manager, "worldtype - add building");
                                                    manager.worldTypes[i].buildings.Add(null);
                                                }
                                            if (GUILayout.Button(new GUIContent("-", "Remove last"), EditorStyles.miniButtonRight, GUILayout.MaxWidth(20))) {
                                                if (manager.worldTypes[i].buildings.Count > 1) {
                                                    Undo.RecordObject(manager, "worldtype - remove building");
                                                    manager.worldTypes[i].buildings.RemoveAt(manager.worldTypes[i].buildings.Count - 1);
                                                }
                                            }
                                        }
                                        GUILayout.EndHorizontal();
                                        EditorGUILayout.Space();
                                    }
                                    EditorGUILayout.EndToggleGroup(); // ends the togglegroup
                                }
                                GUILayout.EndVertical();
                                //end buildings
                                //panels
                                if (manager.worldTypes[i].groundPanels.Count > 0) { // notice that the groundplanes do not have a togglegroup, there is always 1 gorundplane needed, otherwise errors will occur
                                    GUILayout.BeginVertical();
                                    {
                                        EditorGUILayout.LabelField("Panels", EditorStyles.boldLabel);
                                        GUILayout.BeginHorizontal(); // displays a list of objects and buttons to add to or remove from the list
                                        {
                                            GUILayout.BeginVertical(); // displays a list to place groundplanes
                                            {
                                                for (int j = 0; j < manager.worldTypes[i].groundPanels.Count; j++) {
                                                    EditorGUI.BeginChangeCheck();
                                                    GameObject tempgo1 = (GameObject)EditorGUILayout.ObjectField(manager.worldTypes[i].groundPanels[j], typeof(GameObject), false);
                                                    if (EditorGUI.EndChangeCheck()) {
                                                        Undo.RecordObject(manager, "worldtype - add panel");
                                                        manager.worldTypes[i].groundPanels[j] = tempgo1;
                                                    }
                                                }
                                            }
                                            GUILayout.EndVertical();

                                            //buttons
                                            if (GUILayout.Button(new GUIContent("+", "Add to list"), EditorStyles.miniButtonLeft, GUILayout.MaxWidth(20)))
                                                if (manager.worldTypes[i].groundPanels.Count < 10) {
                                                    Undo.RecordObject(manager, "worldtype - add panel");
                                                    manager.worldTypes[i].groundPanels.Add(null);
                                                }
                                            if (GUILayout.Button(new GUIContent("-", "Remove last"), EditorStyles.miniButtonRight, GUILayout.MaxWidth(20))) {
                                                if (manager.worldTypes[i].groundPanels.Count > 1) {
                                                    Undo.RecordObject(manager, "worldtype - remove panel");
                                                    manager.worldTypes[i].groundPanels.RemoveAt(manager.worldTypes[i].groundPanels.Count - 1);
                                                }
                                            }
                                        }
                                        GUILayout.EndHorizontal();
                                        EditorGUILayout.Space();
                                        EditorGUILayout.EndToggleGroup();
                                    }
                                    //end panels
                                }
                                GUILayout.EndVertical();
                                //add a not for the designers how the panel works
                                EditorGUILayout.HelpBox("Note: The panel's children are spawnlocations.\nIf you don't have those, the game might not work as intended.\nPress the gear in the top right, press: Create Example, for example in scene", MessageType.Warning);
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    if (pressedRemove) { // this happens when the buttom for removing a world is pressed, when call earlier it displayed a nullpointerexception
                        Undo.RecordObject(manager, "remove worldtype");
                        manager.RemoveWorldType(i);
                        break;
                    }
                }
            }
            GUILayout.EndVertical();
        }
        #endregion

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); // line


        // following will display sliders, selectors or inputs for multiple public variables
        EditorGUILayout.LabelField("Variables", EditorStyles.boldLabel);

        string[] names = new string[manager.worldList.Count];
        int[] sizes = new int[manager.worldList.Count];
        for (int i = 0; i < manager.worldList.Count; i++) {
            names[i] = manager.worldList[i].name;
            sizes[i] = i;
        }
        manager.startWorld = EditorGUILayout.IntPopup("Start World ", manager.startWorld, names, sizes); //which world will be started with

        manager.speed = EditorGUILayout.Slider("Speed", manager.speed, 1.0f, 25.0f); //the speed of the bike

        manager.amountOfPanels = Mathf.FloorToInt(EditorGUILayout.Slider("Max Panels", manager.amountOfPanels, 6, 35.0f)); //maximum panels allowed in the world

        manager.percentageSpawn = EditorGUILayout.Slider("World Spawnrates", manager.percentageSpawn, 0.0f, 1.0f); //the speed of the bike

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //line

        //display an error message when there is no world and/or worldtype. At least 1 ofeach is needed
        string message = "";
        if(manager.worldList.Count == 0)
            message += "There are 0 worlds, ";
        if (manager.worldTypes.Count == 0)
            message += "There are 0 worldtypes";
        if(message.Length > 0)
            EditorGUILayout.HelpBox(message, MessageType.Error);

        //displays an error when one or multiple worldtypes have no groundplane
        bool errorNoPanel = false;
        foreach(WorldType type in manager.worldTypes) {
            if(type.groundPanels.Count == 0 || type.groundPanels[0] == null)
                errorNoPanel = true;
        }
        if (errorNoPanel)
            EditorGUILayout.HelpBox("One or multiple Worldtypes do not have a panel", MessageType.Error);

        //shows warning when playing/testing in the editor
        if (Application.isEditor && Application.isPlaying)
            EditorGUILayout.HelpBox("We are playing!\nAny change made will not be saved after stopping!", MessageType.Warning);
    }

    void ToggleEditWorlds() {
        editWorlds = !editWorlds;
        editWorldTypes = false;
        expanded = -1;
    }

    void ToggleEditWorldTypes() {
        editWorldTypes = !editWorldTypes;
        editWorlds = false;
        expanded = -1;
    }
}

public class PopupGamemode : PopupWindowContent {
    WorldManager manager;
    int i;

    List<string> scripts = new List<string>();

    public PopupGamemode(WorldManager m, int index) {
        manager = m;
        i = index;
    }

    public override Vector2 GetWindowSize() {
        return new Vector2(200, scripts.Count * 18.5f);
    }

    //displays all the scripts in the gamemode's folder. it can be selected and deselected in the popupmenu
    public override void OnGUI(Rect rect) {
        for (int j = 0; j < scripts.Count; j++) {
            GUILayout.BeginHorizontal();
            bool toggle = EditorGUILayout.Toggle(manager.worldList[i].availableGamemodes.Contains(scripts[j]), GUILayout.Width(20));
            EditorGUILayout.LabelField(scripts[j]);
            if (toggle) {
                if (!manager.worldList[i].availableGamemodes.Contains(scripts[j])) {
                    Undo.RecordObject(manager, "Change world Gamemode");
                    manager.worldList[i].availableGamemodes.Add(scripts[j]);
                }
            } else if (manager.worldList[i].availableGamemodes.Contains(scripts[j])) {
                Undo.RecordObject(manager, "Change world Gamemode");
                manager.worldList[i].availableGamemodes.Remove(scripts[j]);
            }
            GUILayout.EndHorizontal();
        }
    }

    //get the scripts from the folder when the window is opened
    public override void OnOpen() {
        DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/Vr Game/Scripts/Gamemodes");
        FileInfo[] fileInfo = info.GetFiles();
        
        foreach (FileInfo file in fileInfo) {
            if (file.Name.EndsWith("meta"))
                continue;

            scripts.Add(file.Name.Split('.')[0]);
        }
    }
}

public class PopupColors : PopupWindowContent {
    WorldManager manager;
    int i;

    public List<Color> testerColor = new List<Color>();

    public PopupColors(WorldManager m, int index) {
        manager = m;
        i = index;
    }

    public override Vector2 GetWindowSize() {
        return new Vector2(200, 200);
    }

    //displays all the scripts in the gamemode's folder. it can be selected and deselected in the popupmenu
    public override void OnGUI(Rect rect) {
        GUILayout.BeginVertical();
        for (int y = 0; y < 8; y++) {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < 8; x++) {
                testerColor[((7-y) * 8) + x] = EditorGUILayout.ColorField(new GUIContent(""), testerColor[((7 - y) * 8) + x], false, false, false, new ColorPickerHDRConfig(255, 255, 255, 255), GUILayout.Width(20));
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    //get the scripts from the folder when the window is opened
    public override void OnOpen() {
        for (int y = 0; y < 8; y++) {
            for (int x = 0; x < 8; x++) {
                testerColor.Add(manager.worldList[i].texturemap.GetPixel(x, y));
            }
        }
        for (int k = 0; k < 8 * 8; k++) {
        }
    }

    public override void OnClose() {
        Texture2D tex = new Texture2D(8, 8, TextureFormat.RGBA32, false);
        tex.name = "BOOH";
        tex.SetPixels(testerColor.ToArray());
        tex.filterMode = FilterMode.Point;
        tex.Apply();

        manager.worldList[i].texturemap = tex;
    }
}
