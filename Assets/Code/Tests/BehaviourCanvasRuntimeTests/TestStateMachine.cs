using System;
using Code.Runtime.BehaviourGraphSerialization;
using Code.Runtime.StateMachineElements;

namespace Code.Tests.BehaviourCanvasRuntimeTests
{
    public class TestStateMachine : StateMachine
    {
        public IState CurrentState => BehaviourTree.CurrentState;
        public BehaviourTreeAsset Asset => BehaviourTreeAsset;
        private bool _started = false;
        protected override void Start()
        {
        }

        public void BuildTree()
        {
            if (_started) throw new InvalidOperationException("Already built");
            base.Start();
            _started = true;
        }

        protected override void Update()
        {
            if(!_started) return;
            base.Update();
        }
    }
}