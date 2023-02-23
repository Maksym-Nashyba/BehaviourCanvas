using UnityEditor.Experimental.GraphView;

namespace Code.Editor
{
    public class StateView : Node
    {
        private IState _state;

        public StateView(IState state)
        {
            _state = state;
        }
    }
}