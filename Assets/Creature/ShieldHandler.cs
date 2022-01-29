using UnityEngine;

namespace GGJ2022 {
    public interface IShieldHandler {
        void RequestShield(bool state);
    }

    public sealed class ShieldHandler : MonoBehaviour, IShieldHandler {
        [SerializeField] private GameObject _shieldPrefab = default;
        [SerializeField] private float _shieldDelay = 2.0f;
        private bool _isShieldActive = default;
        private float _counter;

        private void Awake() {
            _isShieldActive = false;
            _shieldPrefab.SetActive(false);
        }

        public void RequestShield(bool state) {
            _isShieldActive = !state;
            
            if (!_isShieldActive && _counter == 0.0f)
                ResetTimer();
        }

        private void Update() {
            if (!_isShieldActive)
                DeactivateShield();
            else 
                ProcessTimer();
        }

        private void DeactivateShield() {
            if (_shieldPrefab.activeSelf)
                _shieldPrefab.SetActive(false);
        }
        
        private void ActivateShield() {
            if (!_shieldPrefab.activeSelf)
                _shieldPrefab.SetActive(true);
        }

        private void ProcessTimer() {
            if (_counter > 0.0f) {
                DecreaseTimer();
                return;
            }

            _counter = 0.0f;    
            _shieldPrefab.SetActive(_isShieldActive);
        }

        private void ResetTimer() => _counter = _shieldDelay;
        private void DecreaseTimer() => _counter -= Time.deltaTime; 
    }
}