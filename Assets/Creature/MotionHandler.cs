using UnityEngine;

namespace GGJ2022
{
    public interface IMotionHandler {
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

        public void Move(float forward) {
            var step = Time.deltaTime * _motionSpeed * forward;

            _transform.Translate(_transform.forward * step, Space.World);
        }

        public void Turn(float rotation) {
            var spin = Time.deltaTime * _rotateSpeed;
            _transform.Rotate(0, rotation * spin, 0);
        }
    }
}
