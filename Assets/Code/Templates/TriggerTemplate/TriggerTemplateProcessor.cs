using System;
using System.Collections.Generic;
using Code.BCTemplates;
using Code.BCTemplates.StateTemplate;
using Code.BCTemplates.TriggerTemplate;
using Code.Runtime.Triggers;

namespace Code.Templates.TriggerTemplate
{
    public class TriggerTemplateProcessor : TemplateProcessor<TriggerTemplateData>
    {
        protected override void ProcessChunks(TriggerTemplateData data, out Dictionary<string, string> processedChunks)
        {
            processedChunks = new Dictionary<string, string>
            {
                { "Namespace", typeof(TriggerAssemblyMarker).Namespace },
                { "Name", data.Name + "Trigger" },
                { "BaseClassGenericParameters", BuildBaseClassGenericParameters(data) },
                { "TargetStateField", BuildTargetStateField(data) },
                { "ParameterFields", BuildParameterFields(data) },
                { "ParameterGetterBody", BuildParameterGetterBody(data) },
                { "ResetTargetParameters", BuildResetTargetParameters(data) }
            };
        }

        #region ChunkProcessors

        private string BuildParameterGetterBody(TriggerTemplateData data)
        {
            if (data.Parameters.Length == 0) return String.Empty;
            string result = $"                new Parameter(typeof({data.Parameters[^1].Type.Name}), {data.Parameters[^1].NameCamelCase})";

            for (int i = data.Parameters.Length-2; i >= 0; i--)
            {
                result = $"new Parameter(typeof({data.Parameters[i].Type.Name}), {data.Parameters[i].NameCamelCase}),\n" + result;
            }
            return result;
        }
        
        private string BuildResetTargetParameters(TriggerTemplateData data)
        {
            if (data.Parameters.Length == 0) return String.Empty;
            string result = "_"+data.Parameters[0].NameCamelCase;
            
            for (int i = 1; i < data.Parameters.Length; i++)
            {
                result += $",\n                 _{data.Parameters[i].NameCamelCase}";
            }
            
            return result;
        }

        private string BuildTargetStateField(TriggerTemplateData data)
        {
            return $"private State{BuildBaseClassGenericParameters(data)} _targetState;";
        }

        private string BuildParameterFields(TriggerTemplateData data)
        {
            if (data.Parameters.Length == 0) return "";
            string result = $"private {data.Parameters[0].Type.Name} _{data.Parameters[0].NameCamelCase};";
            
            for (int i = 1; i < data.Parameters.Length; i++)
            {
                result += $"\n private {data.Parameters[i].Type.Name} _{data.Parameters[i].NameCamelCase};";
            }

            return result;
        }

        private string BuildBaseClassGenericParameters(TriggerTemplateData data)
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
            return "TriggerTemplate";
        }
    }
}