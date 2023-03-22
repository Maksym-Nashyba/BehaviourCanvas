using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace BehaviourCanvas.Code.Runtime.BehaviourGraphSerialization
{
    public class ModelSerializer
    {
        public ModelGraph DeserializeModelGraph(TextAsset xml)
        {
            ModelGraph modelGraph = new ModelGraph(DeserializeStateModels(xml), 
                DeserializeTriggerModels(xml));
            List<(int, int[])> modelsAndTargets = DeserializeModelsWithTargets(xml);
            foreach ((int, int[]) modelAndTarget in modelsAndTargets)
            {
                for (int i = 0; i < modelAndTarget.Item2.Length; i++)
                {
                    modelGraph.AddTargetModel(modelAndTarget.Item1, modelAndTarget.Item2[i]);
                }
            }

            return modelGraph;
        }

        private List<StateModel> DeserializeStateModels(TextAsset xml) 
        {
            List<BehaviourElementModel> models = DeserializeBehaviourElementModels(xml, "State");
            return models.ConvertAll(m => (StateModel)m);
        }
    
        private List<TriggerModel> DeserializeTriggerModels(TextAsset xml)
        {
            List<BehaviourElementModel> models = DeserializeBehaviourElementModels(xml, "Trigger");
            return models.ConvertAll(m => (TriggerModel)m);
        }

        private List<BehaviourElementModel> DeserializeBehaviourElementModels(TextAsset xml, string modelKey) 
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml.text);
            XmlNodeList behaviourElementModelsNodes = document.GetElementsByTagName(modelKey);

            List<BehaviourElementModel> deserializedBehaviourModels = 
                new List<BehaviourElementModel>(behaviourElementModelsNodes.Count);
            
            foreach (XmlNode behaviourModelNode in behaviourElementModelsNodes) 
            {
                BehaviourElementModel behaviourModel = modelKey == "State" ? new StateModel() : new TriggerModel();
                
                string modelName = "";
                List<(Type type, string name)> parametersList = new List<(Type, string)>();
                
                foreach (XmlNode treeModelField in behaviourModelNode.ChildNodes) 
                {
                    switch (treeModelField.Name) 
                    {
                        case "Name": 
                            modelName = treeModelField.InnerText;
                            break;
                        case "Id": 
                            behaviourModel.Id = Convert.ToInt32(treeModelField.InnerText);
                            break;
                        case "ResetTarget": 
                            ((TriggerModel)behaviourModel).ResetTarget = Convert.ToBoolean(treeModelField.InnerText);
                            break;
                        case "Parameters": 
                            parametersList.Capacity = treeModelField.ChildNodes.Count;
                            foreach (XmlNode parameter in treeModelField)
                            {
                                parametersList.Add(new ValueTuple<Type, string>
                                    (Reflection.FromFullName(parameter.FirstChild.InnerText),
                                    parameter.LastChild.InnerText));
                            }
                            break;
                    }
                }
                behaviourModel.Model = new Model(modelName, new ParameterSet(parametersList.ToArray()));
                deserializedBehaviourModels.Add(behaviourModel);
            }
            return deserializedBehaviourModels;
        }
        
        private List<(int, int[])> DeserializeModelsWithTargets(TextAsset xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml.text);
            XmlNodeList idNodes = document.GetElementsByTagName("Id");

            List<(int, int[])> modelsAndTargets = new List<(int, int[])>();
            
            foreach (XmlNode idNode in idNodes)
            {
                if (idNode.ParentNode.LastChild.ChildNodes.Count == 0) continue;
                int modelId = Convert.ToInt32(idNode.InnerText);
                XmlNodeList targetModelsXml = idNode.ParentNode.LastChild.ChildNodes;
                List<int> targetModelIds = new List<int>(targetModelsXml.Count);
                
                foreach (XmlNode targetModelNode in targetModelsXml)
                {
                    if (targetModelNode.InnerText == "") continue;
                    targetModelIds.Add(Convert.ToInt32(targetModelNode.InnerText));
                }
                modelsAndTargets.Add((modelId, targetModelIds.ToArray()));
            }
            return modelsAndTargets;
        }
    }
}