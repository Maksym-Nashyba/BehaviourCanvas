using System;
using BehaviourCanvas.Code.Runtime.BehaviourGraphSerialization;
using BehaviourCanvas.Code.Runtime.StateMachineElements;
using UnityEngine;

namespace BehaviourCanvas.Code.Runtime.States
{
    public class TestState : State<float>
    {
        private Single _argument;

        public float Number => _argument;
        
        public TestState(float number)
        {
            _argument = number;
        }

        public TestState()
        {
        }

        protected override void ResetStateParameters(float param0)
        {
            _argument = param0;
        }

        public override void Start()
        {
            Debug.Log("Entered Test State");
        }

        public override void Update()
        {
            Debug.Log("Update In Test State");
        }

        public override void End()
        {
            Debug.Log("Test State Ended");
        }
    
        public override ParameterSet GetParameters() => GetParametersStatic();

        public static ParameterSet GetParametersStatic()
        {
            return new ParameterSet(
                new Parameter(typeof(Single), "argument")
            );
        }
    }
}