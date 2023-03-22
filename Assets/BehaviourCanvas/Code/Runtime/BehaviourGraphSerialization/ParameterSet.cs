using System;
using System.Collections.Generic;
using System.Linq;

namespace BehaviourCanvas.Code.Runtime.BehaviourGraphSerialization
{
    public readonly struct ParameterSet
    {
        public const uint MaxParameterCount = 3;
        
        public static ParameterSet Empty => new ParameterSet(Array.Empty<Parameter>());

        public bool IsEmpty => Count == 0;
        
        public int Count => Parameters.Length;
        
        public readonly Parameter[] Parameters;
        
        public Parameter this[int i] => Parameters[i];

        #region Constructors

        public ParameterSet((Type type, string name)[] rawParameters)
        {
            List<Parameter> parameters = new List<Parameter>(rawParameters.Length);
            foreach ((Type, string) parameterTuple in rawParameters)
            {
                if (parameters.Any(parameter => parameter.Name == parameterTuple.Item2))
                    throw new ArgumentException("Parameters set can't have two parameters with similar names.");
                parameters.Add(new Parameter(parameterTuple));
            }

            Parameters = parameters.ToArray();
        }
        
        public ParameterSet(Parameter[] parameters)
        {
            if (parameters.Length > MaxParameterCount) throw new ArgumentOutOfRangeException($"{nameof(parameters)}",
                $"Behaviour elements can only have up to {MaxParameterCount} parameters");
            Parameters = parameters;
        }

        public ParameterSet(Parameter parameter0)
        {
            Parameters = new[] { parameter0 };
        }
        
        public ParameterSet(Parameter parameter0, Parameter parameter1)
        {
            Parameters = new[] { parameter0 , parameter1};
        }
        
        public ParameterSet(Parameter parameter0, Parameter parameter1, Parameter parameter2)
        {
            Parameters = new[] { parameter0 , parameter1, parameter2};
        }

        #endregion

        #region Mapping

        public bool AreValidValues(params object[] argumets)
        {
            if (argumets.Length < Parameters.Length) return false;
            for (int i = 0; i < Parameters.Length; i++)
            {
                if (!Parameters[i].IsValidValue(argumets[i])) return false;
            }

            return true;
        }
        
        public object[] MapTo(ParameterSet other, object[] values)
        {
            values ??= Array.Empty<object>();
            if (!AreValidValues(values)) throw new ArgumentException();
            if (CanMapDirectrly(other.Parameters) || IsDirectSupersetOf(other.Parameters))
            {
                return values.Take(other.Count).ToArray();
            }

            if (!CanMapTo(other)) throw new ArgumentException("Failed to map arguments.");
            
            object[] indirectMap = new object[other.Count];
            for (var i = 0; i < other.Count; i++)
            {
                int argumentIndex = Array.FindIndex(Parameters, parameter => parameter == other.Parameters[i]);
                indirectMap[i] = values[argumentIndex];
            }
            return indirectMap;
        }
        
        public bool CanMapTo(ParameterSet other)
        {
            if (other.Parameters.Length == 0) return true;
            if (Parameters.Length < other.Parameters.Length) return false;

            if (other.Parameters.Length == Parameters.Length)
            {
                if (CanMapDirectrly(other.Parameters)) return true;
                if (CanMapIndirectrly(other.Parameters)) return true;
            }
            else
            {
                if (IsDirectSupersetOf(other.Parameters)) return true;
                if (IsIndirectSupersetOf(other.Parameters)) return true;
            }
            return false;
        }

        private bool IsIndirectSupersetOf(Parameter[] otherParameters)
        {
            if (otherParameters.Length >= Parameters.Length) return false;

            foreach (Parameter otherParameter in otherParameters)
            {
                if (!Parameters.Contains(otherParameter)) return false;
            }

            return true;
        }

        private bool IsDirectSupersetOf(Parameter[] otherParameters)
        {
            if (otherParameters.Length >= Parameters.Length) return false;
            
            for (int i = 0; i < otherParameters.Length; i++)
            {
                if (!otherParameters[i].IsAssignableFrom(Parameters[i])) return false;
            }

            return true;
        }

        private bool CanMapDirectrly(Parameter[] otherParameters)
        {
            if (otherParameters.Length != Parameters.Length) return false;
            
            for (int i = 0; i < otherParameters.Length; i++)
            {
                if (!otherParameters[i].IsAssignableFrom(Parameters[i])) return false;
            }

            return true;
        }
        
        private bool CanMapIndirectrly(Parameter[] otherParameters)
        {
            foreach (Parameter otherParameter in otherParameters)
            {
                if (!Parameters.Contains(otherParameter)) return false;
            }

            return true;
        }

        #endregion
    }
}