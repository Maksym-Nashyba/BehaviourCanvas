using Code.Runtime.BehaviourGraphSerialization;
using Code.Runtime.StateMachineElements;
using System;

namespace Code.Tests.BehaviourCanvasRuntimeTests
{
    public class TestStateMachine : StateMachine
    {
        public BehaviourTree Tree => BehaviourTree;
        public BehaviourTreeAsset Asset => BehaviourTreeAsset;
        public bool Started { get; private set; }
        protected override void Start()
        {
        }

        public void BuildTree()
        {
            if (Started) throw new InvalidOperationException("Already built");
            base.Start();
            Started = true;
        }

        public void Step()
        {
            Update();
        }
        
        protected override void Update()
        {
            if(!Started) return;
            base.Update();
        }
    }
}