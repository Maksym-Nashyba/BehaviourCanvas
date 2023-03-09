using System;
using System.Linq;
using System.Reflection;
using Code.Runtime;
using Code.Runtime.BehaviourGraphSerialization;
using Code.Runtime.Initialization;
using Code.Runtime.StateMachineElements;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Code.Editor
{
    [CustomEditor(typeof(StateMachine))]
    public class StateMachineEditor : UnityEditor.Editor
    {
        private StateMachine _target;
        private SerializedProperty _behaviourTreeAssetProperty;
        private SerializedProperty _parametersArrayProperty;
        private SerializedProperty _dependencyContainerProperty;
        private BehaviourTreeAsset _behaviourTreeAsset;
        private ParameterSet _parameters;
        private bool _foundValidParameters;

        private void OnEnable()
        {
            _target = target as StateMachine;
            _behaviourTreeAssetProperty = serializedObject.FindProperty("_behaviourTreeAsset");
            _dependencyContainerProperty = serializedObject.FindProperty("_dependencyContainer");
            _parametersArrayProperty = serializedObject.FindProperty("_rootArguments");
            _behaviourTreeAsset = null;
            _parameters = default;
            _behaviourTreeAsset = ResolveBehaviourAsset();
            if (_behaviourTreeAsset != null) _parameters = ResolveRootParameters();
        }
        
        private void OnDisable()
        {
            _parameters = default;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_dependencyContainerProperty, new GUIContent("Dependency Container"));
            serializedObject.ApplyModifiedProperties();
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

            if (_foundValidParameters)
            {
                GUILayout.EndVertical();
                return;
            }
            GUILayout.Label("Root State Arguments");
            EditorGUI.indentLevel++;
            for (int i = 0; i < _parameters.Count; i++)
            { 
                DisplayAppropriateField(
                    _parametersArrayProperty.GetArrayElementAtIndex(i), 
                    _parameters.Parameters[i]);
            }
            serializedObject.ApplyModifiedProperties();
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
                DrawValueTypeField<int>(property, name, EditorGUILayout.IntField);
            }
            else if (parameter.Type == typeof(Single))
            {
                DrawValueTypeField<float>(property, name, EditorGUILayout.FloatField);
            }
            else if (parameter.Type == typeof(Vector2))
            {
                DrawValueTypeField<Vector2>(property, name, EditorGUILayout.Vector2Field);
            }
            else if (parameter.Type == typeof(Vector3))
            {
                DrawValueTypeField<Vector3>(property, name, EditorGUILayout.Vector3Field);
            }
        }

        private void DrawValueTypeField<T>(SerializedProperty property, string name, Func<string, T, GUILayoutOption[], T> field)
        {
            property.serializedObject.Update();
            property = property.FindPropertyRelative("PlainObject");
            object last = property.managedReferenceValue ?? default(T);
            EditorGUI.BeginChangeCheck();
            property.managedReferenceValue = field.Invoke(name, (T)last, null);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }
        
        private BehaviourTreeAsset ResolveBehaviourAsset()
        {
            FieldInfo field = _target!.GetType().GetField("_behaviourTreeAsset", BindingFlags.NonPublic | BindingFlags.Instance);
            return field!.GetValue(_target) as BehaviourTreeAsset;
        }
        
        private ParameterSet ResolveRootParameters()
        {
            ModelSerializer modelSerializer = new ModelSerializer();
            ModelGraph graph = modelSerializer.DeserializeModelGraph(_behaviourTreeAsset!.BehaviourTreeXML);
            IReadOnlyBehaviourElementModel rootState = graph.GetRootState();
            ParameterSet parameters = Reflection.GetStateParameters(rootState.GetModel().Name);
            if (_parametersArrayProperty.arraySize != parameters.Count)
            {
                _parametersArrayProperty.arraySize = parameters.Count;
                _parametersArrayProperty.serializedObject.ApplyModifiedProperties();
            }
            return parameters;
        }
    }
}