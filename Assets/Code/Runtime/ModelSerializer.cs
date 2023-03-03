using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Code.Runtime
{
    public class ModelSerializer
    {
        protected List<StateModel> DeserializeStateModels(TextAsset xml) 
        {
            List<TreeModel> models = DeserializeTreeModels(xml, "State");
            return models.ConvertAll(m => (StateModel)m);
        }
    
        protected List<TriggerModel> DeserializeTriggerModels(TextAsset xml)
        {
            List<TreeModel> models = DeserializeTreeModels(xml, "Trigger");
            return models.ConvertAll(m => (TriggerModel)m);
        }
        
        private List<TreeModel> DeserializeTreeModels(TextAsset xml, string modelKey) 
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml.text);
            XmlNodeList treeModelNodes = document.GetElementsByTagName(modelKey);

            List<TreeModel> deserializedModels = new List<TreeModel>(treeModelNodes.Count);
            foreach (XmlNode treeModelNode in treeModelNodes) 
            {
                TreeModel deserializedTreeModel = modelKey == "State" ? new StateModel() : new TriggerModel();
                Model model = new Model();
                List<(string, string)> parametersList = new List<(string, string)>();
                foreach (XmlNode treeModelField in treeModelNode.ChildNodes) 
                {
                    switch (treeModelField.Name) 
                    {
                        case "Name": 
                            model.Name = treeModelField.InnerText;
                            break;
                        case "ID": 
                            deserializedTreeModel.ID = Convert.ToInt32(treeModelField.InnerText);
                            break;
                        case "ResetTarget": 
                            ((TriggerModel)deserializedTreeModel).ResetTarget = Convert.ToBoolean(treeModelField.InnerText);
                            break;
                        case "Parameters": 
                            parametersList.Capacity = treeModelField.ChildNodes.Count;
                            foreach (XmlNode parameter in treeModelField)
                            {
                                parametersList.Add(new ValueTuple<string, string>(parameter.FirstChild.InnerText, parameter.LastChild.InnerText));
                            }
                            break;
                    }
                }
                model.Parameters = parametersList.ToArray();
                deserializedTreeModel.Model = model;
                deserializedModels.Add(deserializedTreeModel);
            }

            return deserializedModels;
        }
    }
}