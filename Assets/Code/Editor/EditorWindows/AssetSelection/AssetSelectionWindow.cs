using System;
using System.IO;
using Code.Editor.EditorWindows.BehaviourTreeEditor;
using Code.Editor.EditorWindows.PopUpWindow;
using Code.Runtime.BehaviourGraphSerialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor.EditorWindows.AssetSelection
{
    public class AssetSelectionWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;
        private Button _createButton;
        private Button _loadButton;
        private TextField _nameField;

        [MenuItem("Window/CanvasController/BehaviourCanvasEditor")]
        public static void ShowExample()
        {
            AssetSelectionWindow wnd = GetWindow<AssetSelectionWindow>();
            wnd.titleContent = new GUIContent("AssetSelectionWindow");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);

            _createButton = root.Q<Button>("CreateButton");
            _loadButton = root.Q<Button>("LoadButton");
            _nameField = root.Q<TextField>();

            _createButton.clicked += OnCreateButton;
            _loadButton.clicked += OnLoadButton;
        }

        private void OnCreateButton()
        {
            try
            {
                ValidateInputs();
            }
            catch (InvalidDataException e)
            {
                PopUp.Show(e.Message);
                return;
            }
            string name = _nameField.value;
            BehaviourTreeAsset asset = CreateAsset(name);
            ShowMainWindow(asset);
        }

        private void OnLoadButton()
        {
            int controlID = EditorGUIUtility.GetControlID (FocusType.Passive);
            EditorGUIUtility.ShowObjectPicker<BehaviourTreeAsset> (null, true, "", controlID);
        }
        
        private void OnGUI()
        {
            if(Event.current == null) return;
            if (Event.current.commandName == "ObjectSelectorClosed") {
                BehaviourTreeAsset selectedAsset = EditorGUIUtility.GetObjectPickerObject() as BehaviourTreeAsset;
                if(selectedAsset != null) ShowMainWindow(selectedAsset);
            }
        }

        private void ShowMainWindow(BehaviourTreeAsset asset)
        {
            BehaviourCanvasEditor.OpenWithAsset(asset);
            Close();
        }

        private BehaviourTreeAsset CreateAsset(string name)
        {
            throw new NotImplementedException();
        }
        
        private void ValidateInputs()
        {
            throw new NotImplementedException();
        }
        
        private void OnDestroy()
        {
            _createButton.clicked -= OnCreateButton;
            _loadButton.clicked -= OnLoadButton;
        }
    }
}
