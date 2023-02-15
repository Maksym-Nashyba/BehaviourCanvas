using System;
using Code.Templates;
using UnityEditor;
using UnityEngine;

namespace Code.BCTemplates
{
    public abstract class TextAssetTemplateProcessor<TData> : TemplateProcessor<TData> where TData : TemplateData
    {
        protected abstract string GetTemplateNameNoExtension();
        
        protected override string GetRawText()
        {
            return AssetDatabase.LoadAssetAtPath<TextAsset>(FindTemplatesFolder() + $"/{GetTemplateNameNoExtension()}.txt").text;
        }

        private string FindTemplatesFolder(string currentFolder = "Assets")
        {
            while (!currentFolder.EndsWith("BCTemplates"))
            {
                string[] subFolders = AssetDatabase.GetSubFolders(currentFolder);
                if(subFolders == null || subFolders.Length == 0) return String.Empty;
                
                foreach (string subFolder in subFolders)
                {
                    string result = FindTemplatesFolder(currentFolder + $"/{subFolder}");
                    if (!String.IsNullOrEmpty(result)) return result;
                }
            }

            return currentFolder;
        }
    }
}