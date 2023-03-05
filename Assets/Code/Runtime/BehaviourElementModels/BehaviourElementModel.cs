namespace Code.Runtime.BehaviourElementModels
{
    public abstract class BehaviourElementModel : IReadOnlyBehaviourElementModel
    {
        public int Id { get; set; }
        public Model Model { get; set; }
        public IReadOnlyBehaviourElementModel SetTargetModel
        {
            set => _targetModel = value;
        }

        private IReadOnlyBehaviourElementModel _targetModel;

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

        public Model GetModel()
        {
            return Model;
        }

        public IReadOnlyBehaviourElementModel GetTargetModel()
        {
            return _targetModel;
        }
    }
}