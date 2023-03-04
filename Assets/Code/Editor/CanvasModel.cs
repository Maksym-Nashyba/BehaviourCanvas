using System;
using System.Collections.Generic;
using System.Linq;
using Code.Runtime.BehaviourElementModels;

namespace Code.Editor
{
    public class CanvasModel
    {
        public event Action Changed;
        public event Action<IReadOnlyBehaviourElementModel> ModelAdded;
        public event Action<int> ModelRemoved;

        public StateModel RootState => _states.First(state => state.IsRoot);
        public IReadOnlyList<StateModel> States => _states;
        public IReadOnlyList<TriggerModel> Triggers => _triggers;

        private List<StateModel> _states;
        private List<TriggerModel> _triggers;

        public CanvasModel()
        {
            _states = new List<StateModel>();
            _triggers = new List<TriggerModel>();
        }

        public void Deserialize(List<StateModel> states, List<TriggerModel> triggers)
        {
            _states = states;
            _triggers = triggers;
            Changed?.Invoke();
        }

        #region States
        public void AddState(StateModel state)
        {
            _states.Add(state);
            ModelAdded?.Invoke(state);
        }

        public void RemoveState(StateModel state)
        {
            _states.Remove(state);
            ModelRemoved?.Invoke(state.Id); 
        }
        #endregion
        
        #region Triggers
        public void AddTrigger(TriggerModel trigger)
        {
            _triggers.Add(trigger);
            ModelAdded?.Invoke(trigger);
        }

        public void RemoveTrigger(TriggerModel trigger)
        {
            _triggers.Remove(trigger);
            ModelRemoved?.Invoke(trigger.Id);
        }
        #endregion
    }
}