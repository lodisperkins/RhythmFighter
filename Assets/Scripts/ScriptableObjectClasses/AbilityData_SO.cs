using Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class Stat
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private float _value;

    public Stat(string newName, float newValue)
    {
        Name = newName;
        Value = newValue;
    }

    public string Name { get => _name; set => _name = value; }
    public float Value { get => _value; set => _value = value; }
}


[CreateAssetMenu(menuName = "AbilityData/Default")]
public class AbilityData_SO : ScriptableObject
{
    public string AbilityName = "Unassigned";

    public AbilityType TypeOfAbility;

    [TextArea]
    public string AbilityDescription = "None";

    [Header("Usage Timing")]
    [Tooltip("How long the object that used the ability must wait before the ability activates.")]
    public float StartUpTime;
    [Tooltip("How long the ability should be active for.")]
    public float ActiveTime = 0;
    [Tooltip("How long the object that used the ability needs before returning to idle.")]
    public float RecoverTime;

    [Header("Sound And Appearance")]
    [Tooltip("The prefab that holds the visual representation of this ability.")]
    public GameObject VisualPrefab;
    [Tooltip("The sound clip player when this ability begins.")]
    public AudioClip StartSound;
    [Tooltip("The sound clip player when this ability is in its active state.")]
    public AudioClip ActiveSound;
    [Tooltip("The sound clip player when this ability is in its recover state.")]
    public AudioClip DeactivateSound;

    [Header("Usage Stats")]
    [Tooltip("Information for all colliders this ability will use.")]
    [SerializeField]
    private HitColliderData[] _colliderData;
    [Tooltip("Any additional stats this ability needs to keep track of.")]
    [SerializeField]
    protected Stat[] _customStats;

    [Header("Animation Options")]
    [Tooltip("If true, the animation will change speed to match the start, active, and recover times.")]
    public bool UseAbilityTimingForAnimation;
    [Tooltip("A unique animation that will be used for the attack instead of one of the defaults. Only used if the animation type is set to custom.")]
    [SerializeField]
    private AnimationClip _animation;

    /// <summary>
    /// Searches for a stat value that matches the name and returns it if found.
    /// </summary>
    /// <param name="statName">The name of the stat value.</param>
    /// <returns>The value of the stat. Return NaN if the stat couldn't be found.</returns>
    public float GetCustomStatValue(string statName)
    {
        foreach (Stat stat in _customStats)
        {
            if (stat.Name == statName)
                return stat.Value;
        }

        throw new Exception(
            "Couldn't find stat. Either the stat doesn't exist or the name is misspelled. Stat name to search for was " +
            statName);
    }

    /// <summary>
    /// Gets the hit collider data at the given index.
    /// </summary>
    /// <param name="index">The index of the hit collider data as it appears in the hierarchy.</param>
    /// <returns>The collider at the index. Returns null if the index was invalid.</returns>
    public HitColliderData GetCollliderInfo(int index)
    {
        if (index < 0 || index >= _colliderData.Length)
        {
            return null;
        }

        return _colliderData[index];
    }

    public int ColliderInfoCount
    {
        get { return _colliderData.Length; }
    }

    public AnimationClip Animation { get => _animation; set => _animation = value; }
}
