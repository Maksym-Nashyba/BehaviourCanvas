namespace Code.Runtime.BehaviourElementModels
{
    public class TriggerModel : BehaviourElementModel, IReadOnlyTriggerModel
    {
        public bool ResetTarget { get; set; }
    
        public TriggerModel() : base()
        {
            ResetTarget = false;
        }

        public TriggerModel(int id, Model model, bool resetTarget) : base(id, model)
        {
            ResetTarget = resetTarget;
        }

        public bool GetResetTarget()
        {
            return ResetTarget;
        }
    }
}