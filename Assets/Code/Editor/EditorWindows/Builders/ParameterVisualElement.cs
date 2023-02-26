using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Code.Editor.EditorWindows.Builders
{
    public class ParameterVisualElement : VisualElement
    {
        private readonly VisualElement _simpleTypesArea;
        private readonly VisualElement _customTypesArea;
        private readonly TextField _nameField;
        private readonly DropdownField _simpleTypesDropdown;
        private readonly TextField _customTypeNameField;
        private DropdownField _customTypeDropdown;
        public bool IsFilled => HasValidInput();

        public new class UXmlFactory : UxmlFactory<ParameterVisualElement>
        {
        }

        public ParameterVisualElement()
        {
            style.flexDirection = FlexDirection.Row;

            _simpleTypesArea = new VisualElement();
            _simpleTypesArea.style.flexShrink = 0f;
            _customTypesArea = new VisualElement();
            Add(_simpleTypesArea);
            Add(_customTypesArea);
            HideCustomTypesArea();

            _nameField = new TextField();
            _nameField.label = "Parameter Name";
            _simpleTypesArea.Add(_nameField);
            _simpleTypesDropdown = new DropdownField();
            _simpleTypesDropdown.label = "Type";
            _simpleTypesDropdown.choices = new List<string>
            {
                "NONE",
                "object",
                "int",
                "float",
                "Vector2",
                "Vector3",
                "Transform",
                "Rigidbody",
                "GameObject",
                "Collider",
                "CUSTOM"
            };
            _simpleTypesDropdown.value = "NONE";
            _simpleTypesDropdown.RegisterValueChangedCallback(OnSimpleTypeChanged);
            _simpleTypesArea.Add(_simpleTypesDropdown);

            _customTypeNameField = new TextField();
            _customTypeNameField.label = "Custom Type Name";
            _customTypeNameField.style.flexGrow = 1f;
            _customTypeNameField.style.alignItems = Align.FlexStart;
            _customTypesArea.Add(_customTypeNameField);
            _customTypeNameField.RegisterValueChangedCallback(OnInputCustomTypeName);
        }

        private void OnInputCustomTypeName(ChangeEvent<string> args)
        {
            if (_customTypeDropdown is null)
            {
                _customTypeDropdown = new DropdownField();
                _customTypeDropdown.label = "Exact Type";
                _customTypesArea.Add(_customTypeDropdown);
            }

            _customTypeDropdown.value = null;
            _customTypeDropdown.choices = Reflection.GetAllTypesShortName(args.newValue)
                .Select(type => type.FullName).ToList();
        }

        private void OnSimpleTypeChanged(ChangeEvent<string> args)
        {
            switch (args.newValue)
            {
                case "NONE":
                    HideCustomTypesArea();
                    break;
                case "CUSTOM":
                    ShowCustomTypesArea();
                    break;
                default:
                    HideCustomTypesArea();
                    break;
            }
        }

        private bool HasValidInput()
        {
            if (_simpleTypesDropdown.value == "NONE") return false;

            return true;
        }

        private void ShowCustomTypesArea()
        {
            _simpleTypesArea.style.borderRightWidth = 2f;
            _customTypesArea.style.display = DisplayStyle.Flex;
        }

        private void HideCustomTypesArea()
        {
            _simpleTypesArea.style.borderRightWidth = 0;
            _customTypesArea.style.display = DisplayStyle.None;
        }
    }
}