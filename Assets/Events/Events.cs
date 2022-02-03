using UnityEngine;
using UnityEngine.Events;

namespace GGJ2022
{
    public class Events : ScriptableObject
    {
        public event UnityAction TerrainComplete = delegate { };
        
        public event UnityAction StartedRunning = delegate { };
        public event UnityAction StoppedRunning = delegate { };

        public void OnTerrainComplete() {
            TerrainComplete.Invoke();
        }
    }
}
