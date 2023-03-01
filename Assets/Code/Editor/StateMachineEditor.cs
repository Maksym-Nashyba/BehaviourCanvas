using Code.Runtime;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    [CustomEditor(typeof(StateMachine))]
    public class StateMachineEditor : UnityEditor.Editor
    {
        private StateMachine _target;
        private SerializedProperty _behaviourTreeAssetProperty;
        private SerializedProperty _parametersArrayProperty;
        
        private void OnEnable()
        {
            _target = target as StateMachine;
            _behaviourTreeAssetProperty = serializedObject.FindProperty("_behaviourTreeAsset");
            _parametersArrayProperty = serializedObject.FindProperty("_rootArguments");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_behaviourTreeAssetProperty, new GUIContent("Behaviour Tree"));
        }
    }
}