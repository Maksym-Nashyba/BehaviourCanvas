using System.IO;
using System.Xml;
using Code.Runtime;
using Code.Runtime.BehaviourGraphSerialization;
using Code.Runtime.Initialization;
using UnityEditor;
using UnityEngine;

namespace Code.Editor.Serializers
{
    public abstract class EditorSerializer
    {
        protected readonly BehaviourTreeAsset TreeAsset;

        protected EditorSerializer(BehaviourTreeAsset treeAsset)
        {
            TreeAsset = treeAsset;
        }

        private protected abstract void ValidateTreeAsset(BehaviourTreeAsset treeAsset);

        private protected XmlElement CreateElementWithContent(XmlDocument document, string attributeName, string content) 
        {
            XmlElement attribute = document.CreateElement(string.Empty, attributeName, string.Empty);
            attribute.AppendChild(document.CreateTextNode(content));
            return attribute;
        }
        
        private protected void SaveXML(string xmlName, string xmlContent)
        {
            string path = Application.dataPath.Replace("/Assets", "") + "/" + BehaviourCanvasPaths.BehaviourTreeAssets;
            File.WriteAllText(path + $"/{xmlName}.xml", xmlContent);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}