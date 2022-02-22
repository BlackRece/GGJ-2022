using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace GGJ2022 {
    public interface IArea {
        Guid Id { get; }
        
        IntSize Size { get; }
        Vector2Int Center { get; }
        void SetPosition(Vector2Int pos);
        ITileMap TileMap { get; }
        void FlagAsVisited();

        ITile GetRandomTile();
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

        private Guid _id;
        public Guid Id => _id;
        
        private string _name;
        private GameObject _container;
        public GameObject Container => _container;

        private ITileMap _tileMap;
        public ITileMap TileMap => _tileMap;

        private IWallMap _wallMap;
        public IWallMap WallMap => _wallMap;

        private bool _isVisited;

        public static UnityEvent<IArea> AreaVisitedHendler;
        
        public Area(AreaDetail detail) {
            _type = detail.Type;
            _size = detail.Size;
            _position = detail.Position;

            CreateContainer(detail.Parent);
            
            _isVisited = false;
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
            _tileMap.SetParentArea(this);
        }

        public void CreateWalls() {
            if (_tileMap == null) return;

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
            
            _wallMap?.SetPosition(pos);
        }
        
        public void FlagAsVisited() {
            if(_isVisited)
                return;
            
            _isVisited = true;

            //launch event to spawn enemies
            var randomTile = _tileMap.GetRandomTile();
            var tileGO = randomTile.GetGameObject;
            //CreatureManager.SpawnCreatureOnTile(randomTile.GetTopPosition());
            //AreaVisitedHendler?.Invoke(this);
            CreatureManager.SpawnCreatureHandler.Invoke(this);
        }

        public ITile GetRandomTile() => _tileMap.GetRandomTile();
    }
}