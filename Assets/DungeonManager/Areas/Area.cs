using System.Collections.Generic;

using UnityEngine;

namespace GGJ2022 {
    public interface IArea {
        IntSize Size { get; }
        Vector2Int Center { get; }
        void SetPosition(Vector2Int pos);
        ITileMap TileMap { get; } 
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
        public Vector2Int Center => new Vector2Int (
            Mathf.FloorToInt(_rect.center.x), 
            Mathf.FloorToInt(_rect.center.y)
        );

        private readonly AreaType _type;
        public AreaType Type => _type;
        
        private readonly IntSize _size;
        public IntSize Size => _size;

        private readonly Vector2Int _position;  
        
        private string _name;
        private GameObject _container;
        public GameObject Container => _container;

        private ITileMap _tileMap;
        public ITileMap TileMap => _tileMap;

        private IWallMap _wallMap;
        public IWallMap WallMap => _wallMap;

        public Area(AreaDetail detail) {
            _type = detail.Type;
            _size = detail.Size;
            _position = detail.Position;

            CreateContainer(detail.Parent);
        }

        public Area(AreaType type, Vector2Int size, Transform parent, ITileMap tileMap, IWallMap wallMap) {
            _type = type;
            _size = new IntSize(size.x, size.y);

            CreateContainer(parent);
            
            _tileMap = tileMap;
            _wallMap = wallMap;
        }
        
        private void CreateContainer(Transform parent) {
            _name = $"{_type.ToString()} ({_size.Width} : {_size.Height})";
            _container = new GameObject(_name);
            _container.transform.SetParent(parent);
        }
        
        public void CreateFloor() {
            _tileMap = IoC.Resolve<ITileMap>();
            _tileMap.Init(_container.transform, _size);
            _tileMap.CreateTiles(_position);
        }

        public void CreateWalls() {
            if (_tileMap == null) return;
            var tileList = _tileMap.Tiles.Values;
            
            _wallMap = IoC.Resolve<IWallMap>();
            _wallMap.Init(_container.transform);
            _wallMap.CreateWalls(_tileMap.TileList);
        }

        public void CreateDoorWays(List<DoorToThe> doorWays) {
            if (_wallMap == null) return;

            foreach (var door in doorWays)
                _wallMap.CreateDoorWay(door);
        }

        public void SetPosition(Vector2Int pos) {
            _container.transform.position = new Vector3(pos.x, 0, pos.y);

            var mid = _rect.center;
            
            _rect.x += Mathf.FloorToInt(mid.x);
            _rect.y += Mathf.FloorToInt(mid.y);
            
            //_tileMap?.SetPosition(pos);
            _wallMap?.SetPosition(pos);
        }
    }
}