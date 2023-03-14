using System;
using Code.Runtime.StateMachineElements;
using UnityEngine;

namespace Code.Runtime.States
{
    public class TestState : State<float>
    {
        private Single _argument;
        
        public override void ResetStateParameters(float param0)
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
    
        public static (string,Type)[] GetParameterList()
        {
            return new(string, Type)[]
            {
                ("argument", typeof(Single))
            };
        }
    }
}