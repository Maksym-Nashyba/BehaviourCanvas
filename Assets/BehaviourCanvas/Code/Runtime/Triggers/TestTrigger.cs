using System;
using BehaviourCanvas.Code.Runtime.BehaviourGraphSerialization;
using BehaviourCanvas.Code.Runtime.StateMachineElements;
using BehaviourCanvas.Code.Runtime.States;

namespace BehaviourCanvas.Code.Runtime.Triggers
{
    public class TestTrigger : Trigger<Single>
    {
        private State<Single> _targetState;
        private Single _number;
        private bool _isHit;
        
        public float TestParameter => _number;
        public TestState TargetState => (TestState)_targetState;

        public TestTrigger(TestState target, float number)
        {
            _targetState = target;
            _number = number;
        }

        public TestTrigger()
        {
        }

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
            object[] arguments =
            {
                _number
            };
            arguments = GetParameters().MapTo(_targetState.GetParameters(), arguments);
            _targetState.Reset(arguments);
            return _targetState;
        }
    
        public override ParameterSet GetParameters() => GetParametersStatic();

        public static ParameterSet GetParametersStatic()
        {
            return new ParameterSet(
                new Parameter(typeof(Single), "number")
            );
        }
    }
}