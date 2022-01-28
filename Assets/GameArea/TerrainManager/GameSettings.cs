using UnityEngine;

namespace GGJ2022 {
    [CreateAssetMenu (menuName = "GGJ2022/Game Settings")]
    public sealed class GameSettings : ScriptableObject {
        [Header("Tile Prefab: ")] [SerializeField]
        private GameObject _tilePrefab = null;
        
        private void OnEnable() {
            IoC.Initialise(new DependencyManager());

            IoC.Register<ITile>(_tilePrefab);
            // IoC.Register<IEnemy>(_enemyPrefab);
            // IoC.Register<IBullet>(_bulletPrefab);
            // IoC.Register<ILocation>(_destinationPrefab);
            // IoC.Register<ISpawner>(_spawnPrefab);
            // IoC.Register<ITower>(_towerPrefab);
        }
    }
}