using System;
using System.Reflection;
using Code.Runtime;
using Code.Runtime.BehaviourGraphSerialization;
using Code.Runtime.StateMachineElements;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Editor
{
    [CustomEditor(typeof(StateMachine))]
    public class StateMachineEditor : UnityEditor.Editor
    {
        protected StateMachine Target;
        private SerializedProperty _behaviourTreeAssetProperty;
        private SerializedProperty _parametersArrayProperty;
        private SerializedProperty _dependencyContainerProperty;
        private BehaviourTreeAsset _behaviourTreeAsset;
        private ParameterSet _parameters;
        private bool _foundValidParameters;

        private void OnEnable()
        {
            Target = target as StateMachine;
            _behaviourTreeAssetProperty = serializedObject.FindProperty("BehaviourTreeAsset");
            _dependencyContainerProperty = serializedObject.FindProperty("_dependencyContainer");
            _parametersArrayProperty = serializedObject.FindProperty("_rootArguments");
            _behaviourTreeAsset = null;
            _foundValidParameters = false;
            _behaviourTreeAsset = ResolveBehaviourAsset();
            if (_behaviourTreeAsset != null) _parameters = ResolveRootParameters();
        }
        
        private void OnDisable()
        {
            _foundValidParameters = false;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (_dependencyContainerProperty != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(_dependencyContainerProperty, new GUIContent("Dependency Container"));
                if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
            }
            GUILayout.BeginVertical("box");
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_behaviourTreeAssetProperty, new GUIContent("Behaviour Tree"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
                _behaviourTreeAsset = null;
                _parameters = default;
                _behaviourTreeAsset = ResolveBehaviourAsset();
                if (_behaviourTreeAsset != null) _parameters = ResolveRootParameters();
            }

            if (!_foundValidParameters)
            {
                GUILayout.EndVertical();
                return;
            }
            GUILayout.Label("Root State Arguments");
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < _parameters.Count; i++)
            { 
                DisplayAppropriateField(
                    _parametersArrayProperty.GetArrayElementAtIndex(i), 
                    _parameters.Parameters[i]);
            }
            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
        }

        private void DisplayAppropriateField(SerializedProperty property, Parameter parameter)
        {
            if (typeof(Object).IsAssignableFrom(parameter.Type))
            {
                property.serializedObject.Update();
                property = property.FindPropertyRelative("UnityObject");
                EditorGUILayout.ObjectField(property, typeof(Object), new GUIContent(parameter.Name));
                if (property.objectReferenceValue != null)
                {
                    if (property.objectReferenceValue.GetType() != parameter.Type) throw new Exception($"The type of this field is {parameter.Type}");
                }
            }
            else if (parameter.Type == typeof(Int32))
            {
                DrawValueTypeField(property, "IntValue", parameter.Name);
            }
            else if (parameter.Type == typeof(Single))
            {
                DrawValueTypeField(property, "FloatValue", parameter.Name);
            }
            else if (parameter.Type == typeof(Vector2))
            {
                DrawValueTypeField(property, "Vector2Value", parameter.Name);
            }
            else if (parameter.Type == typeof(Vector3))
            {
                DrawValueTypeField(property, "Vector3Value", parameter.Name);
            }
            else
            {
                DrawValueTypeField(property, "PlainObject", parameter.Name);
            }
        }

        private void DrawValueTypeField(SerializedProperty property, string relativePropertyName, string name)
        {
            property.serializedObject.Update();
            property = property.FindPropertyRelative(relativePropertyName);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(property, new GUIContent(name));
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }
        
        protected virtual BehaviourTreeAsset ResolveBehaviourAsset()
        {
            FieldInfo field = typeof(StateMachine).GetField("BehaviourTreeAsset", BindingFlags.NonPublic | BindingFlags.Instance);
            return field!.GetValue(Target) as BehaviourTreeAsset;
        }
        
        private ParameterSet ResolveRootParameters()
        {
            ModelSerializer modelSerializer = new ModelSerializer();
            ModelGraph graph = modelSerializer.DeserializeModelGraph(_behaviourTreeAsset!.GraphXML);
            IReadOnlyBehaviourElementModel rootState = graph.GetRootState();
            ParameterSet parameters = Reflection.GetStateParameters(rootState.GetModel().Name);
            if (_parametersArrayProperty.arraySize != parameters.Count)
            {
                _parametersArrayProperty.arraySize = parameters.Count;
                _parametersArrayProperty.serializedObject.ApplyModifiedProperties();
            }

            _foundValidParameters = true;
            return parameters;
        }
    }
}