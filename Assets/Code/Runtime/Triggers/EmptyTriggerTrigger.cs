using System;
using Code.Runtime.StateMachineElements;
using UnityEngine;

namespace Code.Runtime.Triggers
{
    public class EmptyTriggerTrigger : Trigger
    {
        private State _targetState;
        
    
        public override bool IsHit()
        {
            throw new NotImplementedException();
        }
    
        public override void Reset()
        {
            throw new NotImplementedException();
        }
        
        public override IState PrepareTarget()
        {
            _targetState.ResetStateParameters();
            return _targetState;
        }
        
        public static (string,Type)[] GetParameterList()
        {
            return Array.Empty<(string, Type)>();
        }
    }
}