using System.Collections.Generic;
using System.Linq;
using Code.Runtime.Innitialization;
using UnityEngine;
using Zenject;

namespace Code.Runtime
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private GameObjectContext _dependencyContainer;
        [SerializeField] private BehaviourTreeAsset _behaviourTreeAsset;
        [SerializeField] private List<SerializableParameter>_rootArguments;
        [Inject] private BehaviourTreeBuilder _behaviourTreeBuilder;
        private BehaviourTree _behaviourTree;
        
        private void Start()
        {
            _behaviourTree = _behaviourTreeBuilder.BuildTree(_behaviourTreeAsset, _dependencyContainer);
            _behaviourTree.StartRootState(_rootArguments.Select(parameter => parameter.PlainObject ?? parameter.UnityObject));
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