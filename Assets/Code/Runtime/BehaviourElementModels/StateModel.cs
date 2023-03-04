namespace Code.Runtime.BehaviourElementModels
{
    public class StateModel : BehaviourElementModel
    {
        public const int RootId = -1;
        public bool IsRoot => Id == RootId;
        
        public StateModel() : base()
        {
            
        }

        public StateModel(int id, Model model) : base(id, model)
        {
            
        }
    }
}