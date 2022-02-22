using System;

using UnityEngine;
using UnityEngine.Events;

namespace GGJ2022
{
    [CreateAssetMenu(menuName = "GGJ2022/New Creature Manager")]
    public class CreatureManager : ScriptableObject {
        private MonoBehaviour _parent;

        //public delegate ICreature SpawnCreatureHandler(Vector3 position);

        public static Action<IArea> SpawnCreatureHandler;

        public void Init(MonoBehaviour parent) {
            _parent = parent;

            SpawnCreatureHandler += OnSpawnCreatureInArea;
        }

        private void OnSpawnCreatureInArea(IArea area) {
            var tile = area.GetRandomTile();
            
            var sampleCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var samplePosition = tile.GetTopPosition();
            samplePosition.y++;
            sampleCube.transform.position = samplePosition;
        }
    }
}
