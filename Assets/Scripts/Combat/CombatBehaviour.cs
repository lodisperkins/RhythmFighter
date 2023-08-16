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
    public bool AbilityInUse
    {
        get { return _currentAbility?.InUse == true; }
    }

    void Awake()
    {
        InitAbilities();
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


    public void UseAbility(AbilityType type, params object[] args)
    {
        if (AbilityInUse)
            return;

        _currentAbility = _abilities.Find(ability => ability.AbilityData.TypeOfAbility == type);
        _currentAbility.UseAbility(args);
    }
}
