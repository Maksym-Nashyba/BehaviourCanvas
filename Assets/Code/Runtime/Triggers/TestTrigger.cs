using System;
using Code.Runtime.StateMachineElements;
using Code.Runtime.States;

namespace Code.Runtime.Triggers
{
    public class TestTrigger : Trigger<Single>
    {
        private State<Single> _targetState;
        private Single _number;
        private bool _isHit;
        
        public float TestParameter => _number;
        public TestState TargetState => (TestState)_targetState;

        public void Activate()
        {
            _isHit = true;
        }
        
        public override bool IsHit()
        {
            return _isHit;
        }

        public override void Reset()
        {
            _isHit = false;
        }
    
        public override IState PrepareTarget()
        {
            _targetState.ResetStateParameters(_number);
            return _targetState;
        }
    
        public static (string,Type)[] GetParameterList()
        {
            return new(string, Type)[]
            {
                ("number", typeof(Single))
            };
        }
    }
}