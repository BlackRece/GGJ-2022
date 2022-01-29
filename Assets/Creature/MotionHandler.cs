using UnityEngine;

namespace GGJ2022
{
    public interface IMotionHandler {
        void DoMovement(Vector2 directionVector);
        void Move(float forward);
        void Turn(float rotation);
    }
    
    public sealed class MotionHandler : IMotionHandler
    {
        private readonly Transform _transform;
        
        private readonly float _motionSpeed;
        private readonly float _rotateSpeed;

        public MotionHandler(Transform transform, float motionSpeed, float rotateSpeed) {
            _transform = transform;
            _motionSpeed = motionSpeed;
            _rotateSpeed = rotateSpeed;
        }

        public void DoMovement(Vector2 directionVector) {
            var step = Time.deltaTime * _motionSpeed;
            
            _transform.Translate(
                directionVector.x * step,
                0,
                directionVector.y *  step
            );
        }

        public void Move(float forward) {
            var step = Time.deltaTime * _motionSpeed;

            _transform.rotation.ToAngleAxis(out var angle, out var axis);
            _transform.Translate(_transform.forward * (forward * step), Space.World);
        }

        public void Turn(float rotation) {
            var spin = Time.deltaTime * _rotateSpeed;
            _transform.Rotate(0, rotation * spin, 0);
        }
    }
}
