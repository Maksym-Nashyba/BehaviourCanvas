using System;
using UnityEngine;

namespace Code.Runtime
{
    [Serializable]
    internal struct SerializableParameter
    {
        [SerializeReference] public object PlainObject;
        public UnityEngine.Object UnityObject;
    }
}