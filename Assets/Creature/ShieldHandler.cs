using UnityEngine;

namespace GGJ2022 {
    public interface IShieldHandler {
        void ActivateShield(bool state);
    }

    public sealed class ShieldHandler : MonoBehaviour, IShieldHandler {
        [SerializeField] private GameObject _shieldPrefab = default;
        private bool _isShieldActive = default;

        private void Awake() {
            _isShieldActive = false;
            _shieldPrefab.SetActive(false);
        }

        public void ActivateShield(bool state) {
            _isShieldActive = !state;
        }

        private void Update() {
          
            if (_shieldPrefab.activeSelf != _isShieldActive)
                _shieldPrefab.SetActive(_isShieldActive);
            
        }
    }
}