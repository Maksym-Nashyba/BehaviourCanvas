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
            ValidateTriggersHaveSource(graph);
            ValidateTriggersHaveTargets(graph);
            ValidateTriggerToStateParameters(graph);
        }

        private static void ValidateTriggerToStateParameters(IReadOnlyModelGraph graph)
        {
            foreach (IReadOnlyTriggerModel trigger in graph.GetTriggers().Values)
            {
                Model targetState = trigger.GetTargetModels()[0].GetModel();
                if (!trigger.GetModel().Parameters.Select((tuple, _) => tuple.parameterType)
                        .SequenceEqual(targetState.Parameters.Select((tuple, _) => tuple.parameterType)))
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
        
        private static void ValidateTriggersHaveSource(IReadOnlyModelGraph graph)
        {
            Dictionary<IReadOnlyTriggerModel, IReadOnlyBehaviourElementModel> triggerToSource =
                new Dictionary<IReadOnlyTriggerModel, IReadOnlyBehaviourElementModel>();
            foreach (IReadOnlyBehaviourElementModel state in graph.GetStates().Values)
            {
                foreach (IReadOnlyBehaviourElementModel targetTriggers in state.GetTargetModels())
                {
                    
                }
            }
        }
        
        private static void ValidateStatesHaveSource(IReadOnlyModelGraph graph)
        {
            throw new NotImplementedException();
        }
        

    }
}