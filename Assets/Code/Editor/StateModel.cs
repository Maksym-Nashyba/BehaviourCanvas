namespace Code.Editor
{
    public class StateModel : TreeModel
    {
        public StateModel() : base()
        {
            
        }
        
        public StateModel(int id, string name, (string, string)[] parameters) : base(id, name, parameters)
        {
            
        }

        public StateModel(int id, Model model) : base(id, model)
        {
            
        }
    }
}