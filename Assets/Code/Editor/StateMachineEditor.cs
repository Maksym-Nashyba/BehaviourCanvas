﻿using System;
using System.Linq;
using System.Reflection;
using Code.Runtime;
using Code.Runtime.BehaviourElementModels;
using Code.Runtime.Innitialization;
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
        private (Type, string)[] _parameters;
        
        private void OnEnable()
        {
            _target = target as StateMachine;
            _behaviourTreeAssetProperty = serializedObject.FindProperty("_behaviourTreeAsset");
            _parametersArrayProperty = serializedObject.FindProperty("_rootArguments");
            _dependencyContainerProperty = serializedObject.FindProperty("_dependencyContainer");
            _behaviourTreeAsset = null;
            _parameters = null;
            _behaviourTreeAsset = ResolveBehaviourAsset();
            if (_behaviourTreeAsset != null) _parameters = ResolveRootParameters();
        }
        
        private void OnDisable()
        {
            _parameters = null;
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
                _parameters = null;
                _behaviourTreeAsset = ResolveBehaviourAsset();
                if (_behaviourTreeAsset != null) _parameters = ResolveRootParameters();
            }

            if (_parameters == null)
            {
                GUILayout.EndVertical();
                return;
            }
            GUILayout.Label("Root State Arguments");
            EditorGUI.indentLevel++;
            for (int i = 0; i < _parameters.Length; i++)
            { 
                DisplayAppropriateField(
                    _parametersArrayProperty.GetArrayElementAtIndex(i),
                    _parameters[i].Item1,
                    _parameters[i].Item2);
            }
            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
        }

        private void DisplayAppropriateField(SerializedProperty property, Type parameterType, string name)
        {
            if (typeof(Object).IsAssignableFrom(parameterType))
            {
                property.serializedObject.Update();
                property = property.FindPropertyRelative("UnityObject");
                EditorGUILayout.ObjectField(property, typeof(Object), new GUIContent(name));
                if (property.objectReferenceValue != null)
                {
                    if (property.objectReferenceValue.GetType() != parameterType) throw new Exception($"The type of this field is {parameterType}");
                }
            }
            else if (parameterType == typeof(Int32))
            {
                DrawValueTypeField<int>(property, name, EditorGUILayout.IntField);
            }
            else if (parameterType == typeof(Single))
            {
                DrawValueTypeField<float>(property, name, EditorGUILayout.FloatField);
            }
            else if (parameterType == typeof(Vector2))
            {
                DrawValueTypeField<Vector2>(property, name, EditorGUILayout.Vector2Field);
            }
            else if (parameterType == typeof(Vector3))
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
        
        private (Type, string)[] ResolveRootParameters()
        {
            ModelSerializer modelSerializer = new ModelSerializer();
            ModelGraph graph = modelSerializer.DeserializeModelGraph(_behaviourTreeAsset!.BehaviourTreeXML);
            IReadOnlyBehaviourElementModel rootState = graph.GetRootState();
            (Type, string)[] parameters = Reflection.GetStateParameters(rootState.GetModel().Name);
            if (_parametersArrayProperty.arraySize != parameters.Length)
            {
                _parametersArrayProperty.arraySize = parameters.Length;
                _parametersArrayProperty.serializedObject.ApplyModifiedProperties();
            }
            return parameters;
        }
    }
}