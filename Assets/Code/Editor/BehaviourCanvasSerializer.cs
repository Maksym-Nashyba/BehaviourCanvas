using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Code.Runtime;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public class BehaviourCanvasSerializer : ModelSerializer
    {
        private readonly BehaviourTreeAsset _treeAsset;
    
        public BehaviourCanvasSerializer(BehaviourTreeAsset treeAsset)
        {
            _treeAsset = treeAsset;
            ValidateTreeAsset(treeAsset);
        }
        
        private void ValidateTreeAsset(BehaviourTreeAsset treeAsset)
        {
            TextAsset behaviourTreeXml = treeAsset.BehaviourTreeXML;
            TextAsset nodeTreeXml = treeAsset.NodeTreeXML;
            try
            {
                if (treeAsset.BehaviourTreeXML.bytes is null)
                {
                    behaviourTreeXml = CreateBehaviourTreeXML(new List<StateModel>(), new List<TriggerModel>());
                }
            }
            catch (MissingReferenceException ex)
            {
                behaviourTreeXml = CreateBehaviourTreeXML(new List<StateModel>(), new List<TriggerModel>());
            }
            try
            {
                if (treeAsset.NodeTreeXML.bytes is null)
                {
                    nodeTreeXml = CreateNodeTreeXML(new List<NodeView>());
                }
            }
            catch (MissingReferenceException ex)
            {
                nodeTreeXml = CreateNodeTreeXML(new List<NodeView>());
            }
            _treeAsset.UpdateAsset(behaviourTreeXml, nodeTreeXml);
        }
        
        public StateModel FindRootState(IReadOnlyList<StateModel> states)
        {
            StateModel rootState = new StateModel();
            foreach (StateModel state in states)
            {
                if (state.ID != 1) continue;
                rootState = state;
                break;
            }
            return rootState;
        }
        
        public List<StateModel> DeserializeStateModels()
        {
            return DeserializeStateModels(_treeAsset.BehaviourTreeXML);
        }
    
        public List<TriggerModel> DeserializeTriggerModels()
        {
            return DeserializeTriggerModels(_treeAsset.BehaviourTreeXML);
        }

        public Rect GetNodePosition(string nodeID)
        {
            return GetNodePosition(nodeID, "Node");
        }

        public Rect GetTriggerNodePosition(string nodeID)
        {
            return GetNodePosition(nodeID, "TriggerNode");
        }

        public void Serialize(BehaviourCanvas canvas, BehaviourCanvasView canvasView)
        {
            TextAsset behaviourTreeXML = CreateBehaviourTreeXML(canvas.States, canvas.Triggers);
            TextAsset editorTreeXML = CreateNodeTreeXML(canvasView.Nodes);
            _treeAsset.UpdateAsset(behaviourTreeXML, editorTreeXML);
        }
        
        private Rect GetNodePosition(string nodeID, string nodeType)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(_treeAsset.NodeTreeXML.text);
            XmlNodeList nodes = document.GetElementsByTagName(nodeType);

            Rect position = new Rect(0, 0, 200, 100);
            foreach (XmlNode node in nodes)
            {
                foreach (XmlNode nodeField in node.ChildNodes)
                {
                    if (nodeField.FirstChild.InnerText != nodeID) continue;
                    
                    position.x = Convert.ToSingle(node.ChildNodes[1].InnerText);
                    position.y = Convert.ToSingle(node.LastChild.InnerText);
                    break;
                }
            }

            return position;
        }

        private TextAsset CreateBehaviourTreeXML(IReadOnlyList<StateModel> states, IReadOnlyList<TriggerModel> triggers)
        {
            XmlDocument document = new XmlDocument();
            
            XmlElement behaviourCanvasXML = document.CreateElement(string.Empty, "BehaviourTree", string.Empty);
            document.AppendChild(behaviourCanvasXML);

            XmlElement statesXML = CreateStatesXML(document, states);
            XmlElement triggersXML = CreateTriggersXML(document, triggers);
            
            behaviourCanvasXML.AppendChild(statesXML);
            behaviourCanvasXML.AppendChild(triggersXML);
            
            SaveXML("BehaviourTree", document.OuterXml);
            TextAsset xml = AssetDatabase.LoadAssetAtPath<TextAsset>(BehaviourCanvasPaths.BehaviourTreeAssets + "/BehaviourTree.xml");
            return xml;
        }
        
        private TextAsset CreateNodeTreeXML(IReadOnlyList<NodeView> nodeViews) 
        {
            XmlDocument document = new XmlDocument();
                    
            XmlElement nodeTreeXML = document.CreateElement(string.Empty, "NodeTree", string.Empty);
            document.AppendChild(nodeTreeXML);
        
            XmlElement nodesXML = CreateNodesXML(document, nodeViews);
                    
            nodeTreeXML.AppendChild(nodesXML);
        
            SaveXML("NodeTree", document.OuterXml);
            TextAsset xml = AssetDatabase.LoadAssetAtPath<TextAsset>(BehaviourCanvasPaths.BehaviourTreeAssets + "/NodeTree.xml");
            return xml;
        }

        private XmlElement CreateNodesXML(XmlDocument document, IReadOnlyList<NodeView> nodes)
        {
            XmlElement nodesXML = document.CreateElement(string.Empty, "Nodes", string.Empty);
            
            foreach (NodeView node in nodes)
            {
                string nodeType = node is TriggerView ? "TriggerNode" : "Node";
                XmlElement nodeXML = document.CreateElement(string.Empty, nodeType, string.Empty);
                
                XmlElement idXML = CreateElementWithContent(document, "ID", node.ID.ToString());
                XmlElement xPositionXML = CreateElementWithContent(document, "PositionX", node.GetPosition().x.ToString());
                XmlElement yPositionXML = CreateElementWithContent(document, "PositionY", node.GetPosition().y.ToString());
                
                nodeXML.AppendChild(idXML);
                nodeXML.AppendChild(xPositionXML);
                nodeXML.AppendChild(yPositionXML);

                nodesXML.AppendChild(nodeXML);
            }

            return nodesXML;
        }

        private void SaveXML(string xmlName, string xmlContent)
        {
            string path = Application.dataPath.Replace("/Assets", "") + "/" + BehaviourCanvasPaths.BehaviourTreeAssets;
            File.WriteAllText(path + $"/{xmlName}.xml", xmlContent);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}