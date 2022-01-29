using UnityEngine;

namespace GGJ2022 {
    [CreateAssetMenu (menuName = "GGJ2022/Game Settings")]
    public sealed class GameSettings : ScriptableObject {
        [Header("Tile Prefab: ")] [SerializeField]
        private GameObject _tilePrefab = null;
        
        [Header("Wall Prefab: ")] [SerializeField]
        private GameObject _wallPrefab = null;
        
        [Header("Obstacle Prefab: ")] [SerializeField]
        private GameObject _obstaclePrefab = null;
        
        private void OnEnable() {
            IoC.Initialise(new DependencyManager());

            IoC.Register<ITile>(_tilePrefab);
            IoC.Register<IWall>(_wallPrefab);
            IoC.Register<IObstacle>(_obstaclePrefab);
            // IoC.Register<IEnemy>(_enemyPrefab);
            // IoC.Register<IBullet>(_bulletPrefab);
            // IoC.Register<ILocation>(_destinationPrefab);
            // IoC.Register<ISpawner>(_spawnPrefab);
            // IoC.Register<ITower>(_towerPrefab);
        }
    }

}