namespace Code.Runtime.BehaviourElementModels
{
    public struct Model
    {
        public string Name { get; }
        public (string, string)[] Parameters { get; }

        public Model(string name, (string, string)[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }
    }
}