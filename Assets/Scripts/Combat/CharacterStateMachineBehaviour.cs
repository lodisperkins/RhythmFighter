using Ilumisoft.VisualStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class CharacterStateMachineBehaviour : MonoBehaviour
{
    private StateMachine _stateMachine;
    private bool _canMove;
    private bool _canAttack;

    //Temporary variables for testing.

    [SerializeField]
    private bool _isMoving;
    [SerializeField]
    private bool _isAttacking;
    [SerializeField]
    private bool _isJumping;
    [SerializeField]
    private bool _isIdle;
    [SerializeField]
    private bool _isInHitStun;

    public string CurrentState { get => _stateMachine.CurrentState; }
    public bool CanMove { get => _canMove; private set => _canMove = value; }
    public bool CanAttack { get => _canAttack; private set => _canAttack = value; }

    // Start is called before the first frame update
    void Awake()
    {
        //Cache refernce to attached state machine.
        _stateMachine = GetComponent<StateMachine>();

        //Initialize transition conditions.

        _stateMachine.SetTransitionConditionByLabel("Move", args => _isMoving);
        _stateMachine.SetTransitionConditionByLabel("Attack", args => _isAttacking);
        _stateMachine.SetTransitionConditionByLabel("Jump", args => _isJumping);

        _stateMachine.SetTransitionCondition("Any-Idle", args => _isIdle);
        _stateMachine.SetTransitionCondition("Any-HitStun", args => _isInHitStun);

    }

    public void SetCanMove(bool canMove)
    {
        CanMove = canMove;
    }

    public void SetCanAttack(bool canAttack)
    {
        CanAttack = canAttack;
    }
}
