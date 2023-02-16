using System.Collections.Generic;

namespace Code.BCTemplates.StateTemplate
{
    public class StateTemplateProcessor : TemplateProcessor<StateTemplateData>
    {
        protected override void ProcessChunks(StateTemplateData data, out Dictionary<string, string> processedChunks)
        {
            processedChunks = new Dictionary<string, string>
            {
                { "Name", data.Name + "State" },
                { "BaseClassName", BuildBaseClassName(data) },
                { "Fields", BuildFields(data) },
                { "ResetStateMethodParameters", BuildResetStateMethodParameters(data) },
                { "ResetStateBody", BuildResetStateBody(data) }
            };
        }

        #region ChunkProcessors

        private string BuildResetStateBody(StateTemplateData data)
        {
            string result = $"_{data.Parameters[0].NameCamelCase} = {data.Parameters[0].NameCamelCase};";
            
            for (int i = 1; i < data.Parameters.Length; i++)
            {
                result += $"\n _{data.Parameters[i].NameCamelCase} = {data.Parameters[i].NameCamelCase};";
            }

            return result;
        }

        private string BuildResetStateMethodParameters(StateTemplateData data)
        {
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
            string result = $"private {data.Parameters[0].Type.Name} _{data.Parameters[0].NameCamelCase};";
            
            for (int i = 1; i < data.Parameters.Length; i++)
            {
                result += $"\n private {data.Parameters[i].Type.Name} _{data.Parameters[i].NameCamelCase};";
            }

            return result;
        }

        private string BuildBaseClassName(StateTemplateData data)
        {
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