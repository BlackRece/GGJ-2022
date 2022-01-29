using System;

using UnityEngine;

namespace GGJ2022
{
    public class SimpleFollowCamera : MonoBehaviour {
        [SerializeField] private Transform _target = default;
        [SerializeField] private float _turnSpeed = 0.01f;
        [SerializeField] private float _moveSpeed = 0.02f;
        [SerializeField] private Vector3 _offset = new Vector3(0,10,-10);

        private void LateUpdate() {
            var camTransform = transform;
            camTransform.position = Vector3.Lerp(camTransform.position, _target.position , _moveSpeed);
            camTransform.rotation = Quaternion.Lerp(camTransform.rotation, _target.rotation, _turnSpeed);
            //camTransform.rotation
        }
    }
}
