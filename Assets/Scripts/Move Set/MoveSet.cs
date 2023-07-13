using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;
using System;

    public class MoveSet : MonoBehaviour
{
    private List<string> _bufferedAttacksList = new List<string>();

    [SerializeField]
    private AbilityData_SO _abilitySlot1;
    [SerializeField]
    private AbilityData_SO _abilitySlot2;

    private Ability _ability1;
    private Ability _ability2;

    private string _currentBufferedAttack;

    private float _moveDirection;

    private Vector2 _moveVector;
    public Vector2 MoveVector
    {
        get { return _moveVector; }
        set 
        {
            _moveVector = MoveVector;
            if (value.x != 0)
                _moveDirection = Mathf.Sign(value.x);
            else
                _moveDirection = 0;
        }
    }

    private bool _abilityInUse = false;
    public bool AbilityInUse
    {
        get { return _abilityInUse; }
        set { _abilityInUse = value; }
    }

    void Awake()
    {
       
    }

    public void LightAttackUsed()
    {       
        if (_moveDirection != 0)
        {
            Debug.Log("directional light attack used");
            AttackTracker("DL");
        }
        else
        {
            Debug.Log("light attack used");
            AttackTracker("L");
        }
    }

    public void HeavyAttackUsed()
    {        
        if (_moveDirection != 0)
        {
            Debug.Log("directional heavy attack used");
            AttackTracker("DH");
        }
        else
        {
            Debug.Log("heavy attack used");
            AttackTracker("H");
        }
    }

    public void SpecialAttackUsed()
    { 
        if (_moveDirection != 0)
        {
            Debug.Log("directional special attack used");
            AttackTracker("DS");
        }
        else
        {
            Debug.Log("special attack used");
            AttackTracker("S");
        }
    }

    private void AttackTracker(String _attackBuffered)
    {
        _currentBufferedAttack = _attackBuffered;

        if (_bufferedAttacksList.Count >= 5)
        {
            _bufferedAttacksList.RemoveAt(0);
            _bufferedAttacksList.Add(_attackBuffered);
        }
    }

    void Update()
    {
        
    }
}
