namespace GGJ2022 {
    public class Limit {
        private readonly int _min;
        private readonly int _max;

        public Limit(int min, int max) {
            _min = min;
            _max = max;
        }
        public int Fix(int value) {
            if (value > _max) return _max;
            if (value < _min) return _min;
            return value;
        }
    }

}