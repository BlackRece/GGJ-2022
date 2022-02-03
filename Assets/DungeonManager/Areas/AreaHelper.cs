using UnityEngine;

namespace GGJ2022 {
    public static class AreaHelper {
        public static Vector2Int MakeMiddleOdd(Vector2Int size) {
            var result = size;
            var half = new Vector2((float)size.x / 2, (float)size.y / 2);

            if (half.x % 2 == 0) result.x++;
            if (half.y % 2 == 0) result.y++;

            return result;
        }

        public static Vector2Int FindMiddle(Vector2Int size) {
            var fixedSize = MakeMiddleOdd(size);
            return new Vector2Int(
                (int) Mathf.Floor((float)fixedSize.x / 2), 
                (int) Mathf.Floor((float)fixedSize.y / 2));
        }
        
        public static Vector2 FindMiddle(Vector2 size) {
            var fixedSize = MakeMiddleOdd(new Vector2Int((int)size.x, (int)size.y));
            return new Vector2(
                Mathf.Floor((float)fixedSize.x / 2),
                Mathf.Floor((float)fixedSize.y / 2));
        }

        public static Vector2Int CalcPathSize(Area.DoorToThe direction, int pathSize) {
            var path = new Vector2Int(Area.PATH_WIDTH, Area.PATH_WIDTH);

            var oddPathSize = pathSize;
            if ((float) pathSize % 2 == 0) oddPathSize++;
            
            switch (direction) {
                case Area.DoorToThe.North:
                case Area.DoorToThe.South:
                    path.y += oddPathSize;
                    break;
                case Area.DoorToThe.East:
                case Area.DoorToThe.West:
                    path.x += oddPathSize;
                    break;
            }

            return path;
        }
    }
}