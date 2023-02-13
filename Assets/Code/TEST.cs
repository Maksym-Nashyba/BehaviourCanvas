using System;
using Code.Templates;
using UnityEngine;

namespace Code
{
    public class TEST : MonoBehaviour
    {
        private void Start()
        {
            StateData d = new StateData("ShitassState", ("kok", typeof(Vector3)));
            string generatedClass = new StateTemplate();
        }
    }
}