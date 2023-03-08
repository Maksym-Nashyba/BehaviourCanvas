using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Code.Runtime.BehaviourElementModels;

namespace Code.Editor
{
    public static class ModelGraphValidator
    {
        public static void Validate(IReadOnlyModelGraph graph)
        {
            ValidateStatesHaveSource(graph);
            ValidateTriggersHaveOneSource(graph);
            ValidateTriggersHaveTargets(graph);
            ValidateTriggerToStateParameters(graph);
        }

        private static void ValidateTriggerToStateParameters(IReadOnlyModelGraph graph)
        {
            foreach (IReadOnlyTriggerModel trigger in graph.GetTriggers().Values)
            {
                Model targetState = trigger.GetTargetModels()[0].GetModel();
                if (!Enumerable.SequenceEqual(
                        trigger.GetModel().Parameters.Select((tuple, _) => tuple.typeName), 
                        targetState.Parameters.Select((tuple, _) => tuple.typeName)))
                {
                    throw new InvalidDataException("Prameters differ! " +
                                                   $"Trigger {trigger.GetModel().Name}_{trigger.GetId()} " +
                                                   $" and State {targetState.Name}_{trigger.GetTargetModels()[0].GetId()}");
                }
            }
        }
        
        private static void ValidateTriggersHaveTargets(IReadOnlyModelGraph graph)
        {
            foreach (IReadOnlyTriggerModel trigger in graph.GetTriggers().Values)
            {
                if (trigger.GetTargetModels() == null || trigger.GetTargetModels().Count < 1)
                {
                    throw new InvalidDataException($"Trigger {trigger.GetModel().Name}_{trigger.GetId()} doesn't have a target");
                }
                
                if (trigger.GetTargetModels().Count > 1)
                {
                    throw new InvalidDataException($"Trigger {trigger.GetModel().Name}_{trigger.GetId()} has too many targets");
                }
            }
        }
        
        private static void ValidateTriggersHaveOneSource(IReadOnlyModelGraph graph)
        {
            Dictionary<IReadOnlyTriggerModel, IReadOnlyBehaviourElementModel> triggerToSource =
                new Dictionary<IReadOnlyTriggerModel, IReadOnlyBehaviourElementModel>();
            foreach (IReadOnlyBehaviourElementModel state in graph.GetStates().Values)
            {
                foreach (IReadOnlyTriggerModel targetTrigger in state.GetTargetModels().Cast<IReadOnlyTriggerModel>())
                {
                    if (triggerToSource.ContainsKey(targetTrigger))
                    {
                        IReadOnlyBehaviourElementModel existingSource = triggerToSource[targetTrigger];
                        IReadOnlyBehaviourElementModel newSource = state;
                        throw new InvalidDataException($"Trigger {targetTrigger.GetModel().Name}_{targetTrigger.GetId()}" +
                                                       $" has more than one source: {existingSource.GetId()} and {newSource.GetId()}");
                    }
                    triggerToSource.Add(targetTrigger, state);
                }
            }

            bool allTriggersAdded = graph.GetTriggers().Values.Intersect(triggerToSource.Keys).Count() == triggerToSource.Keys.Count;
            if (!allTriggersAdded) throw new InvalidDataException("Some triggers have no source");
        }
        
        private static void ValidateStatesHaveSource(IReadOnlyModelGraph graph)
        {
            throw new NotImplementedException();
        }
    }
}