using System;

namespace Code.Templates
{
    public partial class StateTemplate
    {
        private StateData _stateData;

        public StateTemplate(StateData stateData)
        {
            _stateData = stateData;
        }

        private string BuiltBaseClassName()
        {
            return "State" + BuildBaseGenericParametersLiteral(true);
        }
        
        private string BuildBaseGenericParametersLiteral(bool withParenthesis)
        {
            string result = _stateData.Parameters[0].Type.Name;

            for (int i = 1; i < _stateData.Parameters.Length; i++)
            {
                result += "," + _stateData.Parameters[i].Type.Name;
            }

            if (withParenthesis) return "<" + result + ">";
            else return result;
        }

        private string BuildParameterConstructorLine(int index)
        {
            return $"_{_stateData.Parameters[index].NameCamelCase} = stateParameter{index}";
        }

        private string BuildResetMethodParametersLiteral()
        {
            string result = String.Empty;
            foreach ((string NameCamelCase, Type Type) parameter in _stateData.Parameters)
            {
                if (!String.IsNullOrEmpty(result)) result += ",";
                result += $"{parameter.Type.Name} {parameter.NameCamelCase}";
            }
            return result;
        }
    }
}