using UnityEngine;

namespace GGJ2022
{
    public interface IMotionHandler {
        void DoMovement(Vector2 directionVector);
    }
    
    public sealed class MotionHandler : IMotionHandler
    {
        private readonly float _motionSpeed;
        private readonly Transform _transform;

        public MotionHandler(Transform transform, float motionSpeed) {
            _transform = transform;
            _motionSpeed = motionSpeed;
        }

        public void DoMovement(Vector2 directionVector) {
            var step = Time.deltaTime * _motionSpeed;
            
            _transform.Translate(
                directionVector.x * step,
                0,
                directionVector.y *  step
            );
        }
    }
}
