using Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AbilityType
{
    LightNeutral,
    LightForward,
    HeavyNeutral,
    HeavyForward,
    SpecialNeutral,
    SpecialForward
}

public class CombatBehaviour : MonoBehaviour
{
    //Keeps track of both directional and attack inputs
    private List<string> _inputList = new List<string>();
    private List<float> _inputTimesList = new List<float>();

    [SerializeField]
    private List<AbilityData_SO> _abilitiesData = new List<AbilityData_SO>();
    [SerializeField]
    private List<Ability> _abilities = new List<Ability>();

    private Ability _currentAbility;

    private float _moveDirection;

    private Vector2 _moveVector;
    public Vector2 MoveVector
    {
        get { return _moveVector; }
        set
        {
            _moveVector = MoveVector;

            if (value.x != 0) { _moveDirection = Mathf.Sign(value.x); }
            else { _moveDirection = 0; }

            TrackMotionInputs(value);
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
        InitAbilities();
    }

    private void TrackMotionInputs(Vector2 value)
    {
        if (_inputList.Count >= 20)
        {
            _inputList.RemoveAt(0);
            _inputTimesList.RemoveAt(0);
        }

        if (value == new Vector2(1, 0)) { _inputList.Add("R"); }
        else if (value == new Vector2(-1, 0)) { _inputList.Add("L"); }
        else if (value == new Vector2(0, -1)) { _inputList.Add("D"); }
        else if (new Vector2(((float)Math.Round(value.x, 1)), ((float)Math.Round(value.y, 1))) == new Vector2(0.7f, -0.7f)) { _inputList.Add("DR");  }
        else if (new Vector2(((float)Math.Round(value.x, 1)), ((float)Math.Round(value.y, 1))) == new Vector2(-0.7f, -0.7f)) { _inputList.Add("DL"); }
        else { _inputList.Add("N"); }
    }

    private void InitAbilities()
    {
        foreach(AbilityData_SO data in _abilitiesData)
        {
            string abilityName = data.name.Substring(0, data.name.Length - 5);

            Type abilityType = Type.GetType("Combat." + abilityName);

            Ability ability = (Ability)Activator.CreateInstance(abilityType);
            ability.Init(this, data);
            _abilities.Add(ability);
        }
    }

    public void LightAttackUsed()
    {
        if (_moveDirection != 0)
        {
            InputTracker(AbilityType.LightForward);
        }
        else
        {
            InputTracker(AbilityType.LightNeutral);
        }
    }

    public void HeavyAttackUsed()
    {
        if (_moveDirection != 0)
        {
            InputTracker(AbilityType.HeavyForward);
        }
        else
        {
            InputTracker(AbilityType.HeavyForward);
        }
    }

    public void SpecialAttackUsed()
    {
        if (_moveDirection != 0)
        {
            InputTracker(AbilityType.SpecialForward);
        }
        else
        {
            InputTracker(AbilityType.SpecialNeutral);
        }
    }

    private void InputTracker(AbilityType type)
    {
        if (_inputList.Count >= 20)
        {
            _inputList.RemoveAt(0);
            _inputTimesList.RemoveAt(0);
        }

        _inputList.Add(type.ToString());
        _inputTimesList.Add(Time.time);

        //If there are certain input combination to check before using an ability, that would be checked here
        UseAbility(type);
    }

    public void UseAbility(AbilityType type, params object[] args)
    {
        _currentAbility = _abilities.Find(ability => ability.AbilityData.TypeOfAbility == type);
        _currentAbility.UseAbility(args);
        _abilityInUse = true;
    }
    public void FinishRecovery()
    {
        _abilityInUse = false;
    }
}
