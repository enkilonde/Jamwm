using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ScriptsCore.CustomTools;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ScriptsCore.Editor {
    [InitializeOnLoad]
    public class QuickAccessWindow : EditorWindow {

        private QuickAccessWindowOptions _options;
        private QuickAccessSecondaryOptions _secondaryOptions;
        private Dictionary<string, List<OpenableInfo>> _assetsSorted = new Dictionary<string, List<OpenableInfo>>();
        private List<string> _AllAssetsDisplayed = new List<string>();
        private Vector2 _scrollPos;
        private const string BuildSettingsScenesGroup = "Build scenes";
        private int LockProjectWindowTimer = 0;

        public OpenableInfo DraggetObject;
        private bool DroppedObjectThisFrame;
        private Vector2 mouseDragPos;
        public int EditingGroup = -1;
        private string tempGroupName;
        private const int _editButtonLenght = 19;
        private bool _isScrollbarVisible = false;
        private const int _scrollbarWidth = 19;

        private bool isEditing = false;


        private enum Tabs {

            Private,
            Shared

        }

        private Tabs currentTab;
        private bool _isDragging;

/*
        [MenuItem("Assets/Add to QuickAccess")]
        public static void AddToQuickAccessCM() {
            foreach (Object obj in Selection.objects) {
                if (obj is GameObject) {
                    string sceneName = ((GameObject)obj).scene.name;
                    if (!string.IsNullOrEmpty(sceneName))
                        continue;
                }
                AddObject(obj);
            }
            GetWindow().Save();
        }

        [MenuItem("Assets/Add to QuickAccess in new Group")]
        public static void AddToQuickAccessNewCM() {
            QuickAccessWindow window = QuickAccessWindow.GetWindow();

            if (window._options.Assets == null)
                window._options.Assets = new List<ObjectGroup>();
            int groupToAdd = window._options.Assets.Count;
            foreach (Object obj in Selection.objects) {
                if (obj is GameObject) {
                    string sceneName = ((GameObject)obj).scene.name;
                    if (!string.IsNullOrEmpty(sceneName))
                        continue;
                }


                AddObject(obj, groupToAdd);
            }
            window.Save();
        }*/

        private static void AddObject(Object target, int groupToAdd = 0) {
            QuickAccessWindow window = GetWindow();

            if (window._options.Assets == null)
                window._options.Assets = new List<ObjectGroup>();

            while (window._options.Assets.Count < groupToAdd + 1) {
                window._options.Assets.Add(new ObjectGroup("Group " + window._options.Assets.Count, Color.white));
                window._options.Assets.Last().Assets = new List<Object>();
            }
            window._options.Assets[groupToAdd].Assets.Add(target);
        }

        public void Save() {
            EditorUtility.SetDirty(_options);
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Tools/QuickAccessWindow")]
        internal static QuickAccessWindow GetWindow() {
            return GetWindow(typeof(QuickAccessWindow), false, "QuickAccess") as QuickAccessWindow;
        }

        public void SetProjectLock(bool isLocked) {
            if (isLocked) {
                LockProjectWindowTimer = 3; //Lock the project window for 3 'frames'
            }
            Type projectBrowserType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
            object projectBrowser = GetWindow(projectBrowserType);
            object value = isLocked;

            var property = projectBrowserType.GetProperty("isLocked",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            property.SetValue(projectBrowser, value);
        }

        private void OnEnable() {
            EditingGroup = -1;
            if (_options == null)
                GetOptions();
        }

        void GetOptions() {
            bool firstTime;
            string optionsPath;
            switch (currentTab) {
                case Tabs.Private:
                    optionsPath = "Assets/Unexported/Scriptables/QuickAccessWindowOptions";
                    break;
                case Tabs.Shared:
                    optionsPath = "Assets/Resources/Scriptables/QuickAccessWindowOptions";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            _options = ScriptableObjectTools.LoadOrCreate<QuickAccessWindowOptions>(
                optionsPath, out firstTime);
            _secondaryOptions =
                ScriptableObjectTools.LoadOrCreate<QuickAccessSecondaryOptions>(
                    "Assets/Unexported/Scriptables/QuickAccessSecondaryOptions_" + currentTab);
        }

        internal void OnGUI() {
            //GUI de la fenetre ici

            Header();

            ParseGroups();

            _isScrollbarVisible = IsScrollbarVisible();

            DrawGroups();


            if (LockProjectWindowTimer > 0) {
                LockProjectWindowTimer--;
                if (LockProjectWindowTimer == 0)
                    SetProjectLock(false);
            }

            if (Event.current.type == EventType.MouseUp) {
                DraggetObject = null;
            }
            DroppedObjectThisFrame = false;
        }

        void Header() {
            if (Application.isPlaying) {
                GUILayout.Label("Game is running, can't change scene", EditorStyles.boldLabel);
                return;
            }
            GUILayout.BeginHorizontal();

            GUILayout.Label("    Quickaccess", EditorStyles.boldLabel);

            if (GUILayout.Button(isEditing ? "Stop edit" : "Edit")) {
                isEditing = !isEditing;
            }

            if (GUILayout.Button("Options")) {
                GetOptions();
                SetProjectLock(true);
                Selection.activeObject = _options;
            }

            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();

            List<Tabs> enumsList = Enum.GetValues(typeof(Tabs)).Cast<Tabs>().ToList();
            foreach (Tabs tab in enumsList) {
                GUIStyle skin = new GUIStyle(GUI.skin.button);
                GUI.backgroundColor = tab == currentTab ? Color.grey : Color.white;
                skin.onNormal.textColor = tab == currentTab ? Color.black : Color.white;

                if (GUILayout.Button(tab.ToString(), skin)) {
                    currentTab = tab;
                    GetOptions();
                }
            }

            GUILayout.EndHorizontal();


            GUI.Box(new Rect(2, 2, 15, 20),
                new GUIContent("?",
                    "Scene switch window : \nThis window display all scenes in build\n  Commands : \n  - Left click : Load Scene.\n  - Middle click : Load scene additive.\n  - Right click : Unload scene."));
        }

        void ParseGroups() {
            _assetsSorted.Clear();
            if (_options.GetScenesFromBuildSettings) {
                for (var i = 0; i < EditorBuildSettings.scenes.Length; i++) {
                    var scene = EditorBuildSettings.scenes[i];
                    AddAsset(scene.path, BuildSettingsScenesGroup, Color.white, true);
                }
            }

            if (_options.Assets != null) {
                for (int i = 0; i < _options.Assets.Count; i++) {
                    ObjectGroup group = _options.Assets[i];
                    for (int j = 0; j < group.Assets.Count; j++) {
                        AddAsset(group.GetPath(j), group.GroupName, group.GroupColor, true);
                    }
                }
            }
            _AllAssetsDisplayed.Clear();
        }

        void DrawGroups() {
            List<string> groups = new List<string>(_assetsSorted.Keys);
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.GetStyle("Button")) { alignment = TextAnchor.MiddleLeft };

            EditorGUILayout.BeginVertical();
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            OpenableInfo currentBrowsedAsset = null;

            float height = 0;

            float StartHeight = 0;

            for (int c = 0; c < groups.Count; c++) {
                GUI.backgroundColor = Color.white;
                Rect groupNameRect = new Rect(0, height, Screen.width - (isEditing ? _editButtonLenght * 4 : 0), EditorGUIUtility.singleLineHeight);

                DrawGroupName(groupNameRect, groups, c);
                StartHeight = height;
                height += EditorGUIUtility.singleLineHeight;
                if (IsGroupFolded(c)) {
                    height += EditorGUIUtility.singleLineHeight / 2;
                    GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
                    continue;
                }
                // GUILayout.Label(groups[c], EditorStyles.boldLabel);
                for (int i = 0; i < _assetsSorted[groups[c]].Count; i++) {
                    currentBrowsedAsset = _assetsSorted[groups[c]][i];

                    if (currentBrowsedAsset == null ||
                        string.IsNullOrEmpty(currentBrowsedAsset.AssetNameParsed)) continue;

                    GUI.backgroundColor = currentBrowsedAsset.Color;

                    GUIContent content = new GUIContent(currentBrowsedAsset.AssetNameParsed, currentBrowsedAsset.Icon);

                    GUILayout.BeginHorizontal();
                    Rect assetButtonRect =
                        new Rect(0, height, Screen.width - (isEditing ? _editButtonLenght * 3 : 0) - (_isScrollbarVisible ? _scrollbarWidth : 0),
                            EditorGUIUtility.singleLineHeight);
                    CustomStartDragAsset(assetButtonRect, currentBrowsedAsset);
                    DrawAssetButton(assetButtonRect, content, buttonStyle, currentBrowsedAsset);

                    if (isEditing) DrawEditButtons(height, currentBrowsedAsset);

                    GUILayout.EndHorizontal();
                    GUILayout.Space(EditorGUIUtility.singleLineHeight);
                    height += EditorGUIUtility.singleLineHeight;
                }
                DragAndDropAsset(new Rect(0, StartHeight, Screen.width, height - StartHeight), c, groups[c]);
            }

            DragAndDropAsset(new Rect(0, height, Screen.width, Screen.height - height), groups.Count, "");
            // DragAndDropAsset(new Rect(0, height, Screen.width, Screen.height - height), groups.Count);

            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            if (Event.current.keyCode == KeyCode.A) {
                var test = DragAndDrop.objectReferences;
                Debug.Log("t");
            }

            _assetsSorted.Clear();
        }

        private bool IsScrollbarVisible() {
            float linesCounts = 4.15f;
            int groupIndex = 0;
            foreach (KeyValuePair<string, List<OpenableInfo>> keyValuePair in _assetsSorted) {
                if (IsGroupFolded(groupIndex)) {
                    linesCounts++;
                } else {
                    linesCounts += keyValuePair.Value.Count + 1; //+1 is for the group name
                }
                groupIndex += 2;
            }

            float height = linesCounts * EditorGUIUtility.singleLineHeight;
            bool isScrollVisible = height >= Screen.height;

            return isScrollVisible;
        }

        private void DrawAssetButton(Rect rect, GUIContent content, GUIStyle style, OpenableInfo Asset) {
            if (GUI.Button(rect, content, style)) {
                Asset.Open(Event.current);
                DraggetObject = null;
            }
        }

        private void DrawEditButtons(float height, OpenableInfo current) {
            EditorGUI.BeginChangeCheck();

            float start = Screen.width - (_isScrollbarVisible ? _scrollbarWidth : 0);

            if (GUI.Button(new Rect(start - _editButtonLenght * 3, height, _editButtonLenght, EditorGUIUtility.singleLineHeight), "X")) {
                RemoveObject(current);
            }

            if (GUI.Button(new Rect(start - _editButtonLenght * 2, height, _editButtonLenght, EditorGUIUtility.singleLineHeight), "▲")) {
                switch (Event.current.button) {
                    case 0:
                        MoveOpenableInsideGroup(current, -1);
                        break;
                    case 1:
                        MoveOpenableBetweenGroups(current, -1);
                        break;
                    case 2:
                        for (int i = 0; i < _assetsSorted[current.Group].Count; i++) {
                            MoveOpenableInsideGroup(current, -1);
                        }
                        break;
                }
            }

            if (GUI.Button(new Rect(start - _editButtonLenght * 1, height, _editButtonLenght, EditorGUIUtility.singleLineHeight), "▼")) {
                switch (Event.current.button) {
                    case 0:
                        MoveOpenableInsideGroup(current, 1);
                        break;
                    case 1:
                        MoveOpenableBetweenGroups(current, 1);
                        break;
                    case 2:
                        for (int i = 0; i < _assetsSorted[current.Group].Count; i++) {
                            MoveOpenableInsideGroup(current, 1);
                        }
                        break;
                }
            }
        }

        private void DrawGroupName(Rect rect, List<string> names, int groupIndex) {
            Rect realRect = new Rect(rect.x, rect.y, rect.width - (isEditing ? _scrollbarWidth : 0), rect.height);
            GUILayout.Label("");

            if (groupIndex != EditingGroup) {
                if (GUI.Button(realRect, (IsGroupFolded(groupIndex) ? "►" : "▼") + names[groupIndex], EditorStyles.boldLabel)) {
                    if (Event.current.button == 1) {
                        EditingGroup = groupIndex;
                        tempGroupName = names[groupIndex];
                    } else if (Event.current.button == 0) {
                        _secondaryOptions.FoldedGroups[groupIndex] = !_secondaryOptions.FoldedGroups[groupIndex];
                    }
                }
            } else {
                GUI.SetNextControlName("NameEdit");
                tempGroupName = GUI.TextField(rect, tempGroupName, EditorStyles.label);
                EditorGUI.FocusTextInControl("NameEdit");


                if (Event.current.isMouse || Event.current.keyCode == KeyCode.Return) {
                    if (string.IsNullOrEmpty(tempGroupName) || names.Contains(tempGroupName)) {
                        EditingGroup = -1;
                        return;
                    }
                    ObjectGroup group = _options.Assets.Find(group => group.GroupName == names[groupIndex]);
                    group.GroupName = tempGroupName;

                    List<OpenableInfo> value = _assetsSorted[names[groupIndex]];
                    _assetsSorted.Remove(names[groupIndex]);
                    _assetsSorted.Add(tempGroupName, value);

                    names[groupIndex] = tempGroupName;
                    EditingGroup = -1;
                }
            }

            if (isEditing && groupIndex > 0) {
                ObjectGroup _currentGroup = _options.Assets[groupIndex - 1];

                Color newColor = EditorGUI.ColorField(
                    new Rect(Screen.width - (_isScrollbarVisible ? _scrollbarWidth : 0) - _editButtonLenght * 1, rect.y, _editButtonLenght,
                        EditorGUIUtility.singleLineHeight), new GUIContent(""), _currentGroup.GroupColor, false, false, false);
                if (EditorGUI.EndChangeCheck()) {
                    _currentGroup.GroupColor = newColor;
                }
            }
        }

        private bool IsGroupFolded(int groupIndex) {
            while (_secondaryOptions.FoldedGroups.Count < groupIndex + 1) {
                _secondaryOptions.FoldedGroups.Add(false);
            }
            return _secondaryOptions.FoldedGroups[groupIndex];
        }

        public void DragAndDropAsset(Rect GroupRect, int groupIndex, string group) {
            Event evt = Event.current;

            if (DragAndDrop.objectReferences.Length > 0 || (DraggetObject != null && evt.mousePosition != mouseDragPos)) {
                Rect rect = new Rect(GroupRect);
                rect.height *= 0.9f;
                rect.center = GroupRect.center;
                GUIStyle style = new GUIStyle(GUI.skin.button);
                GUI.backgroundColor = new Color(0, 1, 0, 0.75f);
                string message = "Add to group";
                if (groupIndex == _assetsSorted.Keys.Count)
                    message = "New Group";
                else
                    message = "Add to group " + group;
                GUI.Box(rect, message, style);
            }
            if (DroppedObjectThisFrame)
                return;
            if (!(evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform || evt.type == EventType.MouseDrag || DraggetObject != null))
                return;

            if (!GroupRect.Contains(evt.mousePosition))
                return;

            if (DraggetObject != null && evt.type == EventType.MouseUp) {
                Debug.Log("Dragged " + DraggetObject.Asset.name);
                RemoveObject(DraggetObject);
                AddObject(DraggetObject.Asset, groupIndex - 1);
                DraggetObject = null;
                DroppedObjectThisFrame = true;
                Save();
            } else {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (evt.type == EventType.DragPerform) {
                    DragAndDrop.AcceptDrag();

                    foreach (Object dragged_object in DragAndDrop.objectReferences) {
                        // Do On Drag Stuff here
                        Debug.Log("Dragged " + dragged_object.name);
                        AddObject(dragged_object, groupIndex - 1);
                    }
                    DroppedObjectThisFrame = true;
                    Save();
                }
            }
        }

        private bool CustomStartDragAsset(Rect ObjectRect, OpenableInfo Asset) {
            return false;
            Event evt = Event.current;
            if (evt.type != EventType.MouseDown)
                return false;
            if (!ObjectRect.Contains(evt.mousePosition))
                return false;

            DraggetObject = Asset;
            mouseDragPos = evt.mousePosition;

            return true;
        }


        public void AddAsset(string assetPath, string group, Color color, bool allowDuplicates = false) {
            if (!_AllAssetsDisplayed.Contains(assetPath))
                _AllAssetsDisplayed.Add(assetPath);
            else if (!allowDuplicates)
                return;

            if (group == "")
                group = GetSceneParentFolderName(assetPath);

            OpenableInfo openable = new OpenableInfo(this, assetPath, group, color);

            if (!new List<string>(_assetsSorted.Keys).Contains(openable.Group)) {
                _assetsSorted.Add(openable.Group, new List<OpenableInfo>());
            }
            // if (_assetsSorted[openable.Group].Find(_openable => openable.AssetPath == assetPath) == null)
            _assetsSorted[openable.Group].Add(openable);
        }

        public static string GetSceneParentFolderName(string path) {
            string[] folders = path.Split('/');
            if (folders.Length >= 2) {
                string chapterFolder = folders[folders.Length - 2];
                return chapterFolder;
            }
            return "";
        }

        public void MoveOpenableInsideGroup(OpenableInfo targetToMove, int direction) {
            if (direction == 0)
                return;
            direction = (int)Mathf.Sign(direction);

            foreach (ObjectGroup optionsAsset in _options.Assets) {
                List<Object> currentGroup = optionsAsset.Assets;
                if (!currentGroup.Contains(targetToMove.Asset))
                    continue;
                int index = currentGroup.IndexOf(targetToMove.Asset);
                if (index == 0 && direction < 0)
                    return; //MoveGroups
                if (index == currentGroup.Count - 1 && direction > 0)
                    return; // MoveGroups

                Object elemToExchange = currentGroup[index + direction];
                currentGroup[index + direction] = targetToMove.Asset;
                currentGroup[index] = elemToExchange;
                break;
            }
            Save();
        }

        public void MoveOpenableBetweenGroups(OpenableInfo targetToMove, int direction) {
            if (direction == 0)
                return;
            direction = (int)Mathf.Sign(direction);

            for (var i = 0; i < _options.Assets.Count; i++) {
                List<Object> currentGroup = _options.Assets[i].Assets;
                if (!currentGroup.Contains(targetToMove.Asset))
                    continue;


                if (i == 0 && direction < 0)
                    return; //MoveToFirst
                if (i == _options.Assets.Count - 1 && direction > 0) {
                    if (_options.Assets.Last().Assets.Count > 1) {
                        currentGroup.Remove(targetToMove.Asset);
                        AddToNewGroup(targetToMove.Asset);
                    }
                    return;
                }

                currentGroup.Remove(targetToMove.Asset);
                if (direction < 0) {
                    _options.Assets[i + direction].Assets.Add(targetToMove.Asset);
                } else {
                    _options.Assets[i + direction].Assets.Insert(0, targetToMove.Asset);
                }
                break;
            }
            Save();
        }

        private void AddToNewGroup(Object targetToAdd) {
            _options.Assets.Add(new ObjectGroup("Group " + (_options.Assets.Count + 1), Color.white));
            _options.Assets.Last().Assets.Add(targetToAdd);
        }

        public void RemoveObject(OpenableInfo targetToRemove) {
            foreach (ObjectGroup optionsAsset in _options.Assets) {
                List<Object> currentGroup = optionsAsset.Assets;
                if (!currentGroup.Contains(targetToRemove.Asset) || optionsAsset.GroupName != targetToRemove.Group)
                    continue;
                currentGroup.Remove(targetToRemove.Asset);
                break;
            }
            Save();
            CleanLists();
        }

        public void CleanLists() {
            for (int i = _options.Assets.Count - 1; i >= 0; i--) {
                if (_options.Assets[i].Assets == null || _options.Assets[i].Assets.Count == 0)
                    _options.Assets.RemoveAt(i);
            }
            Save();
        }

    }


    public class OpenableInfo {

        public QuickAccessWindow Owner;
        public readonly string AssetPath;
        public readonly string AssetNameParsed;
        public readonly string Group;
        public readonly Object Asset;
        public readonly Texture Icon;
        public readonly string extension;
        public readonly Color Color;

        public OpenableInfo(QuickAccessWindow owner, string assetPath, string group, Color color) {
            this.Owner = owner;
            this.AssetPath = assetPath;
            this.AssetNameParsed = GetSceneNameFromPath();
            this.Group = group;
            this.extension = GetExtension();
            this.Asset = AssetDatabase.LoadMainAssetAtPath(AssetPath);
            ;
            this.Icon = AssetPreview.GetMiniThumbnail(Asset);
            this.Color = color;
        }

        private string GetSceneNameFromPath() {
            string[] folders = AssetPath.Split('/');
            return folders[folders.Length - 1].Split('.')[0];
        }

        private string GetExtension() {
            string[] folders = AssetPath.Split('/');
            string[] splited = folders[folders.Length - 1].Split('.');
            if (splited.Length < 2)
                return ""; //maybe it does not have an extension
            return splited[1];
        }

        public virtual void Open(Event current) {
            Type type = AssetDatabase.GetMainAssetTypeAtPath(AssetPath);

            if (current.control || current.command) {
                ControlClick(current.button);
                return;
            } else if (current.alt) {
                AltClick(current.button);
                return;
            }

            if (type == typeof(SceneAsset)) OpenSceneAsset(current.button);
            else if (type == typeof(GameObject)) OpenGameobject(current.button);
            else if (type == typeof(UnityEditor.DefaultAsset)) OpenFolder(current.button);
            else OpenDefault(current.button);
        }

        private void OpenSceneAsset(int eventButton) {
            switch (eventButton) {
                case 0:
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        EditorSceneManager.OpenScene(AssetPath);
                    break;
                case 1:
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        EditorSceneManager.CloseScene(
                            EditorSceneManager.GetSceneByPath(AssetPath), true);
                    break;
                case 2:
                    EditorSceneManager.OpenScene(AssetPath, OpenSceneMode.Additive);
                    break;
            }
        }

        private void OpenGameobject(int eventButton) {
            if (PrefabUtility.IsPartOfAnyPrefab(Asset)) OpenPrefab(eventButton);
        }

        private void OpenPrefab(int eventButton) {
            switch (eventButton) {
                case 0:
                    AssetDatabase.OpenAsset(Asset);
                    break;
                case 1:
                    Selection.activeObject = Asset;
                    EditorGUIUtility.PingObject(Selection.activeObject);
                    break;
                case 2:

                    PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
                    if (stage == null) {
                        Selection.activeGameObject = (GameObject)PrefabUtility.InstantiatePrefab(Asset);
                        Undo.RegisterCreatedObjectUndo(Selection.activeGameObject, "Instantiate");
                    } else {
                        GameObject prefab = PrefabUtility.LoadPrefabContents(stage.prefabAssetPath);
                        Selection.activeGameObject =
                            (GameObject)PrefabUtility.InstantiatePrefab(Asset, stage.prefabContentsRoot.transform);
                        Undo.RegisterCreatedObjectUndo(Selection.activeGameObject, "Instantiate");
                    }


                    break;
            }
        }

        private void OpenFolder(int eventButton) {
            switch (eventButton) {
                case 0:
                    AssetDatabase.OpenAsset(Asset);
                    AssetDatabase.OpenAsset(Asset);
                    break;
                case 1:
                case 2:
                    Selection.activeObject = Asset;
                    break;
            }
        }

        private void OpenDefault(int eventButton) {
            switch (eventButton) {
                case 0:
                    Owner.SetProjectLock(true);
                    AssetDatabase.OpenAsset(Asset);
                    break;
                case 1:
                case 2:
                    Selection.activeObject = Asset;
                    break;
            }
        }

        private void ControlClick(int eventButton) {
            switch (eventButton) {
                case 0:
                    Owner.MoveOpenableInsideGroup(this, 1);
                    break;
                case 1:
                    Owner.MoveOpenableInsideGroup(this, -1);
                    break;
                case 2:
                    Owner.RemoveObject(this);
                    break;
            }
        }

        private void AltClick(int eventButton) {
            switch (eventButton) {
                case 0:
                    Owner.MoveOpenableBetweenGroups(this, 1);
                    break;
                case 1:
                    Owner.MoveOpenableBetweenGroups(this, -1);
                    break;
                case 2:
                    Owner.RemoveObject(this);
                    break;
            }
        }

    }

    [Serializable]
    public class ObjectGroup {

        public string GroupName = "GroupName";
        public Color GroupColor = Color.white;
        public List<Object> Assets = new List<Object>();

        public ObjectGroup() {
            GroupName = "GroupName";
            Assets = new List<Object>();
        }

        public ObjectGroup(string groupName, Color groupColor) {
            GroupName = groupName;
            GroupColor = groupColor;
            GroupColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        }

        public string GetPath(int index) {
            return AssetDatabase.GetAssetPath(Assets[index]);
        }

    }

}