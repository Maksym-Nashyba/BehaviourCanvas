using System;
using System.Collections.Generic;
using System.Linq;
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
        
        protected XmlElement CreateStatesXML(XmlDocument document, IReadOnlyList<StateModel> states) 
        {
            XmlElement statesXML = CreateModelsXML(document, states.Select(state => state.Model).ToList(), "State");
            
            for (int i = 0; i < states.Count; i++)
            {
                XmlElement idXML = CreateElementWithContent(document, "ID", states[i].ID.ToString());
                statesXML.ChildNodes[i].AppendChild(idXML);
            }

            return statesXML;
        }
        
        protected XmlElement CreateTriggersXML(XmlDocument document, IReadOnlyList<TriggerModel> triggers)
        {
            XmlElement triggersXML = CreateModelsXML(document, triggers.Select(trigger => trigger.Model).ToList(), "Trigger");
            
            for (int i = 0; i < triggers.Count; i++)
            {
                XmlElement idXML = CreateElementWithContent(document, "ID", triggers[i].ID.ToString());
                triggersXML.ChildNodes[i].AppendChild(idXML);
                
                XmlElement resetTargetXML = CreateElementWithContent(document, "ResetTarget", triggers[i].ResetTarget.ToString());
                triggersXML.ChildNodes[i].AppendChild(resetTargetXML);
            }

            return triggersXML;
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
            
            foreach ((string, string) parameter in parameters) //TODO check if null
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
        
        protected XmlElement CreateElementWithContent(XmlDocument document, string attributeName, string content)
        {
            XmlElement attribute = document.CreateElement(string.Empty, attributeName, string.Empty);
            attribute.AppendChild(document.CreateTextNode(content));
            return attribute;
        }
    }
}