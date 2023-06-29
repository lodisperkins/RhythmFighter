using Ilumisoft.VisualStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class CharacterStateMachineBehaviour : MonoBehaviour
{
    private CharacterMovement _characterMovement;
    private MoveSet _characterMoveSet;

    private StateMachine _stateMachine;
    private bool _canMove;
    private bool _canAttack;
    private bool _canJump;

    //Temporary variables for testing.

    [SerializeField]
    private bool _isIdle;
    [SerializeField]
    private bool _isInHitStun;

    public string CurrentState { get => _stateMachine.CurrentState; }
    public bool CanMove { get => _canMove; private set => _canMove = value; }
    public bool CanAttack { get => _canAttack; private set => _canAttack = value; }
    public bool CanJump { get => _canJump; private set => _canJump = value; }

    // Start is called before the first frame update
    void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _characterMoveSet = GetComponent<MoveSet>();

        //Cache refernce to attached state machine.
        _stateMachine = GetComponent<StateMachine>();

        //Initialize transition conditions.

        _stateMachine.SetTransitionConditionByLabel("Move", args => _characterMovement.IsMoving);
        _stateMachine.SetTransitionConditionByLabel("Attack", args => _characterMoveSet.IsAttacking);
        _stateMachine.SetTransitionConditionByLabel("Jump", args => _characterMovement.IsJumping);

        _stateMachine.SetTransitionCondition("Any-Idle", args => CheckIsIdle());
        _stateMachine.SetTransitionCondition("Any-HitStun", args => _isInHitStun);

    }

    private bool CheckIsIdle()
    {
        if (_characterMovement.IsMoving == false && _characterMovement.IsJumping == false && _characterMoveSet.IsAttacking == false && _isInHitStun == false)
        {
            _isIdle = true;
            return _isIdle;
        }
        else
        {
            _isIdle = false;
            return _isIdle;
        }
    }

    public void SetCanMove(bool canMove)
    {
        CanMove = canMove;
    }

    public void SetCanAttack(bool canAttack)
    {
        CanAttack = canAttack;
    }

    public void SetCanJump(bool canJump)
    {
        CanJump = canJump;
    }

}
