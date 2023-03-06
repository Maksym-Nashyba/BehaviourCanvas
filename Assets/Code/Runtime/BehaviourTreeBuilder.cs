using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Code.Runtime.BehaviourElementModels;
using Zenject;

namespace Code.Runtime
{
    public sealed class BehaviourTreeBuilder
    {
        private readonly Dictionary<BehaviourTreeAsset, (List<StateModel> states, List<TriggerModel> triggers)> _cachedModels;
        private readonly ModelSerializer _deserializer;

        public BehaviourTreeBuilder()
        {
            _cachedModels =
                new Dictionary<BehaviourTreeAsset, (List<StateModel> states, List<TriggerModel> triggers)>();
            _deserializer = new ModelSerializer();
        }

        public BehaviourTree BuildTree(BehaviourTreeAsset blueprint, GameObjectContext dependencyContainer)
        {
            (List<StateModel> states, List<TriggerModel> triggers) deserializedModels = Deserialize(blueprint);
            IReadOnlyDictionary<int, IState> stateObjects = InstantiateBehaviourElements<IState>(deserializedModels.states);
            IReadOnlyDictionary<int, ITrigger> triggerObjects = InstantiateBehaviourElements<ITrigger>(deserializedModels.triggers);
            InjectDependencies(stateObjects.Values, triggerObjects.Values, dependencyContainer);
            InjectTriggerTargets(deserializedModels.triggers, stateObjects, triggerObjects);
            return new BehaviourTree(stateObjects.Values.ToList(), triggerObjects.Values.ToList());
        }

        private void InjectTriggerTargets(IReadOnlyList<TriggerModel> triggerBlueprints,
            IReadOnlyDictionary<int, IState> states, IReadOnlyDictionary<int, ITrigger> triggers)
        {
            foreach (TriggerModel triggerBlueprint in triggerBlueprints)
            {
                ITrigger trigger = triggers[triggerBlueprint.Id];
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
        
        private IReadOnlyDictionary<int, T> InstantiateBehaviourElements<T>(IEnumerable<BehaviourElementModel> models)
        {
            IEnumerable<(T instantiatedObject, int id)> instances = Reflection.GetStateTypes(models.Select(model => model.Model.Name))
                .Select(type => (T)Activator.CreateInstance(type))
                .Zip(models, (instance, model) => (instance, model.Id));

            Dictionary<int, T> result = new Dictionary<int, T>();
            foreach ((T instantiatedObject, int id) instance in instances)
            {
                result.Add(instance.id, instance.instantiatedObject);
            }
            return result;
        }

        private (List<StateModel> states, List<TriggerModel> triggers) Deserialize(BehaviourTreeAsset blueprint)
        {
            if (_cachedModels.ContainsKey(blueprint)) return _cachedModels[blueprint];
            List<StateModel> stateModels = _deserializer.DeserializeStateModels(blueprint.BehaviourTreeXML);
            List<TriggerModel> triggerModels = _deserializer.DeserializeTriggerModels(blueprint.BehaviourTreeXML);
            (List<StateModel> states, List<TriggerModel> triggers) result = (stateModels, triggerModels);
            _cachedModels.Add(blueprint, result);
            return result;
        }
    }
}