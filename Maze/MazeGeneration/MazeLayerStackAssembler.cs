using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGen
{
    public class MazeLayerStackAssembler : MonoBehaviour
    {
        public static readonly GenericEvent<MazeLayerStack> AssembleMazeStack = new();
        public static readonly GenericEvent<GameObject> MazeStackAssemblyCompleted = new();

        public MazeLayerStack Maze { get; private set; }

        [SerializeField] MazeLayerStackAssemblerSettings settings;
        List<GameObject> mazeLayerGameObjects;
        GameObject MazeParent;

        private void OnEnable()
        {
            AssembleMazeStack.AddListener(GenerateIndividualMazeLayers);
            MazeLayerAssembler.MazeLayerAssemblyCompleted.AddListener(AddMazeLayerToList);
        }

        private void OnDisable()
        {
            AssembleMazeStack.RemoveListener(GenerateIndividualMazeLayers);
            MazeLayerAssembler.MazeLayerAssemblyCompleted.RemoveListener(AddMazeLayerToList);
        }

        private void GenerateIndividualMazeLayers(MazeLayerStack maze)
        {
            transform.DestroyAllChildren(true);
            Maze = maze;

            mazeLayerGameObjects = new();
            MazeParent = new(Maze.name + " - " + GetMazeData.GetTileCount(maze) + " cells");
            MazeParent.transform.parent = transform;

            for(int layerIndex = 0; layerIndex < maze.layers.Count; layerIndex++)
            {
                var currentLayer = maze.layers[layerIndex];
                MazeLayerAssembler.AssembleLayer.Invoke(currentLayer);
            }
        }

        void AddMazeLayerToList(GameObject mazeLayerObject)
        {
            mazeLayerGameObjects.Add(mazeLayerObject);

            if(mazeLayerGameObjects.Count == Maze.layers.Count)
                AssembleMazeStackGameObjectFromLayers();
        }

        private void AssembleMazeStackGameObjectFromLayers()
        {
            for(int layerIndex = 0; layerIndex < mazeLayerGameObjects.Count; layerIndex++)
            {
                var currentLayer = mazeLayerGameObjects[layerIndex];
                currentLayer.transform.position = Vector3.zero + layerIndex * settings.gapBetweenFloors * Vector3.down;
                currentLayer.transform.parent = transform;
            }
        }
    }
}