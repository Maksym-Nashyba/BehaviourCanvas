using System.Collections.Generic;
using System.Linq;
using System.Xml;
using BehaviourCanvas.Code.Runtime.BehaviourGraphSerialization;
using UnityEngine;

namespace BehaviourCanvas.Code.Editor.Serializers
{
    public sealed class EditorModelSerializer : EditorSerializer
    {
        private readonly ModelSerializer _modelSerializer;

        public EditorModelSerializer(BehaviourTreeAsset treeAsset) : base(treeAsset)
        {
            _modelSerializer = new ModelSerializer();
            EnsureXmlExists(treeAsset);
        }

        public void Serialize(IReadOnlyCollection<IReadOnlyBehaviourElementModel> states,
            IReadOnlyCollection<IReadOnlyTriggerModel> triggers)
        {
            TextAsset xml = CreateXML(states, triggers);
            TreeAsset.UpdateBehaviourTreeXML(xml);
        }

        public ModelGraph DeserializeModelGraph()
        {
            return _modelSerializer.DeserializeModelGraph(TreeAsset.GraphXML);
        }

        private TextAsset CreateXML(IReadOnlyCollection<IReadOnlyBehaviourElementModel> states,
            IReadOnlyCollection<IReadOnlyTriggerModel> triggers)
        {
            XmlDocument document = new XmlDocument();

            XmlElement behaviourCanvasXML = document.CreateElement(string.Empty, "Graph", string.Empty);
            document.AppendChild(behaviourCanvasXML);

            XmlElement statesXML = CreateTreeModelsXML(document, states.ToList(), "State");
            XmlElement triggersXML = CreateTreeModelsXML(document, triggers.ToList(), "Trigger");

            behaviourCanvasXML.AppendChild(statesXML);
            behaviourCanvasXML.AppendChild(triggersXML);

            return SaveXML($"{TreeAsset.name}Graph", document.OuterXml);
        }

        private XmlElement CreateTreeModelsXML(XmlDocument document,
            IReadOnlyList<IReadOnlyBehaviourElementModel> behaviourElementModels, string modelKey)
        {
            XmlElement behaviourElementModelsXml = CreateModelsXML(document,
                behaviourElementModels.Select(treeModel => treeModel.GetModel()).ToList(), modelKey);

            for (int i = 0; i < behaviourElementModels.Count; i++)
            {
                XmlElement idXML =
                    CreateElementWithContent(document, "Id", behaviourElementModels[i].GetId().ToString());
                behaviourElementModelsXml.ChildNodes[i].AppendChild(idXML);

                if (behaviourElementModels[i] is IReadOnlyTriggerModel triggerModel)
                {
                    XmlElement resetTargetXML = CreateElementWithContent(document, "ResetTarget",
                        triggerModel.GetResetTarget().ToString());
                    behaviourElementModelsXml.ChildNodes[i].AppendChild(resetTargetXML);
                }

                XmlElement targetModelsXML = document.CreateElement(string.Empty, "TargetModels", string.Empty);
                if (behaviourElementModels[i].GetTargetModels() == null)
                {
                    behaviourElementModelsXml.ChildNodes[i].AppendChild(targetModelsXML);
                    continue;
                }

                foreach (IReadOnlyBehaviourElementModel targetModel in behaviourElementModels[i].GetTargetModels())
                {
                    string targetModelId = targetModel == null ? "" : targetModel.GetId().ToString();
                    XmlElement targetIdXML = CreateElementWithContent(document, "TargetId", targetModelId);
                    targetModelsXML.AppendChild(targetIdXML);
                }

                behaviourElementModelsXml.ChildNodes[i].AppendChild(targetModelsXML);
            }

            return behaviourElementModelsXml;
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

        private XmlElement CreateParametersXml(XmlDocument document, ParameterSet parameters)
        {
            XmlElement parametersXML = document.CreateElement(string.Empty, "Parameters", string.Empty);

            foreach (Parameter parameter in parameters.Parameters)
            {
                XmlElement parameterXML = document.CreateElement(string.Empty, "Parameter", string.Empty);

                XmlElement typeXML = CreateElementWithContent(document, "Type", parameter.Type.FullName);
                XmlElement nameXML = CreateElementWithContent(document, "Name", parameter.Name);

                parameterXML.AppendChild(typeXML);
                parameterXML.AppendChild(nameXML);

                parametersXML.AppendChild(parameterXML);
            }

            return parametersXML;
        }

        private protected override void EnsureXmlExists(BehaviourTreeAsset treeAsset)
        {
            try
            {
                if (treeAsset.GraphXML == null || treeAsset.GraphXML.bytes is null)
                {
                    Serialize(new List<StateModel>(), new List<TriggerModel>());
                }
            }
            catch (MissingReferenceException)
            {
                Serialize(new List<StateModel>(), new List<TriggerModel>());
            }
        }
    }
}