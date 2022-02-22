using UnityEngine;
using UnityEngine.Events;

namespace GGJ2022
{
    public class Events : ScriptableObject
    {
        public static event UnityAction TerrainComplete;
        
        public event UnityAction StartedRunning = delegate { };
        public event UnityAction StoppedRunning = delegate { };

        public void OnTerrainComplete() {
            TerrainComplete.Invoke();
        }
    }
}
