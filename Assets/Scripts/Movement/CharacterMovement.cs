using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody _playerRb;

    //Would take in a charater unique variables
    private float _walkSpeed = 8; 
    private float _jumpForce = 20;
    private float _jumpAngleMod = 0.25f;

    //This variable changes depending on what the input script reads
    private int _moveDirection = 0;
    public int MoveDirection 
    { 
        get { return _moveDirection; }
        set { _moveDirection = value; }
    }

    void Awake()
    {
        _playerRb = GetComponent<Rigidbody>();
    }

    public void CharacterMove()
    {
        //_moveDirection is a +/-1 that determines whether the player is going left or right
        float _horizontalTranslation = _moveDirection * _walkSpeed;

        //applies one frame of horizontal movement
        transform.Translate(_horizontalTranslation, 0, 0);
    }

    //Input script calls this function once since it is a single application of force
    public void CharacterJump()
    {
        if(_moveDirection == 0)
            _playerRb.AddForce(Vector2.up * _jumpForce, ForceMode.Impulse);
        else
            _playerRb.AddForce(new Vector2(_moveDirection * _jumpAngleMod, 1) * _jumpForce, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        CharacterMove();
    }
}
