using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Code.Runtime
{
    public sealed class BehaviourTree
    {
        public IState CurrentState { get; private set; }
        private readonly IReadOnlyList<ITrigger> _triggers;
        private readonly IReadOnlyList<IState> _states;
        private readonly IState _rootState;

        public BehaviourTree(IReadOnlyList<IState> states, IReadOnlyList<ITrigger> triggers)
        {
            _states = states;
            _triggers = triggers;
        }
        
        public void StartRootState(params object[] rootStateParameters)
        {
            if (CurrentState is not null) throw new InvalidOperationException("Only start from root state if the current state is null.");
            
            CurrentState = _rootState;
            throw new NotImplementedException();//TODO pass arguments to the root state
            CurrentState.Start();
        }
        
        public void Transition(IState target)
        {
            CurrentState.End();
            target.Start();
            CurrentState = target;
        }

        public void ResetTriggers(IState state)
        {
            IReadOnlyList<ITrigger> stateTriggers = TriggersFrom(state);
            for (int i = 0; i < stateTriggers.Count; i++)
            {
                stateTriggers[i].Reset();
            }
        }

        public IReadOnlyList<ITrigger> TriggersFrom(IState state)
        {
            throw new NotImplementedException();
            return null;
        }
    }
}