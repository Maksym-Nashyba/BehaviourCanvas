namespace Code.Runtime
{
    public struct Model
    {
        public string Name;
        public (string, string)[] Parameters;

        public Model(string name, (string, string)[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }
    }
}