using System;
using System.Collections.Generic;

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
    }
}