using System.Linq;
using MazeGen;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MazeEditor
{
    public static class MazeLayerStackEditorHelper
    {
        public static void TryShowDefaultUI(MazeLayerStack layerStack, ReorderableList list, ref bool showOrganizingMenu)
        {
            ShowTileCount(layerStack);
            if(layerStack.layers.Any())
                showOrganizingMenu = EditorGUILayout.Toggle("Show Layer Organizer", showOrganizingMenu, GUILayout.Width(10));

            if(showOrganizingMenu)
                list.DoLayoutList();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();

            for(int index = 0; index < layerStack.layers.Count; index++)
                TryShowMazeLayerUI(layerStack, index);

            EditorGUILayout.EndVertical();
        }

        public static void TryShowNoLayersInMazeUI(MazeLayerStack layerStack)
        {
            if(layerStack.layers.Count == 0)
                if(GUILayout.Button("Add Layer"))
                    layerStack.layers.Add(null);
        }

        static void TryShowMazeLayerUI(MazeLayerStack layerStack, int index)
        {
            EditorGUILayout.Space();
            var currentLayer = layerStack.layers[index];

            ShowSlotForNewMazeObject(layerStack, index, currentLayer);

            if(currentLayer != null)
                ShowSingleLayerUIBlock(currentLayer, layerStack, index);
            else
                ShowMissingMazeLayerUI(layerStack, index);
        }

        static void ShowSlotForNewMazeObject(MazeLayerStack layerStack, int index, MazeLayer currentLayer) => layerStack.layers[index] = (MazeLayer)EditorGUILayout.ObjectField("Layer " + (index + 1), currentLayer, typeof(MazeLayer), false);

        static void ShowMissingMazeLayerUI(MazeLayerStack layerStack, int index)
        {
            EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("Remove This Layer"))
            {
                layerStack.layers.RemoveAt(index);
                return;
            }

            if(GUILayout.Button("Create New Layer"))
            {
                MazeLayerCreator.CreateNewLayer(layerStack, index);
                return;
            }

            EditorGUILayout.EndHorizontal();
        }

        static void ShowSingleLayerUIBlock(MazeLayer layer, MazeLayerStack layerStack, int index)
        {
            if(index == 0)
                ShowButtonAddLayerBefore(layerStack, index);

            EditorGUILayout.BeginVertical("box");

            var editor = Editor.CreateEditor(layer);
            editor.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();

            ShowButtonRemoveThisLayer(layerStack, index);
            ShowButtonAddLayerAfter(layerStack, index);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            GUILayout.Space(24);
        }

        static void ShowButtonAddLayerBefore(MazeLayerStack layerStack, int index)
        {
            if(GUILayout.Button("Add Layer Before This"))
                MazeLayerCreator.AddLayerBefore(layerStack, index);
        }

        static void ShowButtonAddLayerAfter(MazeLayerStack layerStack, int index)
        {
            if(GUILayout.Button("Add Layer After This"))
                MazeLayerCreator.AddLayerAfter(layerStack, index);
        }

        static void ShowButtonRemoveThisLayer(MazeLayerStack layerStack, int index)
        {
            if(GUILayout.Button("Remove This Layer"))
                MazeLayerCreator.RemoveLayer(layerStack, index);
        }

        static void ShowTileCount(MazeLayerStack layerStack)
        {
            string totalTileCt = layerStack.layers.Count() + " floors, totaling " + GetMazeData.GetTileCount(layerStack) + " tiles";
            EditorGUILayout.LabelField(totalTileCt);
        }
    }
}