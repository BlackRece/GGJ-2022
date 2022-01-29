using UnityEngine;

namespace GGJ2022
{
    public interface IInputHandler {
        float Forward { get; }
        float Rotation { get; }
        
        Vector2 Movement { get; }
        bool IsShooting { get; }
        bool IsMoving { get; }
    }

    public class InputHandler : MonoBehaviour, IInputHandler {

        private struct InputKeyWords {
            public const string HORIZONTAL = "Horizontal";
            public const string VERTICAL = "Vertical";
            
            public const string SHOOT = "Fire1";
        }
        

        public float Forward => Input.GetAxisRaw(InputKeyWords.VERTICAL);
        public float Rotation => Input.GetAxisRaw(InputKeyWords.HORIZONTAL);
        public Vector2 Movement => new Vector2(Forward, Rotation);

        public bool IsMoving => Movement != Vector2.zero;

        public bool IsShooting => Input.GetButtonDown(InputKeyWords.SHOOT);
    }
}
