using UnityEngine;

namespace GGJ2022
{
    public interface ICreature { }
    
    [RequireComponent(typeof(IInputHandler), typeof(IMotionHandler))]
    public class Player : MonoBehaviour, ICreature {
        private IInputHandler _inputHandler = default;
        private IMotionHandler _motionHandler = default;

        private void Awake() {
            _inputHandler = GetComponent<IInputHandler>();
            _motionHandler = GetComponent<IMotionHandler>();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

}
