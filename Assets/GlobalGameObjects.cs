using System;

using UnityEngine;

namespace GGJ2022
{
    [RequireComponent(typeof(GameSettings), typeof(IDungeonMap))]
    public class GlobalGameObjects : MonoBehaviour {
        [SerializeField] private GameSettings _settings = null;
        [SerializeField] private DungeonMap _dungeonMap = null;
        [SerializeField] private CreatureManager _creatureManager = null;

        private void Awake() {
            _dungeonMap.TerrainComplete += SpawnPlayer;
            
            _dungeonMap.Init(transform, _settings.StartingRoomSize);
            _creatureManager.Init(this);
        }

        private void Start() {
            //_dungeonMap.CreateDungeon();
            //_dungeonMap.CreateSpawnRoom();
            _dungeonMap.CreateQuickDungeon();
        }

        private void SpawnPlayer(Vector3 position) {
            var sampleCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sampleCube.transform.position = position;
        }
    }
}
