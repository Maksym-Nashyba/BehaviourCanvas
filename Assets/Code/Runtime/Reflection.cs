using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Code.Runtime.BehaviourGraphSerialization;
using Code.Runtime.Initialization;
using Code.Runtime.StateMachineElements;
using Code.Runtime.States;

namespace Code.Runtime
{
    public static class Reflection
    {
        public static IEnumerable<Type> GetAllTypesShortName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.Name.Contains(name) && !type.FullName!.Contains("VisualScripting"));
        }

        public static Type FromFullName(string fullName)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).First(type => type.FullName == fullName);
        }

        public static ParameterSet GetStateParameters(string stateName)
        {
            if(!stateName.Contains("State")) stateName += "State";
            string fullName = typeof(StateAssemblyMarker).FullName!.Replace("StateAssemblyMarker", stateName);
            Type stateType = Assembly.GetAssembly(typeof(StateAssemblyMarker)).GetType(fullName);
            if (stateType == null) throw new ArgumentException($"Failed to find type {fullName}. Should be in the same DIRECTORY and NAMESPACE as {nameof(StateAssemblyMarker)}");

            return GetParameterSet(stateType);
        }

        public static IEnumerable<Type> GetStateTypes(IEnumerable<string> names, Type assemblyAnchor)
        {
            Assembly assembly = Assembly.GetAssembly(assemblyAnchor);
            string namespaceName = assemblyAnchor.FullName!.Replace(assemblyAnchor.Name, "");
            return names.Select(name =>
            {
                Type type = assembly.GetType(namespaceName + name);
                if(type is null) throw new ArgumentException($"Failed to find type {namespaceName + name}. " +
                                                             $"Should be in the same DIRECTORY and NAMESPACE as {nameof(assemblyAnchor)}");
                return type;
            });
        }

        public static Model[] FindAllStates()
        {
            return FindAllModels(typeof(IState));
        }

        public static Model[] FindAllTriggers()
        {
            return FindAllModels(typeof(ITrigger));
        }
        
        private static Model[] FindAllModels(Type modelType)
        {
            return Assembly.GetAssembly(typeof(StateAssemblyMarker)).GetTypes()
                .Where(type => modelType.IsAssignableFrom(type) && type != modelType && !type.IsAbstract)
                .Select(type => new Model(type.Name, GetParameterSet(type)))
                .ToArray();
        }

        private static ParameterSet GetParameterSet(Type type)
        {
            Func<ParameterSet> parameterGetter = (Func<ParameterSet>)Delegate.CreateDelegate(typeof(Func<ParameterSet>),
                null,
                type.GetMethod("GetParametersStatic", BindingFlags.Public | BindingFlags.Static)!);
            return parameterGetter.Invoke();
        }
    }
}