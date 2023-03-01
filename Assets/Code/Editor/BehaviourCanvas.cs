using System;
using System.Collections.Generic;
using Code.Runtime;

namespace Code.Editor
{
    public class BehaviourCanvas
    {
        public event Action Chaged;
        public event Action Verified;
    
        public StateModel RootState => _rootState;
        public IReadOnlyList<StateModel> States => _states;
        public IReadOnlyList<TriggerModel> Triggers => _triggers;
    
        private StateModel _rootState; // RootStateID always must be 1
        private List<StateModel> _states;
        private List<TriggerModel> _triggers;

        public BehaviourCanvas(StateModel rootState, List<StateModel> states, List<TriggerModel> triggers)
        {
            _rootState = rootState;
            _states  = states;
            _triggers = triggers;
        }

        public void AddRootState(StateModel state)
        {
            _rootState = state;
        }

        public void AddState(StateModel state)
        {
            _states.Add(state);
        }
        
        public void AddTrigger(TriggerModel trigger)
        {
            _triggers.Add(trigger);
        }
        
        public void RemoveRootState()
        {
            _rootState = null;
        }
        
        public void RemoveState(StateModel state)
        {
            _states.Remove(state);
        }
        
        public void RemoveTrigger(TriggerModel trigger)
        {
            _triggers.Remove(trigger);
        }
    }
}