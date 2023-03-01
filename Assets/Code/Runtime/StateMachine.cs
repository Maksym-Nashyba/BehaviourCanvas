using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Code.Runtime
{
    public abstract class StateMachine : MonoBehaviour
    {
        //[SerializeField] private BehaviourTreeAsset _behaviourTreeAsset;
        [Inject] private BehaviourTreeBuilder _behaviourTreeBuilder;
        protected BehaviourTree BehaviourTree;
        protected IState CurrentState { get; private set; }

        private void Start()
        {
            //BehaviourTree = _behaviourTreeBuilder.BuildTree(_behaviourTreeAsset);
            CurrentState = StartRootState();
        }

        private void Update()
        {
            CurrentState.Update();

            IReadOnlyList<ITrigger> currentStateTriggers = BehaviourTree.Triggers[CurrentState];
            for (int i = 0; i < currentStateTriggers.Count; i++)
            {
                ITrigger trigger = currentStateTriggers[i];
                if (trigger.IsHit())
                {
                    IState beforeTransition = CurrentState;
                    Transition(trigger.PrepareTarget());
                    ResetTriggers(beforeTransition);
                }
            }
        }

        private void Transition(IState target)
        {
            CurrentState.End();
            target.Start();
            CurrentState = target;
        }

        private void ResetTriggers(IState state)
        {
            IReadOnlyList<ITrigger> stateTriggers = BehaviourTree.Triggers[state];
            for (int i = 0; i < stateTriggers.Count; i++)
            {
                stateTriggers[i].Reset();
            }
        }
        
        protected abstract IState StartRootState();
    }
}