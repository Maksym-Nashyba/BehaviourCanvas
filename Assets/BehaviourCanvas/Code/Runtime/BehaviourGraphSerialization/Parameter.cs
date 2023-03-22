using System;

namespace BehaviourCanvas.Code.Runtime.BehaviourGraphSerialization
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

        public bool IsValidValue(object argument)
        {
            return Type.IsAssignableFrom(argument.GetType());
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() / 3 + Name.GetHashCode() / 3;
        }

        public static bool operator ==(Parameter p1, Parameter p2) 
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Parameter p1, Parameter p2) 
        {
            return !p1.Equals(p2);
        }
    }
}