using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Code.Runtime
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private BehaviourTreeAsset _behaviourTreeAsset;
        [SerializeField] private List<SerializableParameter>_rootArguments;
        [Inject] private BehaviourTreeBuilder _behaviourTreeBuilder;
        private BehaviourTree _behaviourTree;
        
        private void Start()
        {
            
            
            return;
            _behaviourTree = _behaviourTreeBuilder.BuildTree(_behaviourTreeAsset);
            _behaviourTree.StartRootState();
        }

        private void Update()
        {
            _behaviourTree.CurrentState.Update();

            IReadOnlyList<ITrigger> currentStateTriggers = _behaviourTree.TriggersFrom(_behaviourTree.CurrentState);
            for (int i = 0; i < currentStateTriggers.Count; i++)
            {
                ITrigger trigger = currentStateTriggers[i];
                if (trigger.IsHit())
                {
                    IState beforeTransition = _behaviourTree.CurrentState;
                    _behaviourTree.Transition(trigger.PrepareTarget());
                    _behaviourTree.ResetTriggers(beforeTransition);
                }
            }
        }
    }
}