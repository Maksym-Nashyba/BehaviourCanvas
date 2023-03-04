namespace Code.Runtime.BehaviourElementModels
{
    public abstract class BehaviourElementModel : IReadOnlyBehaviourElementModel
    {
        public int Id { get; set; }
        public Model Model { get; set; }

        protected BehaviourElementModel()
        {
            Id = 0;
            Model = new Model();
        }

        protected BehaviourElementModel(int id, Model model)
        {
            Id = id;
            Model = model;
        }

        public int GetId()
        {
            return Id;
        }

        public string GetName()
        {
            return Model.Name;
        }

        public (string, string)[] GetParameters()
        {
            return Model.Parameters;
        }
    }
}