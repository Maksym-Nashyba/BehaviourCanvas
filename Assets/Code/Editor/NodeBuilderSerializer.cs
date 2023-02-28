using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public class NodeBuilderSerializer : ModelSerializer
    {
        private TextAsset _nodeDatabaseXML;

        public NodeBuilderSerializer(TextAsset xml)
        {
            _nodeDatabaseXML = xml;
        }
        
        public IReadOnlyList<StateModel> DeserializeStateModels()
        {
            return DeserializeStateModels(_nodeDatabaseXML);
        }
        
        public IReadOnlyList<TriggerModel> DeserializeTriggerModels()
        {
            return DeserializeTriggerModels(_nodeDatabaseXML);
        }

        private TextAsset CreateModelsDatabaseXML()
        {
            XmlDocument document = new XmlDocument();
                    
            XmlElement editorDatabaseXML = document.CreateElement(string.Empty, "ModelsDatabase", string.Empty);
            document.AppendChild(editorDatabaseXML);
            
            //TODO find states and triggers in assets

            /*XmlElement statesXML = CreateStatesXML(document, );
            XmlElement triggersXML = CreateTriggersXML(document, );
            
            editorDatabaseXML.AppendChild(statesXML);
            editorDatabaseXML.AppendChild(triggersXML);*/
            
            TextAsset xml = new TextAsset(document.OuterXml);
            AssetDatabase.CreateAsset(xml, ""); //TODO Add path with name.xml
            AssetDatabase.SaveAssets();
            return xml;
        }
    }
}