namespace Code.Runtime.BehaviourGraphSerialization
{
    public struct Model
    {
        public string Name { get; }
        public ParameterSet Parameters { get; }

        public Model(string name, ParameterSet parameters)
        {
            Name = name;
            Parameters = parameters;
        }
    }
}