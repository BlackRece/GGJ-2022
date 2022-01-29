using UnityEngine;

namespace GGJ2022 {
    public sealed class RoomDimension {
        private const int MAX_ROOM_SIZE = 50;
        private const int MIN_ROOM_SIZE = 10;
        
        private Vector2Int _center;
        public Vector2Int Center {
            get {
                if (_center == default) 
                    _center = GetCenter();

                return _center;
            }
        }
            
        private Vector2Int _size;
        public Vector2Int Size {
            get => _size;
            private set {
                var limiter = new Limit(MIN_ROOM_SIZE, MAX_ROOM_SIZE);
                _size = new Vector2Int(
                    limiter.Fix(value.x),
                    limiter.Fix(value.y)
                );
            }
        }

        public RoomDimension() => 
            _size = default;
        public RoomDimension(int width, int length) => 
            _size = new Vector2Int(width, length);
        public RoomDimension(Vector2Int dimension) => 
            _size = dimension;

        public static RoomDimension Randomise() =>
            new RoomDimension(
                Random.Range(MIN_ROOM_SIZE, MAX_ROOM_SIZE),
                Random.Range(MIN_ROOM_SIZE, MAX_ROOM_SIZE));

        private Vector2Int GetCenter() {
            var result = _size / 2;
            
            if (_size.x % 2 != 0) 
                result.x++;

            if (_size.y % 2 != 0)
                result.y++;

            return result;
        }
    }
}