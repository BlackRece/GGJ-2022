using UnityEngine;

namespace GGJ2022
{
    public interface IInputHandler {
        Vector2 Movement { get; }
        bool IsShooting { get; }
    }

    public class InputHandler : MonoBehaviour, IInputHandler {

        private struct InputKeyWords {
            public const string HORIZONTAL = "Horizontal";
            public const string VERTICAL = "Vertical";
            
            public const string SHOOT = "Fire1";
        }
        
        public Vector2 Movement => new Vector2(
            Input.GetAxisRaw(InputKeyWords.HORIZONTAL),
            Input.GetAxisRaw(InputKeyWords.VERTICAL)
        );

        public bool IsShooting => Input.GetButtonDown(InputKeyWords.SHOOT);
    }
}
