using System.Collections.Generic;
using System.IO;
using System.Linq;
using Code.Runtime.BehaviourGraphSerialization;

namespace Code.Editor
{
    public static class ModelGraphValidator
    {
        public static void Validate(IReadOnlyModelGraph graph)
        {
            ValidateIsntEmpty(graph);
            ValidateHasOneRoot(graph);
            ValidateStatesHaveSource(graph);
            ValidateTriggersHaveTargets(graph);
            ValidateTriggersHaveOneSource(graph);
            ValidateTriggerToStateParameters(graph);
        }

        private static void ValidateHasOneRoot(IReadOnlyModelGraph graph)
        {
            List<IReadOnlyBehaviourElementModel> roots = new List<IReadOnlyBehaviourElementModel>();
            foreach (IReadOnlyBehaviourElementModel state in graph.GetStates().Values)
            {
                if (state.GetId() == StateModel.RootId) roots.Add(state);
            }

            if (roots.Count < 1) throw new InvalidDataException("There is no root state on this graph");
            if (roots.Count > 1) throw new InvalidDataException("There can only be one root state");
        }

        private static void ValidateIsntEmpty(IReadOnlyModelGraph graph)
        {
            if (graph.GetStates().Count < 1) throw new InvalidDataException("The graph is empty");
        }

        private static void ValidateTriggerToStateParameters(IReadOnlyModelGraph graph)
        {
            foreach (IReadOnlyTriggerModel trigger in graph.GetTriggers().Values)
            {
                Model targetState = trigger.GetTargetModels()[0].GetModel();
                if (!trigger.GetModel().Parameters.CanMapTo(targetState.Parameters))
                {
                    throw new InvalidDataException("Parameters differ! " +
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
                if(state.GetTargetModels() is null) continue;
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
            HashSet<IReadOnlyBehaviourElementModel> uncheckedStates = graph.GetStates().Values.ToHashSet();
            foreach (IReadOnlyTriggerModel trigger in graph.GetTriggers().Values)
            {
                if (uncheckedStates.Contains(trigger.GetTargetModels()[0]))
                {
                    uncheckedStates.Remove(trigger.GetTargetModels()[0]);
                }
            }

            if (uncheckedStates.Count > 1)
            {
                throw new InvalidDataException("Some states don't have source");
            }
        }
    }
}