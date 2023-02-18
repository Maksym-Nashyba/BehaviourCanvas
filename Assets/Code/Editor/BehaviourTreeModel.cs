using System.Collections.Generic;

namespace Code.Editor
{
    public class BehaviourTreeModel
    {
        private IState _rootState;
        private List<IState> _states;
        private List<ITrigger> _triggers;
    }
}