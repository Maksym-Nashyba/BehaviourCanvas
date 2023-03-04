using System;
using System.Collections.Generic;
using System.Xml;
using Code.Runtime.BehaviourElementModels;
using UnityEngine;

namespace Code.Runtime
{
    public class ModelSerializer
    {
        public List<StateModel> DeserializeStateModels(TextAsset xml) 
        {
            List<BehaviourElementModel> models = DeserializeBehaviourElementModels(xml, "State");
            return models.ConvertAll(m => (StateModel)m);
        }
    
        public List<TriggerModel> DeserializeTriggerModels(TextAsset xml)
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
                List<(string, string)> parametersList = new List<(string, string)>();
                
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
                                parametersList.Add(new ValueTuple<string, string>(parameter.FirstChild.InnerText,
                                    parameter.LastChild.InnerText));
                            }
                            break;
                    }
                }
                behaviourModel.Model = new Model(modelName, parametersList.ToArray());
                deserializedBehaviourModels.Add(behaviourModel);
            }
            return deserializedBehaviourModels;
        }
    }
}