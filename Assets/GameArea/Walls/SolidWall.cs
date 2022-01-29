using UnityEngine;

namespace GGJ2022
{
    public interface IWall {
        GameObject GetGameObject { get; }
        Vector3 GameObjectPosition { get; }
    }
    
    public class SolidWall : MonoBehaviour, IWall
    {
        public GameObject GetGameObject => gameObject;
        public Vector3 GameObjectPosition => transform.position;
    }

}
