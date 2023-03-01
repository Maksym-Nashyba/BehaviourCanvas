using UnityEngine;

namespace Code.Runtime
{
    public class BehaviourTreeAsset : ScriptableObject
    {
        public TextAsset BehaviourTreeXML => _behaviourTreeXML;
        
        #if UNITY_EDITOR
            public TextAsset NodeTreeXML => _nodeTreeXML;
        #endif
        
        [SerializeField] private TextAsset _behaviourTreeXML;
        [SerializeField] private TextAsset _nodeTreeXML;

        public void UpdateAsset(TextAsset behaviourTreeXML, TextAsset nodeTreeXML)
        {
            _behaviourTreeXML = behaviourTreeXML;
            _nodeTreeXML = nodeTreeXML;
        }
    }
}