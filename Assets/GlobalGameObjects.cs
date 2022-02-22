using UnityEngine;

namespace GGJ2022
{
    [RequireComponent(typeof(GameSettings), typeof(IDungeonMap))]
    public class GlobalGameObjects : MonoBehaviour {
        [SerializeField] private GameSettings _settings = null;
        [SerializeField] private DungeonMap _dungeonMap = null;

        private void Awake() {
            _dungeonMap.Init(transform, _settings.StartingRoomSize);
        }

        private void Start() {
            //_dungeonMap.CreateDungeon();
            //_dungeonMap.CreateSpawnRoom();
            _dungeonMap.CreateQuickDungeon();
        }
    }
}
