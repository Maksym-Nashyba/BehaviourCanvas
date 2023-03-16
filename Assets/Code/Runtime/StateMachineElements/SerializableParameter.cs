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

        public object GetValue(Type type)
        {
            if (type.IsSubclassOf(typeof(UnityEngine.Object))) return UnityObject;
            if (type.IsValueType)
            {
                if (type == typeof(int)) return IntValue;
                if (type == typeof(float)) return FloatValue;
                if (type == typeof(Vector2)) return Vector2Value;
                if (type == typeof(Vector3)) return Vector3Value;
            }
            return PlainObject;
        }
    }
}