using UnityEngine;

namespace GGJ2022 {
    public interface IAreaDetail {
        RectInt Rect { get; set; }
        IntSize Size { get; set; }
        Vector2Int Position { get; set; }
    }

    public sealed class AreaDetail : IAreaDetail {
        private RectInt _rect = default;
        public RectInt Rect {
            get {
                if (_rect.center == Vector2.zero) {
                    _rect = new RectInt(Vector2Int.zero, new Vector2Int(_size.Width, _size.Height));
                    _rect = AreaHelper.CenterRectAt(_rect, Position);
                }
                return _rect;
            }

            set => _rect = value;
        }

        public Area.AreaType Type;

        private IntSize _size;
        public IntSize Size {
            get => _size;
            set => _size = value;
        }

        private Vector2Int _position;
        public Vector2Int Position {
            get => _position;
            set => _position = value;
        }

        public Transform Parent;
    }
}