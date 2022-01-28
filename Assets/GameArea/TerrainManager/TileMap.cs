using System.Collections.Generic;

using UnityEngine;

namespace GGJ2022 {
    public sealed class TileMap : ScriptableObject {
        private Dictionary<Vector2Int, ITile> _tilemap;

        private void OnEnable() {
            _tilemap = new Dictionary<Vector2Int, ITile>();
        }

        public void CreateMap(Vector2Int terrainSize, Transform transform) {
            var tilePrefab = IoC.Resolve<ITile>();
            
            for (var x = 0; x < terrainSize.x; x++) {
                for (var y = 0; y < terrainSize.y; y++) {
                    var mapPosition = new Vector2Int(x, y);
                    var tileGO = Instantiate(tilePrefab.GetGameObject, transform);
                    var tile = tileGO.GetComponent<ITile>();
                    tileGO.transform.position = tile.GetWorldPosition( mapPosition );
                    _tilemap.Add(mapPosition, tile);
                }
            }
        }
    }
}