namespace Code.Runtime.BehaviourElementModels
{
    public struct Model
    {
        public string Name { get; }
        public (string typeName, string parameterName)[] Parameters { get; }

        public Model(string name, (string typeName, string parameterName)[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }
    }
}