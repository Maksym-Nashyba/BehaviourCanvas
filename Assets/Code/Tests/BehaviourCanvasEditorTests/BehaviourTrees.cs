using System.Collections.Generic;
using Code.Runtime.StateMachineElements;
using Code.Runtime.States;
using Code.Runtime.Triggers;
using NUnit.Framework;

namespace Code.Tests.BehaviourCanvasEditorTests
{
    public class BehaviourTrees
    {
        [Test]
        public void OnStartRoot_StartsWithArguments()
        {
            BehaviourTree tree = BuildTree(out TestState rootState, out TestTrigger trigger, out TestState targetState);
            Assert.That(tree.CurrentState, Is.Null);
            tree.StartRootState(new []{new SerializableParameter{FloatValue = 5f}});
            Assert.That(tree.CurrentState, Is.EqualTo(rootState));
            Assert.That(((TestState)tree.CurrentState)!.Number, Is.EqualTo(5f));
        }
        
        [Test]
        public void Transitions()
        {
            BehaviourTree tree = BuildTree(out TestState rootState, out TestTrigger trigger, out TestState targetState);
            tree.StartRootState(new []{new SerializableParameter{FloatValue = 5f}});
            
            Assert.That(tree.CurrentState, Is.EqualTo(rootState));
            tree.Transition(tree.CurrentTriggers[0].PrepareTarget());
            Assert.That(tree.CurrentState, Is.EqualTo(targetState));
        }
        
        [Test]
        public void Transitions_WithParameters()
        {
            BehaviourTree tree = BuildTree(out TestState rootState, out TestTrigger trigger, out TestState targetState);
            tree.StartRootState(new []{new SerializableParameter{FloatValue = 5f}});
            
            Assert.That(tree.CurrentState, Is.EqualTo(rootState));
            tree.Transition(tree.CurrentTriggers[0].PrepareTarget());
            Assert.That(tree.CurrentState, Is.EqualTo(targetState));
            Assert.That(targetState.Number, Is.EqualTo(trigger.TestParameter));
        }

        private BehaviourTree BuildTree(out TestState rootState, out TestTrigger trigger, out TestState targetState)
        {
            rootState = new TestState(-12f);
            targetState = new TestState(9f);
            trigger = new TestTrigger(targetState, 17f);

            List<IState> states = new List<IState>{ rootState, targetState};
            Dictionary<IState, IReadOnlyList<ITrigger>> triggers = new Dictionary<IState, IReadOnlyList<ITrigger>>
            {
                { rootState, new List<ITrigger> { trigger } }
            };

            return new BehaviourTree(rootState,states, triggers);
        }
    }
}