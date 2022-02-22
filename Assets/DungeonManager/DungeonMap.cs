using System;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

namespace GGJ2022 {
    public interface IDungeonMap {
        Vector2Int Position { get; }
        
        void Init(Transform parentTransform, IntSize startingRoomSize);
        
        void CreateDungeon();
        void CreateQuickDungeon();
        
        void CreateSpawnRoom();
        void CreateArea();
        void CreatePath();
        void CreateEndRoom();
    }

    [CreateAssetMenu(menuName = "GGJ2022/Dungeon Map")]
    public sealed class DungeonMap : ScriptableObject, IDungeonMap {
        [SerializeField] private int MAX_TILE_GROUPS = 100;

        [SerializeField] private int MAX_PATH_LENGTH = 10;
        
        [SerializeField] private int MAX_ROOM_SIZE = 50;
        [SerializeField] private int MIN_ROOM_SIZE = 10;
        
        [SerializeField] private IntRange ROOM_LIMIT = new IntRange(5, 10);
        [SerializeField] private IntRange ROOM_WIDTH = new IntRange(10, 50);
        [SerializeField] private IntRange ROOM_HEIGHT = new IntRange(10, 50);
        
        [SerializeField] private IntRange PATH_LENGTH = new IntRange(3, 10);
        
        private Dictionary<Vector2Int, ITile> _tiles;
        private Transform _parentTransform;
        private Area.DoorToThe _direction, _lastDirection;
        private IntSize _startingRoomSize;
        private int _numberOfRooms;
        
        private Vector2Int _currentPos;
        public Vector2Int Position => _currentPos;

        private void OnEnable() {
            _tiles = new Dictionary<Vector2Int, ITile>();
        }
        
        public void Init(Transform parentTransform, IntSize startingRoomSize) {
            _parentTransform = parentTransform;
            
            _startingRoomSize = startingRoomSize;
            _numberOfRooms = 5; // ROOM_LIMIT.Random();
            
            _currentPos = new Vector2Int();
        }

        public void CreateSpawnRoom() {
            var roomType = Area.AreaType.Spawn;
            var areaSize = GetAreaSize(roomType);
            
            _lastDirection = Area.DoorToThe.None;
            _direction = SetRandomDirection();
            
            var room = CreateArea(roomType, areaSize, _currentPos);
                    
            AddAreaToMap(room.TileMap);

            _currentPos += DistanceToEdge(_direction, areaSize);
        }

        public void CreateArea() {
            var roomType = Area.AreaType.Room;
            var areaSize = GetAreaSize(roomType);

            _currentPos += DistanceToEdge(_direction, areaSize);
            
            _lastDirection = AreaHelper.GetLastDirection(_direction);

            do {
                _direction = SetRandomDirection();
            } while (_direction == _lastDirection); 
            
            var room = CreateArea(roomType, areaSize, _currentPos);
                    
            AddAreaToMap(room.TileMap);

            _currentPos += DistanceToEdge(_direction, areaSize);
        }


        public void CreatePath() {
            var roomType = Area.AreaType.Path;
            var areaSize = GetAreaSize(roomType);

            _currentPos += DistanceToEdge(_direction, areaSize);
            
            var path = CreateArea(roomType, areaSize, _currentPos);
                    
            AddAreaToMap(path.TileMap);

            _currentPos += DistanceToEdge(_direction, areaSize);
        }

        public void CreateEndRoom() {
            var areaSize = GetAreaSize(Area.AreaType.Spawn);

            _currentPos += DistanceToEdge(_direction, areaSize);
            
            _lastDirection = AreaHelper.GetLastDirection(_direction);

            _direction = Area.DoorToThe.None;
            
            var room = CreateArea(Area.AreaType.Room, areaSize, _currentPos);
                    
            AddAreaToMap(room.TileMap);
        }

        public void CreateQuickDungeon() {
            CreateSpawnRoom();

            for (var i = 0; i < _numberOfRooms; i++) {
                CreatePath();
                
                CreateArea();
            }

            CreatePath();
            
            CreateEndRoom();
        }
        
