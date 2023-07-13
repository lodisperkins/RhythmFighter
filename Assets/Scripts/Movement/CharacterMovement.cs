using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody _playerRb;
    private Collider _playerCollider;

    [SerializeField]
    private LayerMask floorLayerMask;
    [SerializeField]
    private float _walkSpeed = 1;
    [SerializeField]
    private float _jumpForce = 1;
    [SerializeField]
    private float _jumpAngleMod = 0.25f;

    private bool _isMoving = false;
    private bool _isJumping = false;
    private bool _isGrounded = true;
    private bool _holdingJump = false;

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
            if (value.x != 0) {_isMoving = true; _moveDirection = Mathf.Sign(value.x); }
            else { _moveDirection = 0; _isMoving = false; }

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
    }

    /// <summary>
    /// Takes in a horizontal movement input and translates in the desired direction
    /// </summary>
    public void CharacterMove()
    {
        float _horizontalTranslation = _moveDirection * _walkSpeed;

        if (_moveDirection != 0 && _isJumping == false) { _isMoving = true; }
        else _isMoving = false;

        if(_isGrounded == true)
        transform.Translate(_horizontalTranslation, 0, 0);
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
            _playerRb.AddForce(new Vector2(_moveDirection * _jumpAngleMod, 1) * _jumpForce, ForceMode.Impulse);
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
        }
        else
        {
            rayColor = Color.red;
            _isJumping = true;
            _isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        CharacterMove();
        IsGroundedCheck();

        if (_holdingJump && _isGrounded)
            CharacterJump();
    }
}
