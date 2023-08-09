using Combat;
using DelayedActions;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBehaviour : MonoBehaviour
{
    [Tooltip("The measurement of the amount of damage this object can take or has taken.")]
    [SerializeField]
    private float _health;
    [Tooltip("The starting amount of damage this object can take or has taken. Set to -1 to start with max health.")]
    [SerializeField]
    private float _startingHealth = -1;
    [Tooltip("The maximum amount of damage this object can take or has taken.")]
    [SerializeField]
    private float _maxHealth;

    [Tooltip("Whether or not this object should be deleted if the health is 0.")]
    [SerializeField]
    private bool _destroyOnDeath;

    [SerializeField]
    private UnityEvent _onDeath;
    [SerializeField]
    private UnityEvent<GameObject> _onTakeDamage;
    private Rigidbody _rigidBody;
    private bool _inHitStun;
    private TimedAction _hitStunRoutine;
    private Material _attachedMaterial;
    private Color _defaultColor;
    private Color _hitStunColor = Color.grey;

    /// <summary>
    /// Gets whether or not this object's health is greater than 0.
    /// </summary>
    public virtual bool IsAlive
    {
        get
        {
            return _health > 0;
        }
    }

    /// <summary>
    /// The measurement of the amount of damage this object can take or has taken.
    /// </summary>
    public float Health
    {
        get => _health;
        protected set
        {
            //Prevent damage if the object is dead
            if (value <= _health && !IsAlive)
            {
                return;
            }

            _health = value;
        }
    }

    /// <summary>
    /// The maximum amount of damage this object can take or has taken.
    /// </summary>
    public float MaxHealth { get => _maxHealth; }
    public bool InHitStun { get => _inHitStun; private set => _inHitStun = value; }

    protected virtual void Awake()
    {
        if (_startingHealth < 0)
            _health = _maxHealth;
        else
            _health = _startingHealth;

        _rigidBody = GetComponent<Rigidbody>();
        _attachedMaterial = GetComponent<MeshRenderer>().material;
        _defaultColor = _attachedMaterial.color;
    }

    /// <summary>
    /// Decrements the given value from the health and returns the current health value.
    /// </summary>
    /// <param name="attacker">The owner of the hit box that is dealing damage.</param>
    /// <param name="damage">The amount of damage being applied to the object. 
    public virtual float TakeDamage(GameObject attacker, float damage)
    {
        Health -= damage;

        if (Health < 0)
            _health = 0;

        _onTakeDamage.Invoke(attacker);

        return Health;
    }

    /// <summary>
    /// Decrements the given value from the health and returns the current health value.
    /// </summary>
    /// <param name="attacker">The owner of the hit box that is dealing damage.</param>
    /// <param name="info">The data for the collider. Needed to apply damage, hit stun and knockback.</param>
    /// <returns></returns>
    public virtual float TakeDamage(GameObject attacker, HitColliderData info)
    {
        float damageTaken = Health;

        Health -= info.Damage;

        damageTaken -= Health;
        ActivateHitStun(info.HitStunTime);

        ApplyKnockBack(info.LaunchAngle, info.KnockBackForce);

        _onTakeDamage.Invoke(attacker);
        return damageTaken;
    }

    /// <summary>
    /// Adds an impulse force to the object using its mass.
    /// </summary>
    /// <param name="launchAngle">The direction to apply a force in.</param>
    /// <param name="force">The magnitude of the force vector that will be applied/</param>
    public virtual void ApplyKnockBack(float launchAngle, float force)
    {
        Vector3 velocity = new Vector3(Mathf.Cos(launchAngle), Mathf.Sin(launchAngle), 0) * force;

        _rigidBody.AddForce(velocity, ForceMode.Impulse);
    }

    /// <summary>
    /// Resets the health to be it's starting health or max health based on the starting health value.
    /// If the value was left at -1, the health will be set to the max.
    /// </summary>
    public virtual void ResetHealth()
    {
        _health = _startingHealth == -1 ? _maxHealth : _startingHealth;
    }

    /// <summary>
    /// Flags this object as being in hit stun briefly.
    /// </summary>
    /// <param name="time">the amount of time in seconds the object is in hit stun for.</param>
    public virtual void ActivateHitStun(float time)
    {
        InHitStun = true;
        _hitStunRoutine = CoroutineManager.Instance.StartNewTimedAction(args => InHitStun = false, TimeUnit.SCALEDTIME, time);
    }

    /// <summary>
    /// Disables hit stun and stops the timer.
    /// </summary>
    public virtual void CancelHitStun()
    {
        CoroutineManager.Instance.StopAction(_hitStunRoutine);
        InHitStun = false;
    }

    /// <summary>
    /// Adds an action to the event that is called once when this object's health reaches zero.
    /// </summary>
    /// <param name="action">The new listener to the event.</param>
    public void AddOnDeathAction(UnityAction action)
    {
        _onDeath.AddListener(action);
    }

    /// <summary>
    /// Adds an action to the event called when this object is damaged. Passes the attacker as an argument when invoked.
    /// </summary>
    /// <param name="action">The new listener to to the event</param>
    public void AddOnTakeDamageAction(UnityAction<GameObject> action)
    {
        _onTakeDamage.AddListener(action);
    }

    // Update is called once per frame
    public virtual void Update()
    {

        //Death check
        if (IsAlive && Health <= 0)
            _onDeath?.Invoke();

        //If the object has died and it should be removed on death...
        if (!IsAlive && _destroyOnDeath)
            //destroy the object.
            Destroy(gameObject);

        //Color code for testing purposes.
        if (InHitStun)
            _attachedMaterial.color = _hitStunColor;
        else
            _attachedMaterial.color = _defaultColor;
    }
}
/// <summary>
/// Editor script to test attacks
/// </summary>
#if UNITY_EDITOR

[CustomEditor(typeof(HealthBehaviour))]
class HealthEditor : Editor
{
    private HealthBehaviour _owner;
    private float _damage;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _owner = (HealthBehaviour)target;

        _damage = EditorGUILayout.FloatField("Damage", _damage);

        if (GUILayout.Button("Test Attack"))
        {
            _owner.TakeDamage(null, _damage);
        }
    }
}

#endif