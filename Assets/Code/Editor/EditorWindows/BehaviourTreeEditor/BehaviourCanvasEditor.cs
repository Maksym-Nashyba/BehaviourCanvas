using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Editor.EditorWindows.BehaviourTreeEditor
{
    public class BehaviourCanvasEditor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset = default;

        [MenuItem("Window/BehaviourCanvas/BehaviourCanvasEditor")]
        public static void OpenWindow()
        {
            BehaviourCanvasEditor wnd = GetWindow<BehaviourCanvasEditor>();
            wnd.titleContent = new GUIContent("BehaviourCanvasEditor");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);
            AddStylesheets();
            
            string behaviourTreeAssetPath = BehaviourCanvasPaths.BehaviourTreeAssets +"BehaviourTreeAsset.asset";
            string modelsDatabaseXMLPath = BehaviourCanvasPaths.BehaviourTreeAssets + "ModelsDatabase.xml";
            BehaviourTreeAsset treeAsset = AssetDatabase.LoadAssetAtPath<BehaviourTreeAsset>(behaviourTreeAssetPath);
            TextAsset modelDatabaseXML = AssetDatabase.LoadAssetAtPath<TextAsset>(modelsDatabaseXMLPath);
        
            //BehaviourCanvasSerializer behaviourCanvasSerializer = new BehaviourCanvasSerializer(treeAsset);
            NodeBuilderSerializer nodeBuilderSerializer = new NodeBuilderSerializer(modelDatabaseXML);
        
            //BehaviourCanvas behaviourCanvas = CreateBehaviourCanvas(behaviourCanvasSerializer);
        
            BehaviourCanvasView canvasView = root.Q<BehaviourCanvasView>();
            NodeBuilder nodeBuilder = root.Q<NodeBuilder>();
            //canvasView.Initialize(behaviourCanvas);
            nodeBuilder.Initialize(canvasView, nodeBuilderSerializer);
        }
        
        private void AddStylesheets()
        {
            StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/Editor/EditorWindows/" +
                                                                              "BehaviourTreeEditor/BehaviourCanvasEditor.uss");
            rootVisualElement.styleSheets.Add(stylesheet);
        }
        
        private BehaviourCanvas CreateBehaviourCanvas(BehaviourCanvasSerializer serializer)
        {
            List<StateModel> states = serializer.DeserializeStateModels();
            StateModel rootState = serializer.FindRootState(states);
            List<TriggerModel> triggers = serializer.DeserializeTriggerModels();
            return new BehaviourCanvas(rootState, states, triggers);
        }
    }
}
