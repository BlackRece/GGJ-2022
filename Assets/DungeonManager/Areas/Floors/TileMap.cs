using System.Collections.Generic;

using UnityEngine;

namespace GGJ2022 {
    public interface ITileMap {
        Vector2Int Size { get; }
        GameObject TileContainer { get; }
        
        void Init(Transform parentTransform, Vector2Int size);
        void CreateTiles();

        public List<ITile> Tiles { get; }
        void SetPosition(Vector2Int pos);
    }
    
    public sealed class TileMap : ScriptableObject, ITileMap {
        private Transform _parent;
        private string _name;
        
        private Dictionary<Vector2Int, ITile> _tiles;
        public List<ITile> Tiles => new List<ITile>(_tiles.Values);

        //private Dictionary<Vector3, IObstacle> _obstacles;
        
        private Vector2Int _size;
        public Vector2Int Size => _size;
        
        private GameObject _tileContainer;
        public GameObject TileContainer => _tileContainer;

        public void Init(Transform parent, Vector2Int size) {
            _parent = parent;
            _size = size;
            
            _name = $"Floor ({size.x} : {size.y})";
            _tileContainer = new GameObject(_name);
            _tileContainer.transform.SetParent(_parent);
            
            _tiles = new Dictionary<Vector2Int, ITile>();
        }
        
        private void OnEnable() {
            _tiles = new Dictionary<Vector2Int, ITile>();
        }

        public void CreateTiles() {
            var mid = AreaHelper.FindMiddle(_size);

            for (var x = -mid.x; x <= mid.x; x++) {
                for (var y = -mid.y; y <= mid.y; y++) {
                    var mapPosition = new Vector2Int(x, y);
                    var tile = GenerateTile(mapPosition);

                    if (x == -mid.x) 
                        tile.FlagAsEdge(Area.DoorToThe.West);
                    if (x == mid.x)
                        tile.FlagAsEdge(Area.DoorToThe.East);
                    if (y == -mid.y)
                        tile.FlagAsEdge(Area.DoorToThe.North);
                    if (y == mid.y)
                        tile.FlagAsEdge(Area.DoorToThe.South);

                    _tiles.Add(mapPosition, tile);
                }
            }
        }
        
        public void SetPosition(Vector2Int pos) {
            var offset = new Vector3(pos.x, 0, pos.y);
            
            _tileContainer.transform.position = offset;
            
            foreach (var tile in _tiles.Values) {
                tile.GetGameObject.transform.position += offset;
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