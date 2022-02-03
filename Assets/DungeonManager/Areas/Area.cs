using System.Collections.Generic;

using UnityEngine;

namespace GGJ2022 {
    public interface IArea {
        Vector2Int Size { get; }
        void SetPosition(Vector2Int pos);
    }

    public sealed class Area : IArea {
        public const int PATH_WIDTH = 3;
        public enum DoorToThe {
            None = 0,
            North,
            South,
            East,
            West
        }

        public enum AreaType {
            Path = 0,
            Room,
            Spawn
        }

        private RectInt _rect;
        public RectInt RectSize => _rect;

        private readonly AreaType _type;
        public AreaType Type => _type;
        
        private readonly Vector2Int _size;
        public Vector2Int Size => _rect.size;

        private string _name;
        private GameObject _container;
        public GameObject Container => _container;

        private ITileMap _tileMap;
        public ITileMap TileMap => _tileMap;

        private IWallMap _wallMap;
        public IWallMap WallMap => _wallMap;
        
        public Area(AreaType type, Vector2Int size, Transform parent) {
            _type = type;
            _size = size;
            _rect = new RectInt(new Vector2Int(), size);

            _name = $"{_type.ToString()} ({_size.x} : {_size.y})";
            _container = new GameObject(_name);
            _container.transform.SetParent(parent);
        }
        
        public Area(AreaType type, Vector2Int size, Transform parent, ITileMap tileMap, IWallMap wallMap) {
            _type = type;
            _size = size;

            _name = $"{_type.ToString()} ({_size.x} : {_size.y})";
            _container = new GameObject(_name);
            _container.transform.SetParent(parent);

            _tileMap = tileMap;
            _wallMap = wallMap;
        }
        
        public void SetPosition(Vector2Int pos) {
            _container.transform.position = new Vector3(pos.x, 0, pos.y);

            var mid = _rect.center;
            
            _rect.x += Mathf.FloorToInt(mid.x);
            _rect.y += Mathf.FloorToInt(mid.y);
            
            _tileMap?.SetPosition(pos);
            _wallMap?.SetPosition(pos);
        }

        public void CreateFloor() {
            _tileMap = IoC.Resolve<ITileMap>();
            _tileMap.Init(_container.transform, _size);
            _tileMap.CreateTiles();
        }

        public void CreateWalls() {
            if (_tileMap == null) return;
            
            _wallMap = IoC.Resolve<IWallMap>();
            _wallMap.Init(_container.transform);
            _wallMap.CreateWalls(_tileMap.Tiles);
        }

        public void CreateDoorWays(List<DoorToThe> doorWays) {
            if (_wallMap == null) return;

            foreach (var door in doorWays)
                _wallMap.CreateDoorWay(door);
        }
    }

}