using System.Collections.Generic;
using System.IO;
using System.Xml;
using Code.Runtime;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public abstract class EditorSerializer : ModelSerializer
    {
        protected readonly BehaviourTreeAsset TreeAsset;

        protected EditorSerializer(BehaviourTreeAsset treeAsset)
        {
            TreeAsset = treeAsset;
        }

        private protected abstract void ValidateTreeAsset(BehaviourTreeAsset treeAsset);

        private protected XmlElement CreateModelsXML(XmlDocument document, IReadOnlyList<Model> models, string modelKey)
        {
            XmlElement modelsXML = document.CreateElement(string.Empty, $"{modelKey}s", string.Empty);

            foreach (Model model in models)
            {
                XmlElement modelXML = document.CreateElement(string.Empty, modelKey, string.Empty);
                
                XmlElement nameXML = CreateElementWithContent(document, "Name", model.Name);
                modelXML.AppendChild(nameXML);

                XmlElement parametersXML = CreateParametersXml(document, model.Parameters);
                modelXML.AppendChild(parametersXML);

                modelsXML.AppendChild(modelXML);
            }

            return modelsXML;
        }
        
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
        
        private XmlElement CreateParametersXml(XmlDocument document, (string, string)[] parameters) 
        {
            XmlElement parametersXML = document.CreateElement(string.Empty, "Parameters", string.Empty);
            
            foreach ((string, string) parameter in parameters)
            {
                XmlElement parameterXML = document.CreateElement(string.Empty, "Parameter", string.Empty);

                XmlElement typeXML = CreateElementWithContent(document, "Type", parameter.Item1);
                XmlElement nameXML = CreateElementWithContent(document, "Name", parameter.Item2);
                    
                parameterXML.AppendChild(typeXML);
                parameterXML.AppendChild(nameXML);
                    
                parametersXML.AppendChild(parameterXML);
            }

            return parametersXML;
        }
    }
}