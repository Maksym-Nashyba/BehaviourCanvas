using System;
using UnityEngine;

namespace Code.Runtime.StateMachineElements
{
    [Serializable]
    internal struct SerializableParameter
    {
        [SerializeReference] public object PlainObject;
        public UnityEngine.Object UnityObject;
        public int IntValue;
        public float FloatValue;
        public Vector2 Vector2Value;
        public Vector3 Vector3Value;
    }
}