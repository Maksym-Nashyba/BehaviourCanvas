using System.Collections.Generic;
using Code.Runtime.StateMachineElements;

namespace Code.Runtime.BehaviourGraphSerialization
{
    public abstract class BehaviourElementModel : IReadOnlyBehaviourElementModel
    {
        public int Id { get; set; }
        public Model Model { get; set; }
        public List<IReadOnlyBehaviourElementModel> SetTargetModels
        {
            set => _targetModels = value;
        }
        
        private List<IReadOnlyBehaviourElementModel> _targetModels;
        
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

        public List<IReadOnlyBehaviourElementModel> GetTargetModels()
        {
            return _targetModels;
        }

        public abstract bool CanTarget(IReadOnlyBehaviourElementModel targetModel);
    }
}