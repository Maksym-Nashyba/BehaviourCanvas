using System;
using Code.Templates;

namespace Code.BCTemplates.TriggerTemplate
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