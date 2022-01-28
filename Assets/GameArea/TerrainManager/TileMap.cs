using System;
using System.Collections.Generic;

using UnityEngine;

namespace GGJ2022 {
    public sealed class TileMap : ScriptableObject {
        private Dictionary<Vector2Int, ITile> _tilemap;
        private Transform _parentTransform;

        private void OnEnable() {
            _tilemap = new Dictionary<Vector2Int, ITile>();
        }

        public void CreateMap(Vector2Int terrainSize, Transform transform) {
            _parentTransform = transform;

            //start location
            var current = new Vector2Int();
            CreatePath(current);
            
            //choose direction and move to end of path/room
            current.y += 1;
            //same as pathSize / 2 = 1.5 when rounded by int conversion = 1

            //get room size for next location
            var room = terrainSize;
            //move to centre
            current.y += room.y / 2;
            CreateRoom(current, terrainSize);
        }

        private void CreatePath(Vector2Int pos) =>
            CreateRoom(pos, new Vector2Int(3,3));
        
        private void CreateRoom(Vector2Int position, Vector2Int size) {
            var halfSize = GetRoomCenter(size);
            
            for (var x = 0; x < size.x; x++) {
                for (var y = 0; y < size.y; y++) {
                    GenerateTileAt(position.x + ( x - halfSize.x), position.y + (y - halfSize.y));
                }
            }
        }

        private Vector2Int GetRoomCenter(Vector2Int size) {
            var result = size / 2;
            
            if (size.x % 2 != 0) 
                result.x++;

            if (size.y % 2 != 0)
                result.y++;

            return result;
        }

        private void GenerateTileAt(int x, int y) {
            var tilePrefab = IoC.Resolve<ITile>();

            var mapPosition = new Vector2Int(x, y);
            
            //need to ensure rooms don't overlap...
            if (_tilemap.ContainsKey(mapPosition))
                return;
            
            var tileGO = Instantiate(tilePrefab.GetGameObject, _parentTransform);
            var tile = tileGO.GetComponent<ITile>();
            
            tileGO.transform.position = tile.GetWorldPosition(mapPosition);
            _tilemap.Add(mapPosition, tile);
        }
    }
}