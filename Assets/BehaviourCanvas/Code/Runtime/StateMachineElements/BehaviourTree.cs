using System;
using System.Collections.Generic;
using System.Linq;

namespace BehaviourCanvas.Code.Runtime.StateMachineElements
{
    public sealed class BehaviourTree
    {
        public IState CurrentState { get; private set; }
        private readonly IReadOnlyDictionary<IState, IReadOnlyList<ITrigger>> _triggers;
        private readonly IReadOnlyList<IState> _states;
        private readonly IState _rootState;

        public BehaviourTree(IState rootState, IReadOnlyList<IState> states, IReadOnlyDictionary<IState, IReadOnlyList<ITrigger>> triggers)
        {
            _rootState = rootState;
            _states = states;
            _triggers = triggers;
        }
        
        public void StartRootState(SerializableParameter[] serializableParameters)
        {
            if (CurrentState is not null) throw new InvalidOperationException("Only start from root state if the current state is null.");

            object[] rootStateParameters = serializableParameters.Select((parameter, i) => parameter.GetValue(_rootState.GetParameters().Parameters[i].Type)).ToArray();
            CurrentState = _rootState;
            CurrentState.Reset(rootStateParameters);
            CurrentState.Start();
        }
        
        public void Transition(IState target)
        {
            CurrentState.End();
            CurrentState = target;
            target.Start();
        }

        public void ResetTriggers(IState state)
        {
            IReadOnlyList<ITrigger> stateTriggers = TriggersFrom(state);
            for (int i = 0; i < stateTriggers.Count; i++)
            {
                stateTriggers[i].Reset();
            }
        }

        public IReadOnlyList<ITrigger> CurrentTriggers => TriggersFrom(CurrentState);
        
        public IReadOnlyList<ITrigger> TriggersFrom(IState state)
        {
            return _triggers[state];
        }
    }
}