using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Code.Editor
{
    public abstract class ModelSerializer
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
        
        protected XmlElement CreateStatesXML(XmlDocument document, IReadOnlyList<StateModel> states, bool requiresID) 
        {
            XmlElement statesXML = document.CreateElement(string.Empty, "States", string.Empty);
            
            foreach (StateModel state in states)
            {
                XmlElement stateXML = document.CreateElement(string.Empty, "State", string.Empty);

                XmlElement nameXML = CreateElementWithContent(document, "Name", state.Model.Name);
                stateXML.AppendChild(nameXML);
                
                if (requiresID)
                {
                    XmlElement idXML = CreateElementWithContent(document, "ID", state.ID.ToString());
                    stateXML.AppendChild(idXML);
                }
                
                XmlElement parametersXML = CreateParametersXml(document, state.Model.Parameters);
                stateXML.AppendChild(parametersXML);

                statesXML.AppendChild(stateXML);
            }

            return statesXML;
        }
        
        protected XmlElement CreateTriggersXML(XmlDocument document, IReadOnlyList<TriggerModel> triggers, bool requiresID)
        {
            XmlElement triggersXML = document.CreateElement(string.Empty, "Triggers", string.Empty);
            
            foreach (TriggerModel trigger in triggers)
            {
                XmlElement triggerXML = document.CreateElement(string.Empty, "Trigger", string.Empty);
                
                XmlElement nameXML = CreateElementWithContent(document, "Name", trigger.Model.Name);
                triggerXML.AppendChild(nameXML);
                
                if (requiresID)
                {
                    XmlElement idXML = CreateElementWithContent(document, "ID", trigger.ID.ToString());
                    triggerXML.AppendChild(idXML);
                }
                
                XmlElement resetTargetXML = CreateElementWithContent(document, 
                    "ResetTarget", trigger.ResetTarget.ToString());
                triggerXML.AppendChild(resetTargetXML);

                XmlElement parametersXML = CreateParametersXml(document, trigger.Model.Parameters);
                triggerXML.AppendChild(parametersXML);

                triggersXML.AppendChild(triggerXML);
            }

            return triggersXML;
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