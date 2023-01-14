using System.Collections.Generic;
using MazeGen;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MazeEditor
{
    [CustomEditor(typeof(MazeLayerStack))]
    public class MazeLayerStackEditor : Editor
    {
        public static MazeLayerStack CurrentMaze { get; private set; }

        static ReorderableList list;
        static SerializedProperty layersProperty;
        static bool showOrganizingMenu = false;

        void OnEnable()
        {
            layersProperty = serializedObject.FindProperty("layers");
            list = new ReorderableList(serializedObject, layersProperty, true, true, true, true);

            list.drawElementCallback += DrawElementCallback;
            list.drawHeaderCallback += DrawHeaderCallback;
        }

        private void OnDisable()
        {
            list.drawElementCallback -= DrawElementCallback;
            list.drawHeaderCallback -= DrawHeaderCallback;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            CurrentMaze = target as MazeLayerStack;
            if(CurrentMaze == null) return;

            if(CurrentMaze.layers == null)
                CurrentMaze.layers = new List<MazeLayer>();

            MazeLayerStackEditorHelper.TryShowNoLayersInMazeUI(CurrentMaze);
            MazeLayerStackEditorHelper.TryShowDefaultUI(CurrentMaze, list, ref showOrganizingMenu);

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }

        void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
        }

        void DrawHeaderCallback(Rect rect)
        {
            rect.width -= EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(rect, new GUIContent("Maze Layers"));
        }
    }
}