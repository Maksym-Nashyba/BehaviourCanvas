using System;
using Code.Runtime.StateMachineElements;
using UnityEngine;

namespace Code.Runtime.Triggers
{
    public class AbobaTrigger : Trigger<Vector3, Single, Rigidbody>
    {
        private State<Vector3, Single, Rigidbody> _targetState;
        private Vector3 _positionA;
        private Single _duration;
        private Rigidbody _rigidbody;

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
            _targetState.ResetStateParameters(_positionA, _duration, _rigidbody);
            return _targetState;
        }
    
        public static (string,Type)[] GetParameterList()
        {
            return new(string, Type)[]
            {
                ("positionA", typeof(Vector3)),
                ("duration", typeof(Single)),
                ("rigidbody", typeof(Rigidbody)),
            };
        }
    }
}