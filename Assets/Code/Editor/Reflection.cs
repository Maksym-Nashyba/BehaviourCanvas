using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Code.Editor
{
    public static class Reflection
    {
        public static IEnumerable<Type> GetAllTypesShortName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.Name.Contains(name) && !type.FullName.Contains("VisualScripting"));
        }

        public static Type FromFullName(string fullName)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).First(type => type.FullName == fullName);
        }
    }
}