using Code.Runtime.StateMachineElements;

namespace Code.Runtime.BehaviourGraphSerialization
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

        public override bool CanTarget(IReadOnlyBehaviourElementModel targetModel)
        {
            if (targetModel.GetType() != typeof(StateModel)) return false;
            if (Model.Parameters.Length != targetModel.GetModel().Parameters.Length) return false;
            for (int i = 0; i < Model.Parameters.Length; i++)
            {
                if (Model.Parameters[i].parameterType != targetModel.GetModel().Parameters[i].parameterType) return false;
            }
            return true;
        }
    }
}