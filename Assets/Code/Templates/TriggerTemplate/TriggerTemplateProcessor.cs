using System.Collections.Generic;
using Code.BCTemplates;
using Code.BCTemplates.TriggerTemplate;

namespace Code.Templates.TriggerTemplate
{
    public class TriggerTemplateProcessor : TemplateProcessor<TriggerTemplateData>
    {
        protected override void ProcessChunks(TriggerTemplateData data, out Dictionary<string, string> processedChunks)
        {
            processedChunks = new Dictionary<string, string>
            {
                { "Name", data.Name + "Trigger" },
                { "BaseClassGenericParameters", BuildBaseClassGenericParameters(data) },
                { "TargetStateField", BuildTargetStateField(data) },
                { "ParameterFields", BuildParameterFields(data) },
                { "ResetTargetParameters", BuildResetTargetParameters(data) }
            };
        }

        #region ChunkProcessors

        private string BuildResetTargetParameters(TriggerTemplateData data)
        {
            if (data.Parameters.Length == 0) return "()";
            string result = "(";

            result += $"_{data.Parameters[0].NameCamelCase}";
            for (int i = 1; i < data.Parameters.Length; i++)
            {
                result += $", _{data.Parameters[i].NameCamelCase}";
            }
            
            return result+")";
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