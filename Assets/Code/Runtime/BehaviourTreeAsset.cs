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

        public void UpdateBehaviourTreeXML(TextAsset behaviourTreeXML)
        {
            _behaviourTreeXML = behaviourTreeXML;
        }
        
        public void UpdateNodeTreeXML(TextAsset nodeTreeXML)
        {
            _nodeTreeXML = nodeTreeXML;
        }
    }
}