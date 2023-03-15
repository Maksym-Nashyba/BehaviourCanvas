using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Code.Runtime.BehaviourGraphSerialization;
using Code.Runtime.Initialization;
using Code.Runtime.States;
using Code.Runtime.Triggers;
using Zenject;

namespace Code.Runtime.StateMachineElements
{
    public sealed class BehaviourTreeBuilder
    {
        private readonly Dictionary<BehaviourTreeAsset, ModelGraph> _cachedModels;
        private readonly ModelSerializer _deserializer;

        public BehaviourTreeBuilder()
        {
            _cachedModels =
                new Dictionary<BehaviourTreeAsset, ModelGraph>();
            _deserializer = new ModelSerializer();
        }

        public BehaviourTree BuildTree(BehaviourTreeAsset blueprint, GameObjectContext dependencyContainer)
        {
            ModelGraph deserializedModels = Deserialize(blueprint);
            IReadOnlyDictionary<int, IState> stateObjects = InstantiateBehaviourElements<IState>(deserializedModels.GetIDStates().Values);
            IReadOnlyDictionary<int, ITrigger> triggerObjects = InstantiateBehaviourElements<ITrigger>(deserializedModels.GetIDTriggers().Values);
            InjectDependencies(stateObjects.Values, triggerObjects.Values, dependencyContainer);
            InjectTriggerTargets(deserializedModels.GetIDTriggers().Values.WhereNotNull(), stateObjects, triggerObjects);

            Dictionary<IState, IReadOnlyList<ITrigger>> orderedTriggers
                = deserializedModels.GetIDStates().Values
                .Select(model => model.GetTargetModels())
                .Select(triggerList =>
                    triggerList.Select(triggerModel => triggerObjects[triggerModel.GetId()]).ToList())
                .Cast<IReadOnlyList<ITrigger>>()
                .Zip(stateObjects.Values, (triggers, state) => (triggers, state))
                .ToDictionary(tuple => tuple.state, tuple => tuple.triggers);

            return new BehaviourTree(stateObjects.Values.ToList(), orderedTriggers);
        }

        private void InjectTriggerTargets(IEnumerable<IReadOnlyTriggerModel> triggerBlueprints,
            IReadOnlyDictionary<int, IState> states, IReadOnlyDictionary<int, ITrigger> triggers)
        {
            foreach (IReadOnlyTriggerModel triggerBlueprint in triggerBlueprints)
            {
                ITrigger trigger = triggers[triggerBlueprint.GetId()];
                IState targetState = states[triggerBlueprint.GetTargetModels()[0].GetId()];

                FieldInfo field = trigger.GetType().GetField("_targetState", BindingFlags.Instance | BindingFlags.NonPublic);
                field!.SetValue(trigger, targetState);
            }
        }
        
        private void InjectDependencies(IEnumerable<IState> states, IEnumerable<ITrigger> triggers, GameObjectContext dependencyContainer)
        {
            foreach (IState state in states) dependencyContainer.Container.Inject(state);
            foreach (ITrigger trigger in triggers) dependencyContainer.Container.Inject(trigger);
        }
        
        private IReadOnlyDictionary<int, T> InstantiateBehaviourElements<T>(IEnumerable<IReadOnlyBehaviourElementModel> models)
        {
            Type assemblyAnchor = typeof(T) == typeof(IState)
                ? typeof(StateAssemblyMarker)
                : typeof(TriggerAssemblyMarker);
            IEnumerable<(T instantiatedObject, int id)> instances = Reflection.GetStateTypes(models.Select(model => model.GetModel().Name), assemblyAnchor)
                .Select(type => (T)Activator.CreateInstance(type))
                .Zip(models, (instance, model) => (instance, model.GetId()));

            Dictionary<int, T> result = new Dictionary<int, T>();
            foreach ((T instantiatedObject, int id) instance in instances)
            {
                result.Add(instance.id, instance.instantiatedObject);
            }
            return result;
        }
        
        private ModelGraph Deserialize(BehaviourTreeAsset blueprint)
        {
            if (_cachedModels.ContainsKey(blueprint)) return _cachedModels[blueprint];
            ModelGraph result = _deserializer.DeserializeModelGraph(blueprint.BehaviourTreeXML);
            _cachedModels.Add(blueprint, result);
            return result;
        }
    }
}