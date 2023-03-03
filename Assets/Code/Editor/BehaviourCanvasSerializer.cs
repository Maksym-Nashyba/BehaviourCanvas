using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Code.Runtime;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public class BehaviourCanvasSerializer : EditorSerializer
    {
        public BehaviourCanvasSerializer(BehaviourTreeAsset treeAsset) : base(treeAsset) 
        {
            ValidateTreeAsset(treeAsset);
        }

        public void Serialize(IReadOnlyList<StateModel> states, IReadOnlyList<TriggerModel> triggers)
        {
            TextAsset xml = CreateXML(states, triggers);
            TreeAsset.UpdateBehaviourTreeXML(xml);
        }

        public List<StateModel> DeserializeStateModels()
        {
            return DeserializeStateModels(TreeAsset.BehaviourTreeXML);
        }
    
        public List<TriggerModel> DeserializeTriggerModels()
        {
            return DeserializeTriggerModels(TreeAsset.BehaviourTreeXML);
        }

        private protected sealed override void ValidateTreeAsset(BehaviourTreeAsset treeAsset)
        {
            try
            {
                if (treeAsset.BehaviourTreeXML.bytes is null)
                {
                    Serialize(new List<StateModel>(), new List<TriggerModel>());
                }
            }
            catch (MissingReferenceException ex)
            {
                Serialize(new List<StateModel>(), new List<TriggerModel>());
            }
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

        private XmlElement CreateTreeModelsXML(XmlDocument document, IReadOnlyList<TreeModel> treeModels, string modelKey)
        {
            //string modelKey = treeModels[0].GetType().Name.Split("Model")[0];
            XmlElement treeModelsXml = CreateModelsXML(document, treeModels.Select(treeModel => treeModel.Model).ToList(), modelKey);
            
            for (int i = 0; i < treeModels.Count; i++)
            {
                XmlElement idXML = CreateElementWithContent(document, "ID", treeModels[i].ID.ToString());
                treeModelsXml.ChildNodes[i].AppendChild(idXML);

                if (treeModels[i] is not TriggerModel triggerModel) continue;
                XmlElement resetTargetXML = CreateElementWithContent(document, "ResetTarget", 
                    triggerModel.ResetTarget.ToString());
                treeModelsXml.ChildNodes[i].AppendChild(resetTargetXML);
            }
            return treeModelsXml;
        }
    }
}