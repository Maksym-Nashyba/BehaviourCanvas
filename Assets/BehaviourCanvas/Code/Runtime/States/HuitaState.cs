using System;
using BehaviourCanvas.Code.Runtime.BehaviourGraphSerialization;
using BehaviourCanvas.Code.Runtime.StateMachineElements;
using UnityEngine;

namespace BehaviourCanvas.Code.Runtime.States
{
    public class HuitaState : State<Single, GameObject, Model>
    {
        private Single _zalypa;
 private GameObject _hui;
 private Model _ochko;
    
        protected override void ResetStateParameters(Single param0, GameObject param1, Model param2)
        {
            _zalypa = param0;
 _hui = param1;
 _ochko = param2;
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
        
        public override ParameterSet GetParameters() => GetParametersStatic();

        public static ParameterSet GetParametersStatic()
        {
            return new ParameterSet(
                new Parameter(typeof(Single), "zalypa"),
new Parameter(typeof(GameObject), "hui"),
                new Parameter(typeof(Model), "ochko")
            );
        }
    }
}