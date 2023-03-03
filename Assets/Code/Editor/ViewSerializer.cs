using System;
using System.Collections.Generic;
using System.Xml;
using Code.Runtime;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public class ViewSerializer : EditorSerializer
    {
        public ViewSerializer(BehaviourTreeAsset treeAsset) : base(treeAsset)
        {
            ValidateTreeAsset(treeAsset);
        }

        public void Serialize(IReadOnlyList<NodeView> nodeViews)
        {
            TextAsset xml = CreateXML(nodeViews);
            TreeAsset.UpdateNodeTreeXML(xml);
        }

        public Rect GetNodePosition(string nodeID)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(TreeAsset.NodeTreeXML.text);
            XmlNodeList nodes = document.GetElementsByTagName("Node");
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
        
        private protected sealed override void ValidateTreeAsset(BehaviourTreeAsset treeAsset) 
        {
            try 
            {
                if (treeAsset.NodeTreeXML.bytes is null) 
                {
                    Serialize(new List<NodeView>());
                }
            }
            catch (MissingReferenceException ex) 
            {
                Serialize(new List<NodeView>());
            }
        }
        
        private TextAsset CreateXML(IReadOnlyList<NodeView> nodeViews) 
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
                XmlElement nodeXML = document.CreateElement(string.Empty, "Node", string.Empty);
                
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
    }
}