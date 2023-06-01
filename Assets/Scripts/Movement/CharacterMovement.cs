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

    void Awake()
    {
        _playerRb = GetComponent<Rigidbody>();
    }

    public void CharacterMove(int _moveDirection)
    {
        //_moveDirection is a +/-1 that determines whether the player is going left or right
        float _horizontalTranslation = _moveDirection * _walkSpeed;

        //applies one frame of horizontal movement
        transform.Translate(_horizontalTranslation, 0, 0);
    }

    public void CharacterJump(int _moveDirection)
    {
        //singular application of complete vertical force
        if(_moveDirection == 0)
            _playerRb.AddForce(Vector2.up * _jumpForce, ForceMode.Impulse);
        else
            _playerRb.AddForce(new Vector2(_moveDirection * _jumpAngleMod, 1) * _jumpForce, ForceMode.Impulse);
    }
}
