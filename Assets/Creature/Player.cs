using UnityEngine;

namespace GGJ2022
{
    public interface ICreature { }
    
    [RequireComponent(typeof(IInputHandler))]
    public sealed class Player : MonoBehaviour, ICreature {
        [SerializeField] private CreatureSettings _settings = default;

        private IInputHandler _inputHandler = default;
        private IMotionHandler _motionHandler = default;
        private IShieldHandler _shieldHandler = default;

        private void Awake() {
            _inputHandler = GetComponent<IInputHandler>();
            _motionHandler = new MotionHandler(transform, _settings.MotionSpeed, _settings.RotateSpeed);
            
            _shieldHandler = GetComponent<IShieldHandler>();
        }

        private void Update()
        {
            _motionHandler.Move(_inputHandler.Forward);
            _motionHandler.Turn(_inputHandler.Rotation);

            _shieldHandler.RequestShield(_inputHandler.IsMoving);
        }
    }

}
