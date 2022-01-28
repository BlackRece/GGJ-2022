using System;

using UnityEngine;

namespace GGJ2022
{
    public class SimpleFollowCamera : MonoBehaviour {
        [SerializeField] private GameObject _target = default;
        private Camera _cam = default;
        private Vector3 _offset = default;

        private void Awake() {
            _cam = Camera.main;
            _offset = _cam.transform.position;
        }

        private void Update() {
            _cam.transform.position = _target.transform.position + _offset;
        }
    }
}
