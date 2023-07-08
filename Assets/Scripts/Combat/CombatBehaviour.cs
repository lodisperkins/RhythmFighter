using Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Temp script for testing purposes.
/// </summary>

public class CombatBehaviour : MonoBehaviour
{
    [SerializeField]
    private AbilityData_SO _abilitySlot1;
    [SerializeField]
    private AbilityData_SO _abilitySlot2;

    private Ability _ability1;
    private Ability _ability2;

    // Start is called before the first frame update
    void Awake()
    {
        InitAbilities();
    }

    private void InitAbilities()
    {
        //Example of getting an ability instance from a scriptable object.

        string ability1Name = _abilitySlot1.name.Substring(0, _abilitySlot1.name.Length - 5);

        Type ability1Type = Type.GetType("Combat." + ability1Name);

        _ability1 = (Ability)Activator.CreateInstance(ability1Type);
        _ability1.Init(this, _abilitySlot1);
    }


    public void UseAbility1(params object[] args)
    {
        _ability1.UseAbility(args);
    }

    public void UseAbility2(params object[] args)
    {
        _ability2.UseAbility(args);
    }
}
