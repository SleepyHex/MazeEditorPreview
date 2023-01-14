using System.IO;
using MazeGen;
using UnityEditor;
using UnityEngine;

namespace MazeEditor
{
    public static class MazeLayerCreator
    {
        public static void CreateNewLayer(MazeLayerStack layerStack, int index)
        {
            MazeLayer newLayer = ScriptableObject.CreateInstance<MazeLayer>();
            string assetPath = AssetDatabase.GetAssetPath(layerStack);
            string assetFolder = Path.GetDirectoryName(assetPath).Replace("\\", "/") + "/";

            string newName = layerStack.name + " Layer " + (index + 1);
            int suffix = 1;
            string finalName = newName;
            while(File.Exists(assetFolder + finalName + ".asset"))
            {
                finalName = newName + " (" + suffix + ")";
                suffix++;
            }
            AssetDatabase.CreateAsset(newLayer, assetFolder + finalName + ".asset");
            layerStack.layers[index] = newLayer;
        }

        public static void AddLayerAfter(MazeLayerStack layerStack, int index)
        {
            int desiredAddPos = index + 1;
            int currentCt = layerStack.layers.Count;
            if(desiredAddPos < currentCt)
            {
                if(layerStack.layers[desiredAddPos] != null)
                    layerStack.layers.Insert(index + 1, null);
            }
            else
                layerStack.layers.Insert(index + 1, null);
        }

        public static void AddLayerBefore(MazeLayerStack layerStack, int index)
        {
            int desiredAddPos = index;
            int currentCt = layerStack.layers.Count;
            if(desiredAddPos < currentCt)
            {
                if(layerStack.layers[desiredAddPos] != null)
                    layerStack.layers.Insert(index, null);
            }
            else
                layerStack.layers.Insert(index, null);
        }

        public static void RemoveLayer(MazeLayerStack layerStack, int index) => layerStack.layers.RemoveAt(index);
    }
}