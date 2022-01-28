using UnityEngine;

namespace GGJ2022 {

    public class TerrainManager : MonoBehaviour {
        [SerializeField] private Vector2Int _terrainSize = new Vector2Int(10, 10);
        private TileMap _tileMap;

        private void Awake() {
            if (_tileMap == null)
                _tileMap = ScriptableObject.CreateInstance<TileMap>();
        }

        void Start() {
            transform.localScale.Scale(new Vector3(_terrainSize.x, _terrainSize.y));
            _tileMap.CreateMap(_terrainSize, transform);
            
        }

        void Update() { }
    }
}