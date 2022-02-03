using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Random = UnityEngine.Random;

namespace GGJ2022 {
    public interface IDungeonMap {
        void Init(Transform parentTransform);
        void CreateDungeon(Vector2Int startingRoomSize);
    }

    [CreateAssetMenu(menuName = "GGJ2022/Dungeon Map")]
    public sealed class DungeonMap : ScriptableObject, IDungeonMap {
        [SerializeField] private int MAX_TILE_GROUPS = 100;

        [SerializeField] private int MAX_PATH_LENGTH = 10;
        
        [SerializeField] private int MAX_ROOM_SIZE = 50;
        [SerializeField] private int MIN_ROOM_SIZE = 10;
        
        private Dictionary<Vector2Int, IArea> _areas = default;
        private Transform _parentTransform;
        private Area.DoorToThe _direction, _lastDirection;
        private Vector2Int _startingRoomSize;

        private void OnEnable() {
            _areas = new Dictionary<Vector2Int, IArea>();
        }
        
        public void Init(Transform parentTransform) {
            _parentTransform = parentTransform;
        }
        
        public void CreateDungeon(Vector2Int startingRoomSize) {
            _startingRoomSize = startingRoomSize;
            var roomCounter = 0;
            
            //start location
            var currentPos = new Vector2Int();

            _lastDirection = Area.DoorToThe.None;
            IArea room, path = null;
            var roomType = Area.AreaType.Spawn;

            var areasize = _startingRoomSize;
            
            while (roomCounter < MAX_TILE_GROUPS) {

                _direction = (Area.DoorToThe) Random.Range(1, 4);
                
                var overlappingCounter = 0;
                while (IsOverlappingAnotherArea(currentPos, areasize)) {
                    if (overlappingCounter > 4) break;
                    
                    if (roomCounter == 0)
                        roomType = Area.AreaType.Spawn;
                    else if (roomCounter >= MAX_TILE_GROUPS - 1)
                        roomType = Area.AreaType.Spawn;
                    else {
                        roomType = Area.AreaType.Room;
                    }

                    //areasize = GetAreaSize(roomType);
                    
                    overlappingCounter++;
                }
                if (overlappingCounter > 4) break;
                
                room = CreateArea(roomType, areasize);

                if (room == null) break;
                
                room.SetPosition(currentPos);
                _areas.Add(currentPos,room);

                _lastDirection = _direction;

                roomType = Area.AreaType.Path;
                currentPos += DistanceToEdge(_direction, areasize);
                areasize = GetAreaSize(roomType);
                
                path = CreateArea(roomType, areasize);
                path.SetPosition(currentPos);
                _areas.Add(currentPos, path);

                _lastDirection = _direction;
                currentPos += DistanceToEdge(_direction, areasize);
                
                //increment counter
                roomCounter++;
            }
        }

        private IArea CreateArea(Area.AreaType type, Vector2Int size) {
            var area = new Area(type, size, _parentTransform);
            area.CreateFloor();
            
            if (type != Area.AreaType.Path)
                area.CreateWalls();

            var doorWays = new List<Area.DoorToThe> {_direction};
            if (type != Area.AreaType.Spawn)
                doorWays.Add(_lastDirection);
            area.CreateDoorWays(doorWays);

            return area;
        }

        private Vector2Int GetAreaSize(Area.AreaType type) {
            var roomSize = AreaHelper.MakeMiddleOdd(
                new Vector2Int(
                    Random.Range(MIN_ROOM_SIZE, MAX_ROOM_SIZE),
                    Random.Range(MIN_ROOM_SIZE, MAX_ROOM_SIZE))
            );

            var pathSize = AreaHelper.CalcPathSize(
                _direction,
                Random.Range(0, MAX_PATH_LENGTH)
            );
            
            return type switch {
                Area.AreaType.Spawn => _startingRoomSize,
                Area.AreaType.Path => pathSize,
                Area.AreaType.Room => roomSize,
                _ => new Vector2Int()
            };
        }

        private bool IsOverlappingAnotherArea(Vector2Int position, Vector2Int size) {
            if (size == Vector2Int.zero)
                return true;

            var isOverlappingAnotherArea = false;
            
            foreach (var areaPair in _areas) {
                var storedArea = new Rect(
                    areaPair.Key,
                    areaPair.Value.Size
                    );
                var testArea = new Rect(position, size);
                if (testArea.Overlaps(storedArea)) {
                    isOverlappingAnotherArea = true;
                    break;
                }
            }

            return isOverlappingAnotherArea;
        }

        public Vector2Int DistanceToEdge(Area.DoorToThe edge, Vector2Int size) {
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