using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public delegate bool InputCondition();
    public delegate void InputBufferAction();
    
    /// <summary>
    /// Class to store actions that the player will perform once some condition is met.
    /// </summary>
    public class BufferedInput
    {
        private InputBufferAction _action;
        private float _bufferClearTime;
        private float _bufferStartTime;
        private InputCondition _condition;

        /// <param name="action">The action that will be performed once the condition is met.</param>
        /// <param name="condition">The condition that will be checked every frame to see if the player can perform the action.</param>
        /// <param name="bufferClearTime">The amount of time to wait to clear the buffer without performing the action.</param>
        public BufferedInput(InputBufferAction action, InputCondition condition, float bufferClearTime)
        {
            _action = action;
            _condition = condition;
            _bufferClearTime = bufferClearTime;
            _bufferStartTime = Time.time;
        }

        /// <summary>
        /// Attempts to perform the action that is currently stored.
        /// </summary>
        /// <returns>True, if the condition to perform the action has been met. False if there isn't an action or the buffer is cleared.</returns>
        public bool TryUseAction()
        {
            if (_action == null)
                return false;

            //If the condition required for the action has been met...
            if (_condition.Invoke())
            {
                //...perform the action.
                _action.Invoke();
                _action = null;
                return true;
            }

            //If enough time has passed...
            if (Time.time - _bufferStartTime >= _bufferClearTime)
                //...clear the action.
                _action = null;

            return false;
        }

        /// <returns>Whether or not there is an action store with this buffer. Actions can become null when cleared.</returns>
        public bool HasAction()
        {
            return _action != null;
        }
    }

    public class InputBehaviour : MonoBehaviour
    {
        private PlayerActions _playerControls;
        private CharacterMovement _characterMovement;
        [Tooltip("The number associated with the player. \n  1 - Player 1 \n 2 - Player 2")]
        [SerializeField]
        private int _playerID;
        [SerializeField]
        private bool _canJump;
        [SerializeField]
        private bool _canMove;
        [SerializeField]
        private bool _canAttack;

        private CharacterStateMachineBehaviour _stateMachine;

        private BufferedInput _bufferedAction;
        private InputDevice[] _devices;

        /// <summary>
        /// The devices that this input script will listen to input from.
        /// Used when mapping player controllers to specific characters.
        /// </summary>
        public InputDevice[] Devices 
        {
            get => _devices;
            set
            {
                _devices = value;
                _playerControls.devices = _devices;
            }
        }

        /// <summary>
        /// The number associated with the player.
        /// 1 - Player 1
        /// 2 - Player 2
        /// </summary>
        public int PlayerID { get => _playerID; set => _playerID = value; }

        // Start is called before the first frame update
        void Awake()
        {
            _stateMachine = GetComponent<CharacterStateMachineBehaviour>();
            _characterMovement = GetComponent<CharacterMovement>();
            _playerControls = new PlayerActions();

            _playerControls.Character.Move.performed += BufferMovement;
            _playerControls.Character.LightAttack.performed += BufferLightAttack;
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        private void BufferMovement(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            _bufferedAction = new BufferedInput(() => _characterMovement.MoveVector = direction, () => _stateMachine.CanJump, 60);
        }

        private void BufferLightAttack(InputAction.CallbackContext context)
        {
            _bufferedAction = new BufferedInput(() => Debug.Log("Light Attack input recieved."), () => _stateMachine.CanAttack, 60);
        }

        // Update is called once per frame
        void Update()
        {
            if (_bufferedAction?.HasAction() == true)
                _bufferedAction.TryUseAction();
        }
    }
}