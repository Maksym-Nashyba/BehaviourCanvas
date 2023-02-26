using System;
using System.Collections.Generic;
using System.Linq;

namespace Code.Editor
{
    public static class Reflection
    {
        public static IEnumerable<Type> GetAllTypesShortName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.Name.Contains(name) && !type.FullName.Contains("VisualScripting"));
        }
    }
}