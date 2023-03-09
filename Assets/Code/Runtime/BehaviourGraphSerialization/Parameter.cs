using System;

namespace Code.Runtime.BehaviourGraphSerialization
{
    public readonly struct Parameter
    {
        public readonly Type Type;
        public readonly string Name;

        public Parameter((Type, string) tuple) : this(tuple.Item1, tuple.Item2)
        {
        }
        
        public Parameter(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public bool IsAssignableFrom(Parameter other)
        {
            return Type.IsAssignableFrom(other.Type);
        }
    }
}