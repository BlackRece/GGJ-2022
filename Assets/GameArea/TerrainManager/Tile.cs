using UnityEngine;

namespace GGJ2022 {
    public interface ITile {
        GameObject GetGameObject { get; }
        Transform GetTransform { get; }
        bool HasChildren { get; }

        Vector2Int GetMapPosition();
        Vector3 GetTopPosition();
        Vector3 GetWorldPosition(Vector2Int mapPosition);
    }

    public sealed class Tile : MonoBehaviour, ITile {
        private Vector3 _modelSize;
        public GameObject GetGameObject => gameObject;
        public Transform GetTransform => transform;
        public bool HasChildren => transform.childCount > 0;

        private void Awake() {
            var renderer = GetComponent<Renderer>();
            _modelSize = renderer.bounds.size;
        }

        public Vector3 GetWorldPosition(Vector2Int mapPosition) =>
            new Vector3(mapPosition.x * _modelSize.x, 0, mapPosition.y * _modelSize.z);

        public Vector2Int GetMapPosition() =>
            new Vector2Int(
                (int) (transform.position.x / _modelSize.x),
                (int) (transform.position.z / _modelSize.z));

        public Vector3 GetTopPosition() {
            var position = transform.position;
            position.y += _modelSize.y / 2;
            return position;
        }
    }
}