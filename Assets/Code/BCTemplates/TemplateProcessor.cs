using System;
using System.Collections.Generic;
using Code.Templates;

namespace Code.BCTemplates
{
    public abstract class TemplateProcessor<TData> where TData : TemplateData
    {
        protected IReadOnlyDictionary<string, string> ProcessedChunks => _processedChunks;
        private Dictionary<string, string> _processedChunks;

        public string Process(TData data)
        {
            ProcessChunks(data, out _processedChunks);
            string rawTemplate = GetRawText();
            return InsertProcessedChunks(rawTemplate);
        }

        private string InsertProcessedChunks(string raw)
        {
            string result = raw;
            while (result.Contains('#'))
            {
                int openingPosition = result.IndexOf('#');
                int closingPosition = result.IndexOf('#', openingPosition + 1);
                
                string key = result.Substring(openingPosition + 1, closingPosition - openingPosition - 1);
                if (_processedChunks.ContainsKey(key)) throw new Exception($"Token {key} is invalid or unprocessed");
                
                result = result.Remove(openingPosition, closingPosition - openingPosition + 1)
                    .Insert(openingPosition, ProcessedChunks[key]);
            }

            return result;
        }

        protected abstract void ProcessChunks(TData data, out Dictionary<string, string> processedChunks);

        protected abstract string GetRawText();
    }
}