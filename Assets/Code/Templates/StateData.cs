using System;

namespace Code.Templates
{
    public struct StateData
    {
        public readonly string Name;
        public readonly (string NameCamelCase, Type Type)[] Parameters;

        public StateData(string name, params (string NameCamelCase, Type Type)[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }
    }
}