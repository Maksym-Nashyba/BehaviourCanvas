using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Code.Runtime
{
    public sealed class BehaviourTree
    {
        public IReadOnlyList<IState> States;
        public ReadOnlyDictionary<IState, List<ITrigger>> Triggers;

        public BehaviourTree(IReadOnlyList<IState> states, ReadOnlyDictionary<IState, List<ITrigger>> triggers)
        {
            States = states;
            Triggers = triggers;
        }
    }
}