        public void CreateDungeon() {
            var roomCounter = 0;
            var areaCounter = 0;

            _lastDirection = Area.DoorToThe.None;

            while (roomCounter <= _numberOfRooms) {
                
                var roomType = SetRoomType(areaCounter);

                var areasize = GetAreaSize(
                    roomCounter == _numberOfRooms ? Area.AreaType.Spawn : roomType
                );
                
                _currentPos += DistanceToEdge(_direction, areasize);
                
                _lastDirection = AreaHelper.GetLastDirection(_direction);

                if (roomType == Area.AreaType.Room) {
                    do {
                        _direction = SetRandomDirection();
                    } while (_direction == _lastDirection);
                }

                if (roomCounter == _numberOfRooms)
                    _direction = Area.DoorToThe.None;
                
                var room = CreateArea(roomType, areasize, _currentPos);
                    
                AddAreaToMap(room.TileMap);

                _currentPos += DistanceToEdge(_direction, areasize);
                
                if(roomType == Area.AreaType.Room)
                    roomCounter++;

                areaCounter++;
            }
        }

        private Area.AreaType SetRoomType(int roomCounter) {
            Area.AreaType roomType;
            
            if (roomCounter == 0)
                roomType = Area.AreaType.Spawn;
            else if (roomCounter >= MAX_TILE_GROUPS - 1)
                roomType = Area.AreaType.Spawn;
            else if (roomCounter % 2 == 0) 
                roomType = Area.AreaType.Room;
            else {
                roomType = Area.AreaType.Path;
            }

            return roomType;
        }

        private Area.DoorToThe SetRandomDirection() => 
            (Area.DoorToThe) Random.Range(1, 4);

        private IArea CreateArea(Area.AreaType type, IntSize size, Vector2Int currentPos) {
            var area = new Area(
                new AreaDetail {
                    Type = type,
                    Size = size,
                    Position = currentPos,
                    Parent = _parentTransform
                }
            );
            
            area.CreateFloor();

            if (type != Area.AreaType.Path) {
                area.CreateWalls();

                var doorWays = new List<Area.DoorToThe> {_direction};
                if (type != Area.AreaType.Spawn)
                    doorWays.Add(_lastDirection);
                area.CreateDoorWays(doorWays);
            }

            return area;
        }

        private IntSize GetAreaSize(Area.AreaType type) {
            return type switch {
                Area.AreaType.Spawn => _startingRoomSize,
                Area.AreaType.Path => AreaHelper.CalcPathSize(_direction, PATH_LENGTH.Random()),
                Area.AreaType.Room => new IntSize(ROOM_WIDTH.Random(), ROOM_HEIGHT.Random()),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private void AddAreaToMap(ITileMap tileMap) {
            foreach (var areaTile in tileMap.Tiles) {
                if(_tiles.ContainsKey(areaTile.Key))
                    continue;
                
                _tiles.Add(areaTile.Key, areaTile.Value);
            }
        }

        private bool IsOverlappingAnotherArea(Vector2Int position, IntSize size) {
            /*
            if (size == IntSize.zero)
                return true;

            var isOverlappingAnotherArea = false;

            var testArea = CreateRect(position, size); 
            
            foreach (var areaPair in _tiles) {
                var storedArea = CreateRect(areaPair.Key, areaPair.Value.Size);
                
                if (testArea.Overlaps(storedArea)) {
                    isOverlappingAnotherArea = true;
                    break;
                }
            }

            return isOverlappingAnotherArea;
            */
            
            for (var x = position.x - size.Center().x; x < position.x + size.Center().x; x++) {
                for (var y = position.y - size.Center().y; y < position.y + size.Center().y; y++) {
                    var storedTile = new Vector2Int(x, y);
                    if (_tiles.ContainsKey(storedTile) && _tiles[storedTile] != null)
                        return true;
                }
            }
            
            return false;
        }

        private RectInt CreateRect(Vector2Int position, Vector2Int size) {
            var mid = AreaHelper.FindMiddle(size);
            var result = new RectInt(Vector2Int.zero, size);
            result.position = new Vector2Int(
                position.x - mid.x,
                position.y - mid.y
            );
            return result;
        }

        public Vector2Int DistanceToEdge(Area.DoorToThe edge, IntSize size) {
            //this shouldn't work, right?
            var mid = AreaHelper.FindMiddle(size);
            var result = Vector2Int.zero;

            switch (edge) {
                case Area.DoorToThe.North: 
                    result.y = -mid.y;
                    break;
                case Area.DoorToThe.South:
                    result.y = mid.y;
                    break;
                case Area.DoorToThe.West:
                    result.x = -mid.x;
                    break;
                case Area.DoorToThe.East:
                    result.x = mid.x;
                    break;
            }

            return result;
        }

    }
}