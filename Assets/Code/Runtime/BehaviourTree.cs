using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Code
{
    public abstract class BehaviourTree : MonoBehaviour
    {
        protected IState CurrentState { get; private set; }
        protected IReadOnlyList<IState> States;
        protected ReadOnlyDictionary<IState, List<ITrigger>> Triggers;

        private void Start()
        {
            CurrentState = StartRootState();
        }

        private void Update()
        {
            CurrentState.Update();

            IReadOnlyList<ITrigger> currentStateTriggers = Triggers[CurrentState];
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
            IReadOnlyList<ITrigger> stateTriggers = Triggers[state];
            for (int i = 0; i < stateTriggers.Count; i++)
            {
                stateTriggers[i].Reset();
            }
        }
        
        protected abstract IState StartRootState();
    }
}