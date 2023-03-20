using UnityEngine;

namespace Code.Runtime.BehaviourGraphSerialization
{
    public class BehaviourTreeAsset : ScriptableObject
    {
        public TextAsset BehaviourTreeXML => _behaviourTreeXML;
        
        #if UNITY_EDITOR
            public TextAsset MarkupXML => _markupXML;
        #endif
        
        [SerializeField] private TextAsset _behaviourTreeXML;
        [SerializeField] private TextAsset _markupXML;

        public void UpdateBehaviourTreeXML(TextAsset behaviourTreeXML)
        {
            _behaviourTreeXML = behaviourTreeXML;
        }
        
        public void UpdateMarkupXML(TextAsset nodeTreeXML)
        {
            _markupXML = nodeTreeXML;
        }
    }
}