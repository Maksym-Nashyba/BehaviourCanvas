using System;
using Code.Runtime.StateMachineElements;
using UnityEngine;

namespace Code.Runtime.States
{
    public class ParameterisedState : State<Vector3, StateMachine>
    {
        private Vector3 _position;
        private StateMachine _stateMachine;

        public override void ResetStateParameters(Vector3 param0, StateMachine param1)
        {
            _position = param0;
            _stateMachine = param1;
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void End()
        {
            throw new NotImplementedException();
        }
    
        public static (string,Type)[] GetParameterList()
        {
            return new(string, Type)[]
            {
                ("position", typeof(Vector3)),
                ("stateMachine", typeof(StateMachine)),
            };
        }
    }
}