using UnityEngine;

namespace Code.Editor
{
    [CreateAssetMenu(fileName = "BehaviourTreeAsset", menuName = "BehaviourTreeAsset", order = 0)]
    public class BehaviourTreeAsset : ScriptableObject
    {
        public TextAsset BehaviourTreeXML => _behaviourTreeXML;
        public TextAsset EditorTreeXML => _editorTreeXML;
    
        [SerializeField] private TextAsset _behaviourTreeXML;
        [SerializeField] private TextAsset _editorTreeXML;

        public void UpdateAsset(TextAsset behaviourTreeXML, TextAsset editorTreeXML)
        {
            _behaviourTreeXML = behaviourTreeXML;
            _editorTreeXML = editorTreeXML;
        }
    }
}