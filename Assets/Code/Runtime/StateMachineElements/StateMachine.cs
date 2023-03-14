using System.Collections.Generic;
using System.Linq;
using Code.Runtime.BehaviourGraphSerialization;
using Code.Runtime.Initialization;
using UnityEngine;
using Zenject;

namespace Code.Runtime.StateMachineElements
{
    public class StateMachine : MonoBehaviour
    {
        protected BehaviourTree BehaviourTree;
        [SerializeField] protected BehaviourTreeAsset BehaviourTreeAsset;
        [SerializeField] private GameObjectContext _dependencyContainer;
        [SerializeField] private List<SerializableParameter>_rootArguments;
        [Inject] private BehaviourTreeBuilder _behaviourTreeBuilder;
        
        protected virtual void Start()
        {
            BehaviourTree = _behaviourTreeBuilder.BuildTree(BehaviourTreeAsset, _dependencyContainer);
            BehaviourTree.StartRootState(_rootArguments.Select(parameter => parameter.PlainObject ?? parameter.UnityObject));
        }

        protected virtual  void Update()
        {
            BehaviourTree.CurrentState.Update();

            IReadOnlyList<ITrigger> currentStateTriggers = BehaviourTree.TriggersFrom(BehaviourTree.CurrentState);
            for (int i = 0; i < currentStateTriggers.Count; i++)
            {
                ITrigger trigger = currentStateTriggers[i];
                if (trigger.IsHit())
                {
                    IState beforeTransition = BehaviourTree.CurrentState;
                    BehaviourTree.Transition(trigger.PrepareTarget());
                    BehaviourTree.ResetTriggers(beforeTransition);
                }
            }
        }
    }
}