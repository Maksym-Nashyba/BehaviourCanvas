using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Code.Runtime;
using Code.Runtime.BehaviourElementModels;
using UnityEditor;
using UnityEngine;

namespace Code.Editor.Serializers
{
    public sealed class BehaviourCanvasSerializer : EditorSerializer
    {
        private readonly ModelSerializer _modelSerializer;
        
        public BehaviourCanvasSerializer(BehaviourTreeAsset treeAsset) : base(treeAsset)
        {
            _modelSerializer = new ModelSerializer();
            ValidateTreeAsset(treeAsset);
        }

        public void Serialize(IReadOnlyList<StateModel> states, IReadOnlyList<TriggerModel> triggers)
        {
            TextAsset xml = CreateXML(states, triggers);
            TreeAsset.UpdateBehaviourTreeXML(xml);
        }

        public List<StateModel> DeserializeStateModels()
        {
            return _modelSerializer.DeserializeStateModels(TreeAsset.BehaviourTreeXML);
        }
    
        public List<TriggerModel> DeserializeTriggerModels()
        {
            return _modelSerializer.DeserializeTriggerModels(TreeAsset.BehaviourTreeXML);
        }

        private TextAsset CreateXML(IReadOnlyList<StateModel> states, IReadOnlyList<TriggerModel> triggers)
        {
            XmlDocument document = new XmlDocument();
            
            XmlElement behaviourCanvasXML = document.CreateElement(string.Empty, "BehaviourTree", string.Empty);
            document.AppendChild(behaviourCanvasXML);

            XmlElement statesXML = CreateTreeModelsXML(document, states, "State");
            XmlElement triggersXML = CreateTreeModelsXML(document, triggers, "Trigger");
            
            behaviourCanvasXML.AppendChild(statesXML);
            behaviourCanvasXML.AppendChild(triggersXML);
            
            SaveXML("BehaviourTree", document.OuterXml);
            TextAsset xml = AssetDatabase.LoadAssetAtPath<TextAsset>(BehaviourCanvasPaths.BehaviourTreeAssets + "/BehaviourTree.xml");
            return xml;
        }

        private XmlElement CreateTreeModelsXML(XmlDocument document, IReadOnlyList<BehaviourElementModel> treeModels, string modelKey)
        {
            XmlElement treeModelsXml = CreateModelsXML(document, treeModels.Select(treeModel => treeModel.Model).ToList(), modelKey);
            
            for (int i = 0; i < treeModels.Count; i++)
            {
                XmlElement idXML = CreateElementWithContent(document, "Id", treeModels[i].Id.ToString());
                treeModelsXml.ChildNodes[i].AppendChild(idXML);

                if (treeModels[i] is not TriggerModel triggerModel) continue;
                XmlElement resetTargetXML = CreateElementWithContent(document, "ResetTarget", 
                    triggerModel.ResetTarget.ToString());
                treeModelsXml.ChildNodes[i].AppendChild(resetTargetXML);
            }
            return treeModelsXml;
        }
        
        private XmlElement CreateModelsXML(XmlDocument document, IReadOnlyList<Model> models, string modelKey)
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
        
        private protected override void ValidateTreeAsset(BehaviourTreeAsset treeAsset)
        {
            try
            {
                if (treeAsset.BehaviourTreeXML.bytes is null)
                {
                    Serialize(new List<StateModel>(), new List<TriggerModel>());
                }
            }
            catch (MissingReferenceException)
            {
                Serialize(new List<StateModel>(), new List<TriggerModel>());
            }
        }
    }
}