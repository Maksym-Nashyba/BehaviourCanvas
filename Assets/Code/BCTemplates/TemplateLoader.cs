using UnityEditor;
using UnityEngine;

namespace Code.BCTemplates
{
    public static class TemplateLoader
    {
        public static string GetRawText(string templateNameNoExtension)
        {
            string fullPath = GetFullPath(templateNameNoExtension);
            TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(fullPath);
            return asset.text;
        }

        private static string GetFullPath(string templateNameNoExtension)
        {
            return $"Assets/Code/BCTemplates/RawTemplates/{templateNameNoExtension}.txt";//TODO get rid from hard-coded path. Use Editor prefs instead.
        }
    }
}