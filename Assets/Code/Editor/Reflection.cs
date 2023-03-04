﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Code.Runtime;
using Code.Runtime.BehaviourElementModels;
using Code.Runtime.States;

namespace Code.Editor
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
                .Where(type => modelType.IsAssignableFrom(type) && type != modelType && !type.IsAbstract).Select(type =>
                {
                    Func<(string, Type)[]> parameterGetter = (Func<(string, Type)[]>)Delegate.CreateDelegate(typeof(Func<(string, Type)[]>),
                        null,
                        type.GetMethod("GetParameterList", BindingFlags.Public | BindingFlags.Static)!);
                    (string, string)[] parameters = parameterGetter.Invoke().Select(parameter => (parameter.Item2.Name, parameter.Item1)).ToArray();
                    return new Model(type.Name, parameters);
                }).ToArray();
        }

        public static (Type, string)[] GetStateParameters(string stateName)
        {
            if(!stateName.Contains("State")) stateName += "State";
            string fullName = typeof(StateAssemblyMarker).FullName!.Replace("StateAssemblyMarker", stateName);
            Type stateType = Assembly.GetAssembly(typeof(StateAssemblyMarker)).GetType(fullName);
            if (stateType == null) throw new ArgumentException($"Failed to find type {fullName}. Should be in the same DIRECTORY and NAMESPACE as {nameof(StateAssemblyMarker)}");
            Func<(string, Type)[]> parameterGetter = (Func<(string, Type)[]>)Delegate.CreateDelegate(typeof(Func<(string, Type)[]>),
                null,
                stateType.GetMethod("GetParameterList", BindingFlags.Public | BindingFlags.Static)!);
            return parameterGetter.Invoke().Select(parameter => (parameter.Item2, parameter.Item1)).ToArray();
        }
    }
}