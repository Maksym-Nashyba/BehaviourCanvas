using System;
using System.Collections.Generic;
using System.Xml;
using Code.Runtime;
using Code.Runtime.BehaviourGraphSerialization;
using UnityEditor;
using UnityEngine;

namespace Code.Editor.Serializers
{
    public sealed class ViewSerializer : EditorSerializer
    {

        public ViewSerializer(BehaviourTreeAsset treeAsset) : base(treeAsset)
        {
            EnsureXmlExists(treeAsset);
        }

        public void Serialize(IReadOnlyCollection<NodeView> nodeViews)
        {
            TextAsset xml = CreateXML(nodeViews);
            TreeAsset.UpdateMarkupXML(xml);
        }

        public Rect GetNodePosition(string nodeID)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(TreeAsset.MarkupXML.text);
            XmlNodeList ids = document.GetElementsByTagName("ModelId");
            Rect position = new Rect(0, 0, 500, 250);
            foreach (XmlNode id in ids)
            {
                if (id.FirstChild.InnerText != nodeID) continue;
                    
                position.x = Convert.ToSingle(id.ParentNode.ChildNodes[1].InnerText);
                position.y = Convert.ToSingle(id.ParentNode.LastChild.InnerText);
                break;
            }
            return position;
        }
        
        private TextAsset CreateXML(IReadOnlyCollection<NodeView> nodeViews)
        {
            XmlDocument document = new XmlDocument();
                    
            XmlElement nodeTreeXML = document.CreateElement(string.Empty, "Markup", string.Empty);
            document.AppendChild(nodeTreeXML);
        
            XmlElement nodesXML = CreateNodesXML(document, nodeViews);
                    
            nodeTreeXML.AppendChild(nodesXML);
        
            return SaveXML($"{TreeAsset.name}Markup", document.OuterXml);
        }

        private XmlElement CreateNodesXML(XmlDocument document, IReadOnlyCollection<NodeView> nodes)
        {
            XmlElement nodesXML = document.CreateElement(string.Empty, "Nodes", string.Empty);
            
            foreach (NodeView node in nodes)
            {
                XmlElement nodeXML = document.CreateElement(string.Empty, "Node", string.Empty);
                
                XmlElement idXML = CreateElementWithContent(document, "ModelId", node.ModelId.ToString());
                XmlElement xPositionXML = CreateElementWithContent(document, "PositionX", node.GetPosition().x.ToString());
                XmlElement yPositionXML = CreateElementWithContent(document, "PositionY", node.GetPosition().y.ToString());
                
                nodeXML.AppendChild(idXML);
                nodeXML.AppendChild(xPositionXML);
                nodeXML.AppendChild(yPositionXML);

                nodesXML.AppendChild(nodeXML);
            }

            return nodesXML;
        }

        private protected override void EnsureXmlExists(BehaviourTreeAsset treeAsset)
        {
            try 
            {
                if (treeAsset.MarkupXML == null || treeAsset.MarkupXML.bytes is null) 
                {
                    Serialize(new List<NodeView>());
                }
            }
            catch (MissingReferenceException) 
            { 
                Serialize(new List<NodeView>());
            }
        }
    }
}