using Code.Editor;
using UnityEditor;

namespace Code.Tests.BehaviourCanvasRuntimeTests
{
    [CustomEditor(typeof(TestStateMachine))]
    public class TestStateMachineEditor : StateMachineEditor
    {
    }
}