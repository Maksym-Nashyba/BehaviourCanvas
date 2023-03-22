using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Runtime.BehaviourGraphSerialization
{
    public class BehaviourTreeAsset : ScriptableObject
    {
        public TextAsset GraphXML => _graphXML;
        
        #if UNITY_EDITOR
            public TextAsset MarkupXML => _markupXML;
        #endif
        
        [SerializeField] private TextAsset _graphXML;
        [SerializeField] private TextAsset _markupXML;

        public void UpdateBehaviourTreeXML(TextAsset behaviourTreeXML)
        {
            _graphXML = behaviourTreeXML;
        }
        
        public void UpdateMarkupXML(TextAsset nodeTreeXML)
        {
            _markupXML = nodeTreeXML;
        }
    }
}