using System;
using System.Collections.Generic;
using System.Linq;
using Code.Runtime;
using Code.Runtime.BehaviourElementModels;

namespace Code.Editor
{
    public class BehaviourCanvas
    {
        public StateModel RootState => _states.First(state => state.Id == -1);
        public IReadOnlyList<StateModel> States => _states;
        public IReadOnlyList<TriggerModel> Triggers => _triggers;
        
        private List<StateModel> _states;
        private List<TriggerModel> _triggers;

        private IdStore _idStore;
        private readonly BehaviourCanvasView _view;
        private readonly ModelBuilder _modelBuilder;

        public BehaviourCanvas(BehaviourCanvasView view, ModelBuilder modelBuilder)
        {
            _view = view;
            _modelBuilder = modelBuilder;
        }

        public void Initialize(List<StateModel> states, List<TriggerModel> triggers)
        {
            SubscribeOnEvents();
            
            _states = new List<StateModel>();
            _triggers = new List<TriggerModel>();
            _idStore = new IdStore(GetIncrementedBiggestId(states, triggers));

            if (states.Count == 0 && triggers.Count == 0) return;
            CreateBehaviourTree(states, triggers);
        }

        public void UnsubscribeFromEvents()
        {
            _modelBuilder.CreateState -= CreateState;
            _modelBuilder.CreateTrigger += CreateTrigger;
            _view.DeleteTreeModel -= DeleteTreeModel;
            _view.SetRootState += SetRootState;
        }

        private void SubscribeOnEvents()
        {
            _modelBuilder.CreateState += CreateState;
            _modelBuilder.CreateTrigger += CreateTrigger;
            _view.DeleteTreeModel += DeleteTreeModel;
            _view.SetRootState += SetRootState;
        }

        private int GetIncrementedBiggestId(IReadOnlyList<StateModel> states, IReadOnlyList<TriggerModel> triggers)
        {
            int firstId = states.Count != 0 ? states[^1].Id : -2;
            int secondId = triggers.Count != 0 ? triggers[^1].Id : -2;
            return Math.Max(firstId, secondId) + 1;
        }

        private void CreateBehaviourTree(List<StateModel> states, List<TriggerModel> triggers)
        {
            foreach (StateModel state in states)
            {
                CreateState(state);
            }
            foreach (TriggerModel trigger in triggers)
            {
                CreateTrigger(trigger);
            }
        }

        private void SetRootState(int stateId)
        {
            int oldRootStateNewId = _idStore.ID;
            RootState.Id = oldRootStateNewId;
            _states.First(state => state.Id == stateId).Id = -1;
            _view.SetRootNode(stateId, oldRootStateNewId);
        }

        private void CreateState(Model model)
        {
            StateModel state = new StateModel(_idStore.ID, model);
            CreateState(state);
        }
        
        private void CreateState(StateModel state)
        {
            _states.Add(state);
            _view.CreateNodeView(state, _view.FindNodePosition(state.Id));
        }
        
        private void CreateTrigger(Model model)
        {
            TriggerModel trigger = new TriggerModel(_idStore.ID, model, false);
            CreateTrigger(trigger);
        }
        
        private void CreateTrigger(TriggerModel trigger)
        {
            _triggers.Add(trigger);
            _view.CreateTriggerView(trigger, _view.FindNodePosition(trigger.Id));
        }
        
        private void DeleteTreeModel(int modelID)
        {
            foreach (StateModel state in _states.Where(state => state.Id == modelID))
            {
                DeleteState(state);
                _idStore = new IdStore(GetIncrementedBiggestId(_states, _triggers));
                return;
            }
            foreach (TriggerModel trigger in _triggers.Where(trigger => trigger.Id == modelID))
            {
                DeleteTrigger(trigger);
                _idStore = new IdStore(GetIncrementedBiggestId(_states, _triggers));
                return;
            }
        }

        private void DeleteState(StateModel state)
        {
            _states.Remove(state);
            _view.DeleteNode(state.Id);
        }
        
        private void DeleteTrigger(TriggerModel trigger)
        {
            _triggers.Remove(trigger);
            _view.DeleteNode(trigger.Id);
        }
    }
}