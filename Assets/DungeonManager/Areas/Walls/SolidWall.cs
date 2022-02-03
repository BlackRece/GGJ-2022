using System.Collections.Generic;

using UnityEngine;

namespace GGJ2022
{
    public interface IWall {
        GameObject GetGameObject { get; }
        Vector3 GameObjectPosition { get; }
        List<Area.DoorToThe> Directions { get; set; }
        void Hide();
    }
    
    public class SolidWall : MonoBehaviour, IWall
    {
        public GameObject GetGameObject => gameObject;
        public Vector3 GameObjectPosition => transform.position;

        private List<Area.DoorToThe> _direction;
        public List<Area.DoorToThe> Directions {
            get => _direction;
            set => _direction = value;
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        private void Awake() {
            _direction = new List<Area.DoorToThe> 
                {Area.DoorToThe.None};
        }
    }

}
