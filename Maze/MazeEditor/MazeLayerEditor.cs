using UnityEditor;
using UnityEngine;
using static MazeAlgos.MazeAlgorithmChoices;
using MazeGen;

namespace MazeEditor
{
    [CustomEditor(typeof(MazeLayer))]
    public class MazeLayerEditor : Editor
    {
        public static MazeLayer currentMaze { get; set; }

        SerializedProperty generationAlgorithmProp;
        SerializedProperty seedProp;
        SerializedProperty sizeProp;
        SerializedProperty spawnProp;
        SerializedProperty goalProp;

        void OnEnable()
        {
            if(target == null) return;
            currentMaze = (MazeLayer)target;
            currentMaze.SetNeedUpdate();

            generationAlgorithmProp = serializedObject.FindProperty("GenerationAlgorithm");
            seedProp = serializedObject.FindProperty("Seed");
            sizeProp = serializedObject.FindProperty("Size");
            spawnProp = serializedObject.FindProperty("Spawn");
            goalProp = serializedObject.FindProperty("Goal");
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            ShowMazeLayerProperties();

            if(EditorGUI.EndChangeCheck())
                UpdateTargetObject();
        }

        void UpdateTargetObject()
        {
            ClampValues();
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
            currentMaze.SetNeedUpdate();
        }

        void ShowMazeLayerProperties()
        {
            ShowTotalTileCount();
            ShowMazeGenAlgoDropdown();
            ShowSeedProp();
            ShowSizeProp();
            ShowSpawnProp();
            ShowGoalProp();
        }


        void ShowTotalTileCount() => GUILayout.Label(sizeProp.vector2IntValue.x * sizeProp.vector2IntValue.y + " Cells", EditorStyles.boldLabel);
        void ShowMazeGenAlgoDropdown() => generationAlgorithmProp.enumValueIndex = (int)(AlgorithmChoice)EditorGUILayout.EnumPopup("Generation Algorithm", (AlgorithmChoice)generationAlgorithmProp.enumValueIndex);
        void ShowSeedProp() => seedProp.intValue = EditorGUILayout.IntField("Seed", seedProp.intValue);
        void ShowSizeProp() => sizeProp.vector2IntValue = EditorGUILayout.Vector2IntField("Size", sizeProp.vector2IntValue);
        void ShowSpawnProp() => spawnProp.vector2IntValue = EditorGUILayout.Vector2IntField("Spawn", spawnProp.vector2IntValue);
        void ShowGoalProp() => goalProp.vector2IntValue = EditorGUILayout.Vector2IntField("Goal", goalProp.vector2IntValue);


        void ClampValues()
        {
            if(sizeProp != null)
                sizeProp.vector2IntValue = VectorUtil.ClampVal(sizeProp.vector2IntValue, Vector2Int.one, currentMaze.MaxMazeSize);

            if(spawnProp != null)
                spawnProp.vector2IntValue = VectorUtil.ClampVal(spawnProp.vector2IntValue, Vector2Int.zero, sizeProp.vector2IntValue - Vector2Int.one);

            if(goalProp != null)
                goalProp.vector2IntValue = VectorUtil.ClampVal(goalProp.vector2IntValue, Vector2Int.zero, sizeProp.vector2IntValue - Vector2Int.one);
        }

        void OnValidate() => ClampValues();
    }
}