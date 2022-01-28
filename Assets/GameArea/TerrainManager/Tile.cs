using UnityEngine;

namespace GGJ2022 {
    public interface ITile {
        GameObject GetGameObject { get; }

        Vector3 GetTopPosition();
        Vector3 GetWorldPosition(Vector2Int mapPosition);
    }

    public sealed class Tile : MonoBehaviour, ITile {
        private Vector3 _modelSize;
        public GameObject GetGameObject => gameObject;

        private void Awake() {
            var renderer = GetComponent<Renderer>();
            _modelSize = renderer.bounds.size;
        }

        public Vector3 GetWorldPosition(Vector2Int mapPosition) =>
            new Vector3(mapPosition.x * _modelSize.x, 0, mapPosition.y * _modelSize.z);
        
        public Vector3 GetTopPosition() {
            var position = _modelSize;
            position.y += _modelSize.y / 2;
            return position;
        }
    }
}