namespace Code.Editor
{
    public class TriggerModel : TreeModel
    {
        public bool ResetTarget { get; set; }
    
        public TriggerModel() : base()
        {
            ResetTarget = false;
        }
    
        public TriggerModel(int id, string name, (string, string)[] parameters, bool resetTarget) : base(id, name, parameters)
        {
            ResetTarget = resetTarget;
        }
    
        public TriggerModel(int id, Model model, bool resetTarget) : this(id, model.Name, model.Parameters, resetTarget)
        {
        
        }
    }
}