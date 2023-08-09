using Ilumisoft.VisualStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StateMachine))]
public class CharacterStateMachineBehaviour : MonoBehaviour
{
    private CharacterMovement _characterMovement;
    private CombatBehaviour _characterCombatBehavior;
    private HealthBehaviour _characterHealth;
    private StateMachine _stateMachine;

    //These variables are set upon entry of a state within the state machine
    private bool _canMove = true;
    private bool _canAttack = true;
    private bool _canJump = true;

    //Temporary variables for testing.
    [SerializeField]
    private bool _isIdle = true;

    public string CurrentState { get => _stateMachine.CurrentState; }
    public bool CanMove { get => _canMove; private set => _canMove = value; }
    public bool CanAttack { get => _canAttack; private set => _canAttack = value; }
    public bool CanJump { get => _canJump; private set => _canJump = value; }

    // Start is called before the first frame update
    void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _characterCombatBehavior = GetComponent<CombatBehaviour>();
        _characterHealth = GetComponent<HealthBehaviour>();

        //Cache refernce to attached state machine.
        _stateMachine = GetComponent<StateMachine>();

        //Initialize transition conditions.
        _stateMachine.SetTransitionConditionByLabel("Move", args => _characterMovement.IsMoving);
        _stateMachine.SetTransitionConditionByLabel("Attack", args => _characterCombatBehavior.AbilityInUse);
        _stateMachine.SetTransitionConditionByLabel("Jump", args => _characterMovement.IsJumping);

        _stateMachine.SetTransitionCondition("Any-Idle", args => CheckIsIdle());
        _stateMachine.SetTransitionCondition("Any-HitStun", args => _characterHealth.InHitStun);
    }

    //This determines if the player is idle based on what are and aren't able to do
    private bool CheckIsIdle()
    {
        if (_characterMovement.IsMoving == false && _characterMovement.IsJumping == false
            && _characterCombatBehavior.AbilityInUse == false
            && _characterHealth.InHitStun == false)
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
