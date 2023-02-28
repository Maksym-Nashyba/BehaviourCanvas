using System;
using Code.BCTemplates.StateTemplate;
using Code.BCTemplates.TriggerTemplate;
using Code.Templates;
using Code.Templates.StateTemplate;
using Code.Templates.TriggerTemplate;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Tests
{
    public class BehaviourCanvasTemplates
    {
        [Test]
        public void TemplatesLoad()
        {
            string[] templates =
            {
                "StateTemplate",
                "TriggerTemplate"
            };
            foreach (string template in templates)
            {
                string text = TemplateLoader.GetRawText("StateTemplate");
                bool nullOrEmpty = String.IsNullOrEmpty(text);
                if(nullOrEmpty) Assert.Fail($"Failed to load {template}");
            }

            string loadedTemplates = String.Empty;
            foreach (string template in templates)
            {
                loadedTemplates += $"\n {template}";
            }
            Assert.Pass($"Loaded templates: \n {loadedTemplates}");
        }

        [Test]
        public void StateTemplateProcessedCorrectly()
        {
            StateTemplateProcessor stateTemplateProcessor = new StateTemplateProcessor();
            StateTemplateData data = new StateTemplateData("Hunt", ("target", typeof(NavMeshAgent)), ("maxDistance", typeof(float)));
            string processed = stateTemplateProcessor.Process(data);
            Assert.AreEqual(processed.Replace("\r", ""), 
                TemplateLoader.GetRawText("StateTemplateTest").Replace("\r", ""));
        }
        
        [Test]
        public void TriggerTemplateProcessedCorrectly()
        {
            TriggerTemplateProcessor stateTemplateProcessor = new TriggerTemplateProcessor();
            TriggerTemplateData data = new TriggerTemplateData("NoticedKrisa", ("target", typeof(NavMeshAgent)), ("maxDistance", typeof(float)));
            string processed = stateTemplateProcessor.Process(data);
            Assert.AreEqual(processed.Replace("\r", ""), 
                TemplateLoader.GetRawText("TriggerTemplateTest").Replace("\r", ""));
        }
    }
}
