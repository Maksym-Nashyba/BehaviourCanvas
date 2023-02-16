using System;
using Code.Templates;

namespace Code.BCTemplates.StateTemplate
{
    public class StateTemplateData : TemplateData
    {
        public readonly (string NameCamelCase, Type Type)[] Parameters;
        
        public StateTemplateData(string name, params (string NameCamelCase, Type Type)[] parameters) : base(name)
        {
            Parameters = parameters;
        }
    }
}