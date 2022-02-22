using System;

using UnityEngine;

namespace GGJ2022 {
    [Serializable]
    public sealed class IntSize {
        private int _width;
        public int Width {
            get => _width;
            set => _width = value;
        }
        
        private int _height;
        public int Height {
            get => _height;
            set => _height = value;
        }
        
        public IntSize(int width, int height) {
            _width = width;
            _height = height;
        }

        public static IntSize zero => new IntSize(0, 0);

        public Vector2Int Center() =>
            new Vector2Int(
                _width == 0 ? 0 : (int) Mathf.Floor((float) _width / 2),
                _height == 0 ? 0 : (int) Mathf.Floor((float) _height / 2));
    }
}