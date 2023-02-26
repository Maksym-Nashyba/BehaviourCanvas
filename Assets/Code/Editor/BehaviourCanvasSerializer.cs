using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public class BehaviourCanvasSerializer
    {
        private readonly BehaviourTreeAsset _treeAsset;
    
        public BehaviourCanvasSerializer(BehaviourTreeAsset treeAsset)
        {
            _treeAsset = treeAsset;
        }
        
        public StateModel DeserializeRootState()
        {
            //Read from xml _treeAsset.BehaviourTreeXML
            throw new NotImplementedException();
        }
    
        public List<StateModel> DeserializeStates()
        {
            //Read from xml _treeAsset.BehaviourTreeXML
            throw new NotImplementedException();
        }
    
        public List<TriggerModel> DeserializeTriggers()
        {
            //Read from xml _treeAsset.BehaviourTreeXML
            throw new NotImplementedException();
        }
    
        public Rect GetNodePosition(string nodeID)
        {
            //Read from xml _treeAsset.EditorTreeXML
            throw new NotImplementedException();
        }
    
        public void Serialize(BehaviourCanvas canvas, BehaviourCanvasView canvasView) //TODO take out in a separate class
        {
            TextAsset behaviourTreeXML = CreateBehaviourTreeXML(canvas);
            TextAsset editorTreeXML = CreateEditorTreeXML(canvasView);
            _treeAsset.UpdateAsset(behaviourTreeXML, editorTreeXML);
        }

        private TextAsset CreateBehaviourTreeXML(BehaviourCanvas canvas)
        {
            XmlDocument document = new XmlDocument();
            
            XmlElement behaviourCanvasXML = document.CreateElement(string.Empty, "BT", string.Empty);
            document.AppendChild(behaviourCanvasXML);

            XmlElement statesXML = CreateStatesXML(document, canvas.States);
            XmlElement triggersXML = CreateTriggersXML(document, canvas.Triggers);
            
            behaviourCanvasXML.AppendChild(statesXML);
            behaviourCanvasXML.AppendChild(triggersXML);

            TextAsset xml = new TextAsset(document.OuterXml);
            AssetDatabase.CreateAsset(xml, ""); //TODO Add path //TODO take out in a separate class
            AssetDatabase.SaveAssets(); //TODO take out in a separate class
            return xml;
        }
        
        private TextAsset CreateEditorTreeXML(BehaviourCanvasView canvasView) 
        {
            XmlDocument document = new XmlDocument();
                    
            XmlElement editorCanvasXML = document.CreateElement(string.Empty, "Editor", string.Empty);
            document.AppendChild(editorCanvasXML);
        
            XmlElement nodesXML = CreateNodesXML(document, canvasView.Nodes);
                    
            editorCanvasXML.AppendChild(nodesXML);
        
            TextAsset xml = new TextAsset(document.OuterXml);
            AssetDatabase.CreateAsset(xml, ""); //TODO Add path //TODO take out in a separate class
            AssetDatabase.SaveAssets(); //TODO take out in a separate class
            return xml;
        }
        
        private XmlElement CreateStatesXML(XmlDocument document, IReadOnlyList<StateModel> states)
        {
            XmlElement statesXML = document.CreateElement(string.Empty, "States", string.Empty);
            
            foreach (StateModel state in states)
            {
                XmlElement stateXML = document.CreateElement(string.Empty, "State", string.Empty);
                
                XmlElement idXML = CreateAttributeWithContent(document, "ID", state.ID.ToString());
                XmlElement parametersXML = CreateParametersXml(document, state.Parameters);
                
                stateXML.AppendChild(idXML);
                stateXML.AppendChild(parametersXML);

                statesXML.AppendChild(stateXML);
            }

            return statesXML;
        }
        
        private XmlElement CreateTriggersXML(XmlDocument document, IReadOnlyList<TriggerModel> triggers)
        {
            XmlElement triggersXML = document.CreateElement(string.Empty, "Triggers", string.Empty);
            
            foreach (TriggerModel trigger in triggers)
            {
                XmlElement triggerXML = document.CreateElement(string.Empty, "Trigger", string.Empty);
                
                XmlElement idXML = CreateAttributeWithContent(document, "ID", trigger.ID.ToString());
                XmlElement resetTargetXML = CreateAttributeWithContent(document, 
                    "ResetTarget", trigger.ResetTarget.ToString());
                XmlElement parametersXML = CreateParametersXml(document, trigger.Parameters);
                
                triggerXML.AppendChild(idXML);
                triggerXML.AppendChild(resetTargetXML);
                triggerXML.AppendChild(parametersXML);

                triggersXML.AppendChild(triggerXML);
            }

            return triggersXML;
        }

        private XmlElement CreateParametersXml(XmlDocument document, (string, string)[] parameters)
        {
            XmlElement parametersXML = document.CreateElement(string.Empty, "Parameters", string.Empty);
            
            foreach ((string, string) parameter in parameters) //TODO check if null
            {
                XmlElement parameterXML = document.CreateElement(string.Empty, "Parameter", string.Empty);

                XmlElement typeXML = CreateAttributeWithContent(document, "Type", parameter.Item1);
                XmlElement nameXML = CreateAttributeWithContent(document, "Name", parameter.Item2);
                    
                parameterXML.AppendChild(typeXML);
                parameterXML.AppendChild(nameXML);
                    
                parametersXML.AppendChild(parameterXML);
            }

            return parametersXML;
        }
        
        private XmlElement CreateNodesXML(XmlDocument document, IReadOnlyList<NodeView> nodes)
        {
            XmlElement nodesXML = document.CreateElement(string.Empty, "Nodes", string.Empty);
            
            foreach (NodeView node in nodes)
            {
                XmlElement nodeXML = document.CreateElement(string.Empty, "Node", string.Empty);
                
                XmlElement idXML = CreateAttributeWithContent(document, "ID", node.ID.ToString());
                XmlElement xPositionXML = CreateAttributeWithContent(document, "PositionX", node.GetPosition().x.ToString());
                XmlElement yPositionXML = CreateAttributeWithContent(document, "PositionY", node.GetPosition().y.ToString());
                
                nodeXML.AppendChild(idXML);
                nodeXML.AppendChild(xPositionXML);
                nodeXML.AppendChild(yPositionXML);

                nodesXML.AppendChild(nodeXML);
            }

            return nodesXML;
        }

        private XmlElement CreateAttributeWithContent(XmlDocument document, string attributeName, string content)
        {
            XmlElement attribute = document.CreateElement(string.Empty, attributeName, string.Empty);
            attribute.AppendChild(document.CreateTextNode(content));
            return attribute;
        }
    }
}