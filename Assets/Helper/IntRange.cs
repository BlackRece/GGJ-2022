using System;

namespace GGJ2022 {
    [Serializable]
    public sealed class IntRange {
        public int min;
        public int max;
        
        public IntRange(int min, int max) {
            this.min = min;
            this.max = max;
        }

        public int Random() => 
            UnityEngine.Random.Range(min, max);
    }
}