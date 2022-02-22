using System.Collections.Generic;

using UnityEngine;

namespace GGJ2022 {
    public sealed class AreaBuilder {
        private Area.AreaType _type;
        private Vector2Int _size;
        private Transform _parent;
        private ITileMap _tileMap;
        private IWallMap _wallMap;

        public static Area Path(Vector2Int pathSize, Area.DoorToThe door, Transform parentTransform) {
            return new AreaBuilder()
                .WithParent(parentTransform)
                .WithTypeOf(Area.AreaType.Path)
                .WithSizeOf(pathSize)
                .WithFloorTiles()
                .WithDoorWays(new List<Area.DoorToThe> {door})
                .Build();
        }

        public static Area Room(Vector2Int roomSize, List<Area.DoorToThe> doors, Transform parentTransform) {
            return new AreaBuilder()
                .WithParent(parentTransform)
                .WithTypeOf(Area.AreaType.Room)
                .WithSizeOf(roomSize)
                .WithFloorTiles()
                .WithDoorWays(doors)
                .WithWalls()
                .Build();
        }

        public static Area Spawn(Vector2Int roomSize, Area.DoorToThe side, Transform parentTransform) {
            return new AreaBuilder()
                .WithParent(parentTransform)
                .WithTypeOf(Area.AreaType.Spawn)
                .WithSizeOf(roomSize)
                .WithFloorTiles()
                .WithDoorWays(new List<Area.DoorToThe> {side})
                .WithWalls()
                .Build();
        }
        
        private Area Build() => new Area(_type, _size, _parent, _tileMap, _wallMap);

        private AreaBuilder WithParent(Transform transform) {
            _parent = transform;
            return this;
        }
        
        private AreaBuilder WithTypeOf(Area.AreaType type) {
            _type = type;
            return this;
        }

        private AreaBuilder WithSizeOf(Vector2Int size) {
            _size = AreaHelper.MakeMiddleOdd(size);
            return this;
        }

        private AreaBuilder WithFloorTiles() {
            _tileMap = IoC.Resolve<ITileMap>();
            _tileMap.Init(_parent, new IntSize(_size.x, _size.y));
            //_tileMap.CreateTiles(TODO);
            return this;
        }
        
        private AreaBuilder WithWalls() {
            if (_tileMap == null) return this;
            
            _wallMap = IoC.Resolve<IWallMap>();
            _wallMap.Init(_parent);
            _wallMap.CreateWalls(_tileMap.TileList);
            return this;
        }

        private AreaBuilder WithDoorWays(List<Area.DoorToThe> doorWays) {
            if (_wallMap == null) return this;

            foreach (var door in doorWays)
                _wallMap.CreateDoorWay(door);
            
            return this;
        }
    }
}