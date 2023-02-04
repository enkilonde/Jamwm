using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ScriptsCore.CustomTools {

    public static class DirectoryUtility
    {

#if UNITY_EDITOR
        /// <summary>
        /// Create a folder and all parents folders if they do not exists
        /// The starting folder is the assets folder.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Return false if no folder was created (if the folder already exist).</returns>
        public static bool CreateDirectory(string path)
        {
            bool folderCreated = false;
            string dataPath = Application.dataPath;

            List<string> subDirectory = path.Split('/').ToList();

            if (subDirectory[0] == "Assets") subDirectory.RemoveAt(0);
            if (subDirectory[subDirectory.Count - 1].Contains('.')) subDirectory.RemoveAt(subDirectory.Count - 1);
        
            for (int i = 0; i < subDirectory.Count; i++)
            {
                if (Directory.Exists(dataPath + "/" + subDirectory[i]))
                {
                    dataPath += "/" + subDirectory[i];
                }
                else
                {
                    Directory.CreateDirectory(dataPath + "/" + subDirectory[i]);
                    Debug.Log("Created directory at '" + dataPath + "/" + subDirectory[i] + "'");
                    dataPath += "/" + subDirectory[i];
                    folderCreated = true;
                }
            }


            return folderCreated;
        }


        /// <summary>
        /// Selects a folder in the project window and shows its content.
        /// Opens a new project window, if none is open yet.
        /// </summary>
        /// <param name="folderInstanceId">The instance of the folder asset to open.</param>
        public static void ShowFolderContents(int folderInstanceId)
        {
            // Find the internal ProjectBrowser class in the editor assembly.
            Assembly editorAssembly = typeof(UnityEditor.Editor).Assembly;
            System.Type projectBrowserType = editorAssembly.GetType("UnityEditor.ProjectBrowser");

            // This is the internal method, which performs the desired action.
            // Should only be called if the project window is in two column mode.
            MethodInfo showFolderContents = projectBrowserType.GetMethod(
                "ShowFolderContents", BindingFlags.Instance | BindingFlags.NonPublic);

            // Find any open project browser windows.
            Object[] projectBrowserInstances = Resources.FindObjectsOfTypeAll(projectBrowserType);

            if (projectBrowserInstances.Length > 0)
            {
                for (int i = 0; i < projectBrowserInstances.Length; i++)
                    ShowFolderContentsInternal(projectBrowserInstances[i], showFolderContents, folderInstanceId);
            }
            else
            {
                EditorWindow projectBrowser = OpenNewProjectBrowser(projectBrowserType);
                ShowFolderContentsInternal(projectBrowser, showFolderContents, folderInstanceId);
            }
        }

        private static void ShowFolderContentsInternal(Object projectBrowser, MethodInfo showFolderContents, int folderInstanceID)
        {
            // Sadly, there is no method to check for the view mode.
            // We can use the serialized object to find the private property.
            SerializedObject serializedObject = new SerializedObject(projectBrowser);
            bool inTwoColumnMode = serializedObject.FindProperty("m_ViewMode").enumValueIndex == 1;

            if (!inTwoColumnMode)
            {
                // If the browser is not in two column mode, we must set it to show the folder contents.
                MethodInfo setTwoColumns = projectBrowser.GetType().GetMethod(
                    "SetTwoColumns", BindingFlags.Instance | BindingFlags.NonPublic);
                setTwoColumns?.Invoke(projectBrowser, null);
            }

            bool revealAndFrameInFolderTree = true;
            showFolderContents.Invoke(projectBrowser, new object[] { folderInstanceID, revealAndFrameInFolderTree });
        }

        private static EditorWindow OpenNewProjectBrowser(System.Type projectBrowserType)
        {
            EditorWindow projectBrowser = EditorWindow.GetWindow(projectBrowserType);
            projectBrowser.Show();

            // Unity does some special initialization logic, which we must call,
            // before we can use the ShowFolderContents method (else we get a NullReferenceException).
            MethodInfo init = projectBrowserType.GetMethod("Init", BindingFlags.Instance | BindingFlags.Public);
            init.Invoke(projectBrowser, null);

            return projectBrowser;
        }

#endif


        public static string GetPathElement(string path, int index, bool keepExtention = false)
        {
            string[] dirs = path.Split('/');

            string file = dirs[dirs.Length - 1 - index];

            if (index == 0 && !keepExtention)
                return file.Split('.')[0];
            else
                return file;
        }


    }

}
