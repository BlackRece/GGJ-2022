using UnityEngine;

namespace GGJ2022
{
    public class AreaHandler : MonoBehaviour
    {
        [SerializeField]
        private float _distanceToFloor;

        private void Awake() {
            if (_distanceToFloor <= 0) _distanceToFloor = 1f;
        }

        void Update() {
            CastRayToFloor();
        }

        private void CastRayToFloor() {
            if (!Physics.Raycast(transform.position, Vector3.down, out var tileHit, _distanceToFloor)) 
                return;

            if (!tileHit.transform.TryGetComponent(typeof(ITile), out var tile))
                return;

            ITile floorTile = (ITile) tile;
            floorTile.FlagAsVisited();
        }
    }
}
