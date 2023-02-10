using UnityEngine;

namespace Code
{
    public class StateMachine : MonoBehaviour
    {
        private State _currentState;

        private void Update()
        {
            foreach (Transition transition in _currentState.Transitions)
            {
                if(!transition.ShouldFire())continue;

                _currentState = transition.Fire();
                transition.Reset();
                break;
            }
            
            _currentState?.Update();
        }
    }
}