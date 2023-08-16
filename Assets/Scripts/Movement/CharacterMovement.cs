using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody _playerRb;
    private Collider _playerCollider;

    [SerializeField]
    private GameObject _facingChecker;
    [SerializeField]
    private Collider _playerFacingCollider;
    [SerializeField]
    private LayerMask floorLayerMask;
    [SerializeField]
    private float _walkSpeed = 0.1f;
    [SerializeField]
    private float _jumpForce = 1;
    [SerializeField]
    private float _jumpAngleMod = 0.25f;
    [SerializeField]
    private float _backwalkSpeed = 0.05f;

    private bool _isMoving = false;
    private bool _isJumping = false;
    private bool _isGrounded = true;
    private bool _holdingJump = false;
    private bool _swapFacingOnLanding = false;

    [SerializeField]
    private bool _facingRight = true;

    private float _horizontalTranslation;

    private CharacterStateMachineBehaviour _stateMachine;


    //Variables to be refferenced by the CharacterStateMachineBehaviour script
    public bool IsMoving
    {
        get { return _isMoving; }
        private set { _isMoving = value; }
    }

    public bool IsJumping
    {
        get { return _isJumping; }
        private set { _isJumping = value; }
    }

    public bool IsGrounded
    {
        get { return _isGrounded; }
        private set { _isGrounded = value; }
    }
    public bool HoldingJump
    {
        get { return _holdingJump; }
        set { _holdingJump = value; }
    }

    private Vector2 _moveVector;
    public Vector2 MoveVector 
    { 
        get { return _moveVector; }
        set
        {
            _moveVector = value;
            if (value.x != 0) 
            {
                _isMoving = true; 
                _moveDirection = Mathf.Sign(value.x);

                if (_facingRight && _moveDirection == 1 || !_facingRight && _moveDirection == -1)
                    _horizontalTranslation = _moveDirection * _walkSpeed;
                else
                    _horizontalTranslation = _moveDirection * _backwalkSpeed;
            }
            else { _moveDirection = 0; _isMoving = false; _horizontalTranslation = 0; }

            if(value.y > 0) { _holdingJump = true; }
            else { _holdingJump = false; }
        }
    }
    public float _moveDirection;
    public float MoveDirection
    {
        get { return _moveDirection; }
        set { _moveDirection = value; }
    }

    void Awake()
    {
        _playerRb = GetComponent<Rigidbody>();
        _playerCollider = GetComponent<BoxCollider>();
        _stateMachine = GetComponent<CharacterStateMachineBehaviour>();
    }

    /// <summary>
    /// Takes in a horizontal movement input and translates in the desired direction
    /// </summary>
    public void CharacterMove()
    {
        if (_moveDirection != 0 && _isJumping == false) { _isMoving = true; }
        else _isMoving = false;

        if(_isGrounded == true)
        {
            if(_facingRight)
                transform.Translate(0, 0, _horizontalTranslation);
            else
                transform.Translate(0, 0, -_horizontalTranslation);
        }
    }

    /// <summary>
    /// Called when the player jumps and applies a single instance of force.
    /// </summary>
    public void CharacterJump()
    {
        if (_moveDirection == 0)
        {
            _playerRb.velocity = new Vector3();
            _playerRb.AddForce(Vector2.up * _jumpForce, ForceMode.Impulse);
        }
        else
        {
            _playerRb.velocity = new Vector3();
            _playerRb.AddForce(new Vector3(_moveDirection * _jumpAngleMod, 1, 0) * _jumpForce, ForceMode.Impulse);
        }
    }

    public void IsGroundedCheck()
    {
        float extraHeightTest = 0.025f;
        Color rayColor;

        if (Physics.Raycast(transform.position, -Vector3.up, _playerCollider.bounds.extents.y + extraHeightTest, floorLayerMask))
        {
            rayColor = Color.green;
            if (_moveDirection != 0) { _isMoving = true; }
            _isJumping = false;
            _isGrounded = true;
            if(_swapFacingOnLanding)
                SwapFacing();

        }
        else
        {
            rayColor = Color.red;
            _isJumping = true;
            _isGrounded = false;
        }
    }

    private void SwapFacing()
    {
        _facingRight = !_facingRight;
        transform.Rotate(180, 0, 0);
        //transform.rotation = Quaternion.Euler(0, 90, 0);

        if (_facingRight && _moveDirection == 1 || !_facingRight && _moveDirection == -1)
            _horizontalTranslation = _moveDirection * _walkSpeed;
        else
            _horizontalTranslation = _moveDirection * _backwalkSpeed;

        _swapFacingOnLanding = false;
    }

    /// <summary>
    /// This collider extends above and bellow, and can only collide with player facing collision (unique collider). It only serves the purpose of determining when to swap character facing
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FacingChecker" && other.gameObject != _facingChecker)
        {
            if (!IsGrounded)
                _swapFacingOnLanding = true;
            else
            {
                SwapFacing();
            }
        }
    }

    void FixedUpdate()
    {
        IsGroundedCheck();

        if (_stateMachine.CurrentState == "HitStun")
            return;

        CharacterMove();

        if (_holdingJump && _isGrounded)
            CharacterJump();
    }
}
