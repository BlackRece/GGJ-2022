using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

namespace GGJ2022 {
    public interface IWallMap {
        void Init(Transform parent);
        void CreateWalls(List<ITile> tiles);
        void CreateDoorWay(Area.DoorToThe doorway);
        void SetPosition(Vector2Int pos);
    }
    
    public sealed class WallMap : ScriptableObject, IWallMap {
        private Transform _parent;
        private string _name;
        
        private Dictionary<Vector2Int, IWall> _walls;
        private Vector2Int _size;
        
        private GameObject _container;
        public GameObject WallContainer => _container;

        public void Init(Transform parent) {
            _parent = parent;

            _walls = new Dictionary<Vector2Int, IWall>();
            _name = $"Walls ({_size.x} : {_size.y})";

            _container = new GameObject(_name);
            _container.transform.SetParent(_parent);
        }
        
        private void OnEnable() {
            _walls = new Dictionary<Vector2Int, IWall>();
            //_obstacles = new Dictionary<Vector3, IObstacle>();
        }

        public void CreateWalls(List<ITile> tiles) {
            foreach (var tile in tiles) {
                var tileEdges = tile.GetEdges;
                if (tileEdges.Count < 1) continue;
                
                var mapPos = tile.GetMapPosition();
                if (_walls.ContainsKey(mapPos)) continue;
                
                var wall = GenerateWall(_container.transform);
                
                var worldPos = tile.GetTopPosition();
                worldPos.y += GetGameObjectHeight(wall.GetGameObject) / 2;
                wall.GetGameObject.transform.position = worldPos;
                wall.Directions = tileEdges;
                
                _walls.Add(mapPos, wall);
            }
        }

        private void CalcAreaSize(Dictionary<Vector2, List<Area.DoorToThe>> tileEdges) {
            var norths = 0;
            var wests = 0;
            
            Vector2 
                southWest = default, 
                southEast = default,
                northWest = default,
                northEast = default;
            
            foreach (var edge in tileEdges) {
                if (edge.Value.Contains(Area.DoorToThe.North))
                    norths++;

                if (edge.Value.Contains(Area.DoorToThe.West))
                    wests++;

                if (edge.Value.Contains(Area.DoorToThe.West) &&
                    edge.Value.Contains(Area.DoorToThe.South)) {
                    southWest = edge.Key;
                }
                if (edge.Value.Contains(Area.DoorToThe.East) &&
                    edge.Value.Contains(Area.DoorToThe.South)) {
                    southEast = edge.Key;
                }
                if (edge.Value.Contains(Area.DoorToThe.West) &&
                    edge.Value.Contains(Area.DoorToThe.North)) {
                    northWest = edge.Key;
                }
                if (edge.Value.Contains(Area.DoorToThe.East) &&
                    edge.Value.Contains(Area.DoorToThe.North)) {
                    northEast = edge.Key;
                }
            }

            _size = new Vector2Int(norths, wests);
            var checkWidth = Vector2.Distance(northWest, northEast);
            var checkLength = Vector2.Distance(northWest, southWest);
            Assert.AreNotEqual(_size.x, Mathf.FloorToInt(checkWidth));
            Assert.AreNotEqual(_size.y, Mathf.FloorToInt(checkLength));
        }

        public void CreateDoorWay(Area.DoorToThe doorway) {
            if (doorway == Area.DoorToThe.None) return;
            
            var doorPositions = GetDoorsInWallFacing(doorway);
            
            foreach (var doorPos in doorPositions) {
                if (_walls.ContainsKey(doorPos)) 
                    _walls[doorPos].Hide();
            }
        }

        public void SetPosition(Vector2Int pos) {
            var offset = new Vector3(pos.x, 0, pos.y);

            _container.transform.position = offset;
            
            foreach (var wall in _walls.Values) {
                wall.GetGameObject.transform.position += offset;
            }
        }

        private IWall GenerateWall(Transform parentTransform) {
            var wallGO = Instantiate(
                IoC.Resolve<IWall>().GetGameObject,
                _container.transform
            );
            
            return wallGO.GetComponent<IWall>();
        }
        
        public Vector2Int DistanceToEdge(Area.DoorToThe edge) {
            var mid = AreaHelper.FindMiddle(_size);
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

        private List<Vector2Int> GetWallsFacing(Area.DoorToThe direction) {
            var walls = new List<Vector2Int>();

            foreach (var wallPair in _walls) {
                var wall = wallPair.Value;
                var edges = wall.Directions;
                if (!edges.Contains(direction)) 
                    continue;
                
                walls.Add(wallPair.Key);
            }

            return walls;
        }

        private List<Vector2Int> GetDoorsInWallFacing(Area.DoorToThe way) {
            var walls = GetWallsFacing(way);
            var wallsInDoorWay = new List<Vector2Int>();
            
            if (walls.Count < 1) return wallsInDoorWay;

            if (walls.Count <= Area.PATH_WIDTH) {
                foreach (var wallPositon in walls) {
                    wallsInDoorWay.Add(wallPositon);
                    return wallsInDoorWay;
                }
            }
            
            var middleDoor = Mathf.FloorToInt((float) walls.Count / 2);
            var halfDoorWidth = Mathf.FloorToInt((float) Area.PATH_WIDTH / 2);
            
            for (var i = middleDoor - halfDoorWidth; i <= middleDoor + halfDoorWidth; i++) 
                wallsInDoorWay.Add(walls[i]);
            
            return wallsInDoorWay;
        }

        public List<Vector2Int> GetDoors(Area.DoorToThe way) {
            var mid = AreaHelper.FindMiddle(_size);
            var ways = new List<Vector2Int>();

            var edge = DistanceToEdge(way);
            if (edge.x == 0) {
                for (var i = -1; i <= 1; i++) 
                    ways.Add(new Vector2Int(edge.x, edge.y + i));
            }
            
            if (edge.y == 0) {
                for (var i = -1; i <= 1; i++) 
                    ways.Add( new Vector2Int(edge.x + i, 0));
            }

            return ways;
        }

        private float GetGameObjectHeight(GameObject gameObject) => 
                gameObject.GetComponent<Renderer>().bounds.size.y;
        
    }
}