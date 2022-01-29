using System;

using UnityEngine;

namespace GGJ2022
{
    public class SimpleFollowCamera : MonoBehaviour {
        [SerializeField] private GameObject _target = default;
        [SerializeField] private float _turnSpeed = 50f;
        private Camera _cam = default;
        private Vector3 _offset = default;
        private Quaternion _rotateOffset = default;

        private void Awake() {
            _cam = Camera.main;
            _offset = _cam.transform.position;
            _rotateOffset = _cam.transform.rotation;
        }

        private void Update() {
            _cam.transform.position = (_target.transform.position + _offset);

            var lookRotation = Quaternion.LookRotation(_target.transform.forward);
            _cam.transform.rotation = lookRotation;
            
            //_cam.transform.rotation = Quaternion.Slerp(transform.rotation, _target.transform.rotation, Time.deltaTime * _turnSpeed);
        }

        private void LateUpdate() {
            var forward45 = Vector3.up;
            forward45.x += 45;
            _target.transform.rotation.ToAngleAxis(out var angle, out var axis);

            _cam.transform.position = (_target.transform.position + _offset);
            _rotateOffset = Quaternion.AngleAxis(angle, Vector3.up) * _rotateOffset;
            _cam.transform.LookAt(_target.transform.position);
        }
    }
}
