#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace ScriptsCore.CustomTools {

    public static class ScriptableObjectTools
    {
        /// <summary>
        /// The <paramref name="assetPath"/> need to be "Assets/../myScriptableObject"
        /// </summary>
        /// <typeparam name="T">The Type of ScriptableObject.</typeparam>
        /// <param name="assetPath">The path where the asset is/will be.</param>
        /// <returns>The ScriptableObject ref.</returns>
        public static T LoadOrCreate<T>(string assetPath) where T : ScriptableObject
        {
            return LoadOrCreate<T>(assetPath, out bool _);

        }// loadOrCreate()


        /// <summary>
        /// The <paramref name="assetPath"/> need to be "Assets/../myScriptableObject"
        /// </summary>
        /// <typeparam name="T">The Type of ScriptableObject.</typeparam>
        /// <param name="assetPath">The path where the asset is/will be.</param>
        /// <param name="created">Was a object created?</param>
        /// <returns>The ScriptableObject ref.</returns>
        public static T LoadOrCreate<T>(string assetPath, out bool created) where T : ScriptableObject
        {
            if (!assetPath.EndsWith(".asset"))
            {
                assetPath += ".asset";
            }

            DirectoryUtility.CreateDirectory(assetPath);

            T scriptableObject = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            created = false;
            if (scriptableObject == null)
            {
                created = true;
                scriptableObject = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(scriptableObject, assetPath);

                EditorUtility.SetDirty(scriptableObject);
            
            }

            return scriptableObject;

        }

    }

}

#endif