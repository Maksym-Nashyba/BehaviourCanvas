using System;

namespace Code.Runtime.BehaviourElementModels
{
    public struct Model
    {
        public string Name { get; }
        public (Type parameterType, string parameterName)[] Parameters { get; }

        public Model(string name, (Type parameterType, string parameterName)[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }
    }
}