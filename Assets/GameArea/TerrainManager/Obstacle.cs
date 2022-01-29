using UnityEngine;

namespace GGJ2022 {
    public interface IObstacle {
        GameObject GetGameObject { get; }
        
        void Init(int healthAmount = 1);
        void IncreaseHealth(int healthAmount);
        void DecreaseHealth(int healthAmount);
    }

    public sealed class Obstacle : MonoBehaviour, IObstacle {
        private int _healthAmount = default;

        public GameObject GetGameObject => gameObject;
        
        public void Init(int healthAmount = 1) {
            IncreaseHealth(healthAmount);
        }
        
        public void IncreaseHealth(int healthAmount) => 
            _healthAmount += Mathf.Abs(healthAmount);

        public void DecreaseHealth(int healthAmount) {
            if (_healthAmount <= 0) {
                _healthAmount = 0;
                
                gameObject.SetActive(false);
                return;
            }
            
            _healthAmount -= Mathf.Abs(healthAmount);
        }
    }
}