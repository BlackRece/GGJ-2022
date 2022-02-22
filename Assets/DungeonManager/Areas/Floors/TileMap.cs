using System.Collections.Generic;

using UnityEngine;

namespace GGJ2022 {
    public interface ITileMap {
        IntSize Size { get; }
        GameObject TileContainer { get; }
        
        void Init(Transform parentTransform, IntSize size);
        void CreateTiles(Vector2Int position);

        public List<ITile> TileList { get; }
        Dictionary<Vector2Int, ITile> Tiles { get; }
    }
    
    public sealed class TileMap : ScriptableObject, ITileMap {
        private Transform _parent;
        private string _name;
        
        private Dictionary<Vector2Int, ITile> _tiles;

        public Dictionary<Vector2Int, ITile> Tiles => _tiles;
        public List<ITile> TileList => new List<ITile>(_tiles.Values);

        //private Dictionary<Vector3, IObstacle> _obstacles;
        
        private IntSize _size;
        public IntSize Size => _size;
        
        private GameObject _tileContainer;
        public GameObject TileContainer => _tileContainer;

        public void Init(Transform parent, IntSize size) {
            _parent = parent;
            _size = size;
            
            _name = $"Floor ({size.Width} : {size.Height})";
            _tileContainer = new GameObject(_name);
            _tileContainer.transform.SetParent(_parent);
            
            _tiles = new Dictionary<Vector2Int, ITile>();
        }
        
        private void OnEnable() {
            _tiles = new Dictionary<Vector2Int, ITile>();
        }

        public void CreateTiles(Vector2Int position) {
            var mid = _size.Center();

            var width = new IntRange(position.x - mid.x, position.x + mid.x);
            var height = new IntRange(position.y - mid.y, position.y + mid.y);
            
            for (var x = width.min; x <= width.max; x++) {
                for (var y = height.min; y <= height.max; y++) {
                    var mapPosition = new Vector2Int(x, y);
                    var tile = GenerateTile(mapPosition);

                    if (x == width.min) 
                        tile.FlagAsEdge(Area.DoorToThe.West);
                    if (x == width.max)
                        tile.FlagAsEdge(Area.DoorToThe.East);
                    if (y == height.min)
                        tile.FlagAsEdge(Area.DoorToThe.North);
                    if (y == height.max)
                        tile.FlagAsEdge(Area.DoorToThe.South);

                    _tiles.Add(mapPosition, tile);
                }
            }
        }
        
        private ITile GenerateTile(Vector2Int mapPosition) {
            var tileGO = Instantiate(
                IoC.Resolve<ITile>().GetGameObject,
                _tileContainer.transform
            );
            
            var generateTile = tileGO.GetComponent<ITile>();
            tileGO.transform.position = generateTile.GetWorldPosition(mapPosition);

            return generateTile;
        }

        public List<ITile> GetEdgeTiles() {
            var edgeTiles = new List<ITile>();

            foreach (var tile in _tiles.Values) {
                if (!tile.IsEdge) continue;
                
                edgeTiles.Add(tile);
            }

            return edgeTiles;
        }
    }
}