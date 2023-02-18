using UnityEditor.Experimental.GraphView;

namespace Code.Editor
{
    public class TriggerView : Node
    {
        private ITrigger _trigger;
        
        public TriggerView(ITrigger trigger)
        {
            _trigger = trigger;
        }
    }
}