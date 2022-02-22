using UnityEditor;
using UnityEngine;

namespace GGJ2022
{
    [CustomEditor(typeof(DungeonMap), true)]
    public class AreaGeneratorEditor : Editor {
        private DungeonMap _dungeonMap;

        private void Awake() {
            _dungeonMap = (DungeonMap)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            
            if(GUILayout.Button("Create Spawn Area"))
                _dungeonMap.CreateSpawnRoom();

            if (GUILayout.Button("Create Room")) {
                _dungeonMap.CreateArea();
                // myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
                // EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
            }
        }
    }
}
