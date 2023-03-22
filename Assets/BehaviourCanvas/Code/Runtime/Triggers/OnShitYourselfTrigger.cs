using System;
using BehaviourCanvas.Code.Runtime.BehaviourGraphSerialization;
using BehaviourCanvas.Code.Runtime.StateMachineElements;
using UnityEngine;

namespace BehaviourCanvas.Code.Runtime.Triggers
{
    public class OnShitYourselfTrigger : Trigger<Vector2, GameObject, Transform>
    {
        private State<Vector2, GameObject, Transform> _targetState;
        private Vector2 _first;
        private GameObject _second;
        private Transform _third;
    
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
            object[] arguments =
            {
                _first,
                 _second,
                 _third
            };
            arguments = GetParameters().MapTo(_targetState.GetParameters(), arguments);
            _targetState.Reset(arguments);
            return _targetState;
        }
        
        public override ParameterSet GetParameters() => GetParametersStatic();

        public static ParameterSet GetParametersStatic()
        {
            return new ParameterSet(
                new Parameter(typeof(Vector2), "first"),
new Parameter(typeof(GameObject), "second"),
                new Parameter(typeof(Transform), "third")
            );
        }
    }
}