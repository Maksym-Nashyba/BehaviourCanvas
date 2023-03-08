using System;
using System.Collections.Generic;
using Code.BCTemplates;
using Code.BCTemplates.StateTemplate;
using Code.Runtime.States;

namespace Code.Templates.StateTemplate
{
    public class StateTemplateProcessor : TemplateProcessor<StateTemplateData>
    {
        protected override void ProcessChunks(StateTemplateData data, out Dictionary<string, string> processedChunks)
        {
            processedChunks = new Dictionary<string, string>
            {
                { "Namespace", typeof(StateAssemblyMarker).Namespace },
                { "Name", data.Name + "State" },
                { "BaseClassName", BuildBaseClassName(data) },
                { "Fields", BuildFields(data) },
                { "ResetStateMethodParameters", BuildResetStateMethodParameters(data) },
                { "ParameterGetterBody", BuildParameterGetterBody(data) },
                { "ResetStateBody", BuildResetStateBody(data) }
            };
        }

        #region ChunkProcessors

        private string BuildParameterGetterBody(StateTemplateData data)
        {
            if (data.Parameters.Length == 0) return "return Array.Empty<(string, Type)>();";

            string result = "return new(string, Type)[]\n        {\n";
            for (int i = 0; i < data.Parameters.Length; i++)
            {
                result += "            (" + "\"" + $"{data.Parameters[i].NameCamelCase}" + "\"";
                result += $", typeof({data.Parameters[i].Type.Name})),\n";
            }

            result += "        };";
            return result;
        }
        
        private string BuildResetStateBody(StateTemplateData data)
        {
            if (data.Parameters.Length == 0) return "";
            string result = $"_{data.Parameters[0].NameCamelCase} = param0;";
            
            for (int i = 1; i < data.Parameters.Length; i++)
            {
                result += $"\n _{data.Parameters[i].NameCamelCase} = param{i};";
            }

            return result;
        }

        private string BuildResetStateMethodParameters(StateTemplateData data)
        {
            if (data.Parameters.Length == 0) return "()";
            string result = "(";

            result += $"{data.Parameters[0].Type.Name} param0";
            for (int i = 1; i < data.Parameters.Length; i++)
            {
                result += $", {data.Parameters[i].Type.Name} param{i}";
            }
            
            return result+")";
        }

        private string BuildFields(StateTemplateData data)
        {
            if (data.Parameters.Length == 0) return "";
            string result = $"private {data.Parameters[0].Type.Name} _{data.Parameters[0].NameCamelCase};";
            
            for (int i = 1; i < data.Parameters.Length; i++)
            {
                result += $"\n private {data.Parameters[i].Type.Name} _{data.Parameters[i].NameCamelCase};";
            }

            return result;
        }

        private string BuildBaseClassName(StateTemplateData data)
        {
            if (data.Parameters.Length == 0) return "";
            string result = "<";

            result += data.Parameters[0].Type.Name;
            for (int i = 1; i < data.Parameters.Length; i++)
            {
                result += $", {data.Parameters[i].Type.Name}";
            }
            
            return result+">";
        }

        #endregion

        protected override string GetTemplateNameNoExtension()
        {
            return "StateTemplate";
        }
    }
}