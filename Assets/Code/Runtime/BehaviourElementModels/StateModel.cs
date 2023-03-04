namespace Code.Runtime.BehaviourElementModels
{
    public class StateModel : BehaviourElementModel
    {
        public bool IsRoot => Id == 1;
        
        public StateModel() : base()
        {
            
        }

        public StateModel(int id, Model model) : base(id, model)
        {
            
        }
    }
}