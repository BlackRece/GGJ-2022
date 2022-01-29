using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

namespace GGJ2022 {
    public sealed class TileMap : ScriptableObject {
        [SerializeField] private int PATH_WIDTH = 3;
        [SerializeField] private int MAX_PATH_LENGTH = 10;
        private const int MAX_TILE_GROUPS = 100;
        
        private Dictionary<Vector2Int, ITile> _tilemap;
        private Dictionary<Vector3, IWall> _walls;
        private Dictionary<Vector3, IObstacle> _obstacles;
        
        private Transform _parentTransform;
        
        private enum Wall {
            None = 0,
            North,
            South,
            East,
            West
        }
        
        private enum AreaType {
            Path = 0,
            Room,
            Spawn
        }

        private void OnEnable() {
            _tilemap = new Dictionary<Vector2Int, ITile>();
            _walls = new Dictionary<Vector3, IWall>();
            _obstacles = new Dictionary<Vector3, IObstacle>();
        }

        public void CreateMap(Vector2Int startingRoomSize, Transform transform) {
            _parentTransform = transform;

            int roomCounter = 0;
            
            //start location
            var currentPos = new Vector2Int();
            var nextRoom = new RoomDimension(startingRoomSize);

            Vector2Int path;
            int pathLength;
            AreaType type;
            
            while (roomCounter < MAX_TILE_GROUPS) {
                if (roomCounter == 0 || roomCounter >= MAX_TILE_GROUPS - 1) 
                    type = AreaType.Spawn;
                else 
                    type = AreaType.Room;

                //room
                CreateArea(currentPos, nextRoom.Size, type);
                
                //get path length for next location
                pathLength = Random.Range(0, MAX_PATH_LENGTH) + 1;
                
                //ensure odd numbered path length
                if (pathLength % 2 == 0) pathLength++;
                
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
                    distance.y -= (pathLength / 2 + room.Center.y) +1;
                    break;
                case Wall.South:
                    distance.y += (pathLength / 2 + room.Center.y) -1;
                    break;
                case Wall.East:
                    distance.x += (pathLength / 2 + room.Center.x) -1;
                    break;
                case Wall.West:
                    distance.x -= (pathLength / 2 + room.Center.x) +1;
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
        
        //TODO: Add doorways
        //TODO: Method too big! break down!
        private void CreateArea(Vector2Int position, Vector2Int size, AreaType type = AreaType.Path) {
            var halfSize = GetRoomCenter(size);
            
            var tileContainer = new GameObject($"Tiles ({position.x} : {position.y})");
            tileContainer.transform.SetParent(_parentTransform);
            
            var wallContainer = new GameObject($"Walls ({position.x} : {position.y})");
            wallContainer.transform.SetParent(_parentTransform);
            
            var obstacleContainer = new GameObject($"Obstacles ({position.x} : {position.y})");
            obstacleContainer.transform.SetParent(_parentTransform);

            var isOverlapping = false;
            
            for (var x = 0; x < size.x; x++) {
                if (isOverlapping) break;
                
                for (var y = 0; y < size.y; y++) {
                    if (isOverlapping) break;
                    
                    var tilePosition = new Vector2Int(
                        position.x + ( x - halfSize.x),
                        position.y + (y - halfSize.y)
                    );

                    if (_tilemap.ContainsKey(tilePosition)) {
                        //isOverlapping = true;
                        //break;
                        continue;
                    }
                    
                    GenerateTileAt(
                        tilePosition.x,
                        tilePosition.y,
                        tileContainer.transform
                    );

                    var tile = GetTileAt(tilePosition);
                    
                    if (type == AreaType.Path) {
                        if (tile.HasChildren) {
                            var obj = tile.GetTransform.GetChild(0);
                            if (_obstacles.ContainsKey(obj.position)) 
                                _obstacles.Remove(obj.position);

                            if (_walls.ContainsKey(obj.position))
                                _walls.Remove(obj.position);
                            
                            Destroy(obj);
                        }
                        continue;
                    }

                    if (tile.HasChildren) continue;
                    
                    if (x == 0 || x == size.x - 1 || y == 0 || y == size.y - 1) {
                        var wallObject = IoC.Resolve<IWall>().GetGameObject;
                        var wallPosition = GetObjectPosition(wallObject, tile.GetTopPosition());

                        if (_walls.ContainsKey(wallPosition)) {
                            isOverlapping = true;
                            continue;
                        }

                        var wall = Instantiate(
                            wallObject,
                            wallPosition,
                            Quaternion.identity,
                            tile.GetTransform
                        );
                        _walls.Add(wallPosition, wall.GetComponent<IWall>());
                    }

                    if (type == AreaType.Spawn) continue;
                        
                    if (x > 0 && x < size.x && y > 0 && y < size.y) {
                        var obstacleObject = IoC.Resolve<IObstacle>().GetGameObject;
                        var obstaclePosition = GetObjectPosition(obstacleObject, tile.GetTopPosition());

                        if (_obstacles.ContainsKey(obstaclePosition)) {
                            isOverlapping = true;
                            continue;
                        }
                        
                        var obstacle = Instantiate(
                            obstacleObject,
                            obstaclePosition,
                            Quaternion.identity,
                            tile.GetTransform
                        );
                        _obstacles.Add(obstaclePosition, obstacle.GetComponent<IObstacle>());
                    }
                }
            }

            if (isOverlapping || tileContainer.transform.childCount <= 3) {
                for (var i = tileContainer.transform.childCount - 1; i >= 0; i--) {
                    if (obstacleContainer.transform.childCount == i + 1) {
                        var obstacleChild = obstacleContainer.transform.GetChild(i);
                        if (_obstacles.ContainsKey(obstacleChild.position)) 
                            _obstacles.Remove(obstacleChild.position);
                        
                        Destroy(obstacleChild);
                    }
                    
                    if (wallContainer.transform.childCount == i + 1) {
                        var wallChild = wallContainer.transform.GetChild(i);
                        if (_walls.ContainsKey(wallChild.position)) 
                            _walls.Remove(wallChild.position);
                        
                        Destroy(wallChild);
                    }
                    
                    if(!isOverlapping) continue;
                    if (tileContainer.transform.childCount == i + 1) {
                        var tileChild = tileContainer.transform.GetChild(i);
                        var tileInstance = tileChild.GetComponent<ITile>();
                        if (_tilemap.ContainsKey(tileInstance.GetMapPosition())) 
                            _tilemap.Remove(tileInstance.GetMapPosition());
                        
                        Destroy(tileChild);
                    }
                }
            }
        }

        private Vector3 GetObjectPosition(GameObject gameObject, Vector3 topOfTilePosition) {
            topOfTilePosition.y += GetGameObjectHeight(gameObject) / 2;
            return topOfTilePosition;
        }

        private ITile GetTileAt(Vector2Int tileMapPosition) =>
            _tilemap.ContainsKey(tileMapPosition) ? _tilemap[tileMapPosition] : null;

        private Vector2Int GetRoomCenter(Vector2Int size) {
            var result = size / 2;
            
            if (size.x % 2 != 0) 
                result.x++;

            if (size.y % 2 != 0)
                result.y++;

            return result;
        }

        private float GetGameObjectHeight(GameObject gameObject) => 
            gameObject.GetComponent<Renderer>().bounds.size.y;

        private void GenerateTileAt(int x, int y, Transform parentTransform) {
            var mapPosition = new Vector2Int(x, y);
            
            //need to ensure rooms don't overlap...
            if (_tilemap.ContainsKey(mapPosition))
                return;
            
            var tileGO = Instantiate(
                IoC.Resolve<ITile>().GetGameObject,
                parentTransform
            );
            
            var tile = tileGO.GetComponent<ITile>();
            
            tileGO.transform.position = tile.GetWorldPosition(mapPosition);
            _tilemap.Add(mapPosition, tile);
        }

    }
}