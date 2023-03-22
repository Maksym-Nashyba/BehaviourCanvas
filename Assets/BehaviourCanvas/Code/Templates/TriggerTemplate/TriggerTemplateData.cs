using System;

namespace BehaviourCanvas.Code.Templates.TriggerTemplate
{
    public class TriggerTemplateData : TemplateData
    {
        public readonly (string NameCamelCase, Type Type)[] Parameters;
        
        public TriggerTemplateData(string name, params (string NameCamelCase, Type Type)[] parameters) : base(name)
        {
            Parameters = parameters;
        }
    }
}