using System.Collections.Generic;
using UnityEngine;

namespace ScriptsCore.Editor {

    public class QuickAccessWindowOptions : ScriptableObject
    {
        public bool GetScenesFromBuildSettings = true;

        public List<ObjectGroup> Assets;


        public void CloneValues(QuickAccessWindowOptions target)
        {
            target.GetScenesFromBuildSettings = GetScenesFromBuildSettings;
            target.Assets = Assets;
        }
        
        /*[InfoBox("<size=13><b>QuickAccess window documentation : </b>" +
                 "\n  This window allow to have shortcuts for your assets or folders" +
                 "\n\n<b>Adding Assets to the window</b>" +
                 "\n  1) Right click one or multiples assets in you project window, and click on \"Add to QuickAccess\"" +
                 "\n  2) Same as above, but with \"Add To QuickAccess in new group\", this will add in a new group" +
                 "\n  3) You can also manually go to the options assets by clicking the Options button (this documentation is already in the option, so you are here) and manualy add groups and assets" +
                 "\n\n\n<b>Using the window : </b>" +
                 "\n  Left-click : Open the asset" +
                 "\n  Right-Click : Ping the asset in the project \n    - For scenes, it also remove the scene if it is additive" +
                 "\n  Middle-Click : Instantiate the object or scene \n    - If it can't be instantiated, it just select the object" +
                 "\n\n<b>Shortcuts : </b>" +
                 "\n  Option + Left/Right click : Move the selected asset up/down in the group" +
                 "\n  Command + Left/Right click : Move the selected asset in the next/previous group" +
                 "\n  Option/Command + Middle click : Remove the selected asset</size>")]
        [HideLabel]
        [DisplayAsString]
        public string UselessParameter = "";*/
    }

}