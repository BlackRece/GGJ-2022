using UnityEngine;

namespace GGJ2022 {
    public interface ICreatureSettings {
        float MotionSpeed { get; }
    }
    
    [CreateAssetMenu(menuName = "GGJ2022/Create creature settings")]
    public sealed class CreatureSettings : ScriptableObject, ICreatureSettings {
        [SerializeField] private float _motionSpeed = 1f;
        public float MotionSpeed => _motionSpeed;
    }
}