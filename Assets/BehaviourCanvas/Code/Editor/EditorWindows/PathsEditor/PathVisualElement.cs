using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace BehaviourCanvas.Code.Editor.EditorWindows.PathsEditor
{
    public class PathVisualElement : VisualElement
    {
        public string PathText => ("Assets/"+_textField.value).TrimEnd('/');
        
        private readonly Label _label;
        private readonly TextField _textField;
        private readonly Button _confirmButton;
        
        public new class UxmlFactory : UxmlFactory<PathVisualElement> 
        {
            
        }

        public PathVisualElement()
        {
            style.flexDirection = FlexDirection.Row;
            
            VisualElement leftPanel = new VisualElement
            {
                style =
                {
                    flexBasis = Length.Percent(90)
                }
            };
            Add(leftPanel);

            _label = new Label
            {
                text = "PathLabel"
            };
            leftPanel.Add(_label);

            _textField = new TextField
            {
                label = "                  Asstets/"
            };
            leftPanel.Add(_textField);
            
            _confirmButton = new Button
            {
                text = "Confirm",
                style =
                {
                    width = Length.Percent(10),
                    alignSelf = Align.FlexEnd
                }
            };
            Add(_confirmButton);
        }

        public void Setup(string id, Action callback)
        {
            if (!BehaviourCanvasPaths.Ids.Contains(id)) throw new ArgumentException($"'{id}' isn't a valid path id. It should be added in {nameof(BehaviourCanvasPaths.Ids)}");
            if (!String.IsNullOrWhiteSpace(BehaviourCanvasPaths.GetSavedPath(id)))
            {
                if (EditorPrefs.HasKey(id)) _textField.value = BehaviourCanvasPaths.GetSavedPath(id).Substring(6).TrimStart('/');
            }
            _label.text = id;
            _confirmButton.clicked += callback;
        }
    }
}