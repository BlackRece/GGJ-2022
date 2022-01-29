using System.Collections.Generic;

using UnityEngine;

namespace GGJ2022 {
    public class Limit {
        private readonly int _min;
        private readonly int _max;

        public Limit(int min, int max) {
            _min = min;
            _max = max;
        }
        public int Fix(int value) {
            if (value > _max) return _max;
            if (value < _min) return _min;
            return value;
        }
    }
    
    public sealed class TileMap : ScriptableObject {
        [SerializeField] private int PATH_WIDTH = 3;
        [SerializeField] private int MAX_PATH_LENGTH = 10;
        [SerializeField] private int MAX_TILE_GROUPS = 100;
        
        private Dictionary<Vector2Int, ITile> _tilemap;
        private Transform _parentTransform;
        
        private enum Wall {
            None = 0,
            North,
            South,
            East,
            West
        }

        private void OnEnable() {
            _tilemap = new Dictionary<Vector2Int, ITile>();
        }

        public void CreateMap(Vector2Int startingRoomSize, Transform transform) {
            _parentTransform = transform;

            int roomCounter = 0;
            
            //start location
            var currentPos = new Vector2Int();
            var nextRoom = new RoomDimension(startingRoomSize);

            Vector2Int path;
            int pathLength;
            
            while (roomCounter < MAX_TILE_GROUPS) {
                //room
                CreateArea(currentPos, nextRoom.Size);
                
                //get path length for next location
                pathLength = Random.Range(0, MAX_PATH_LENGTH) + 1;
                
                //choose direction
                var direction = (Wall)Random.Range(1, 4);
                
                //move to room's wall
                currentPos += CalcNextAreaCenter(direction, pathLength, nextRoom);
                
                //set path area
                path = CalcPathSize(direction, pathLength);
                
                //generate path
                CreateArea(currentPos, path);
                
                //set room area
                nextRoom = RoomDimension.Randomise();
                
                //move to next room's center
                currentPos += CalcNextAreaCenter(direction, pathLength, nextRoom);
                
                //increment counter
                roomCounter++;
            }
        }

        private Vector2Int CalcNextAreaCenter(Wall direction, int pathLength, RoomDimension room) {
            var distance = new Vector2Int();
            
            switch (direction) {
                case Wall.North:
                    distance.y -= (pathLength / 2 + room.Center.y);
                    break;
                case Wall.South:
                    distance.y += (pathLength / 2 + room.Center.y);
                    break;
                case Wall.East:
                    distance.x += (pathLength / 2 + room.Center.x);
                    break;
                case Wall.West:
                    distance.x -= (pathLength / 2 + room.Center.x);
                    break;
            }

            return distance;
        }

        private Vector2Int CalcPathSize(Wall direction, int pathSize) {
            var path = new Vector2Int(PATH_WIDTH, PATH_WIDTH);
            
            switch (direction) {
                case Wall.North:
                case Wall.South:
                    path.y += pathSize;
                    break;
                case Wall.East:
                case Wall.West:
                    path.x += pathSize;
                    break;
            }

            return path;
        }

        //private void CreatePath(Vector2Int pos, Vector2Int size) =>
        //    CreateArea(pos, size);
        
        private void CreateArea(Vector2Int position, Vector2Int size) {
            var halfSize = GetRoomCenter(size);
            var container = new GameObject($"{position.x} : {position.y}");
            container.transform.SetParent(_parentTransform);
            
            for (var x = 0; x < size.x; x++) {
                for (var y = 0; y < size.y; y++) {
                    GenerateTileAt(
                        position.x + ( x - halfSize.x),
                        position.y + (y - halfSize.y),
                        container.transform);
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

        private void GenerateTileAt(int x, int y, Transform parentTransform) {
            var tilePrefab = IoC.Resolve<ITile>();

            var mapPosition = new Vector2Int(x, y);
            
            //need to ensure rooms don't overlap...
            if (_tilemap.ContainsKey(mapPosition))
                return;
            
            var tileGO = Instantiate(tilePrefab.GetGameObject, parentTransform);
            var tile = tileGO.GetComponent<ITile>();
            
            tileGO.transform.position = tile.GetWorldPosition(mapPosition);
            _tilemap.Add(mapPosition, tile);
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

    public sealed class RoomDimension {
        private const int MAX_ROOM_SIZE = 20;
        private const int MIN_ROOM_SIZE = 5;
        
        private Vector2Int _center;
        public Vector2Int Center {
            get {
                if (_center == default) 
                    _center = GetCenter();

                return _center;
            }
        }
            
        private Vector2Int _size;
        public Vector2Int Size {
            get => _size;
            private set {
                var limiter = new Limit(MIN_ROOM_SIZE, MAX_ROOM_SIZE);
                _size = new Vector2Int(
                    limiter.Fix(value.x),
                    limiter.Fix(value.y)
                );
            }
        }

        public RoomDimension() => 
            _size = default;
        public RoomDimension(int width, int length) => 
            _size = new Vector2Int(width, length);
        public RoomDimension(Vector2Int dimension) => 
            _size = dimension;

        public static RoomDimension Randomise() =>
            new RoomDimension(
                Random.Range(MIN_ROOM_SIZE, MAX_ROOM_SIZE),
                Random.Range(MIN_ROOM_SIZE, MAX_ROOM_SIZE));

        private Vector2Int GetCenter() {
            var result = _size / 2;
            
            if (_size.x % 2 != 0) 
                result.x++;

            if (_size.y % 2 != 0)
                result.y++;

            return result;
        }
    }
}