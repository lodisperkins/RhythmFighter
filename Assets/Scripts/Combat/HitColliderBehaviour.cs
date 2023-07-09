using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// Class that is responsible for storing all info about a specific collider.
    /// </summary>
    [System.Serializable]
    public class HitColliderData
    {
        [Tooltip("The name of this collider. Used to identify this collider for debugging.")]
        public string Name;

        [Tooltip("Whether or not this collider can has a life span. If disabled, the time active value is ignored" +
            " and the collider will persist until it's destroyed.")]
        public bool DespawnAfterTimeLimit;
        [Tooltip("The amount of time it will take to automatically despawn this collider if Despawn After Time Limit is enabled.")]
        public float TimeActive;
        [Tooltip("Whether or not this collider should be destroyed in the next frame after collding with a valid object.")]
        public bool DestroyOnHit;
        [Tooltip("Whether or not this collider is allowed to register a collision multiple times without the object exitting the collider. " +
            "Uses Unity's OnTriggerStay as opposed to OnTriggerEnter.")]
        public bool IsMultiHit;
        [Tooltip("The amount of time this collider must wait before registering another valid collision if Is MultiHit is enabled.")]
        public float MultiHitWaitTime;
        [Tooltip("The layers that this collider will consider a valid collision. Internal collision events and dealing damage will only be occur" +
            "upon collision with game objects in these layers.")]
        public LayerMask CollisionLayers;
        [Tooltip("The amount of health this collider will try to decrement from the receiver.")]
        public float Damage;

        [Header("Collision Effects")]
        [Tooltip("The object that will be spawned when this hit collider is active. This is useful for spawning particle effects like muzzle flashes or charge effects.")]
        public GameObject SpawnEffect;
        [Tooltip("The object that will be spawned when this hit collider collides with a valid object. This is useful for spawning particle effects like hit sparks.")]
        public GameObject HitEffect;

        public AudioClip SpawnSound;
        public AudioClip HitSound;
        public AudioClip DespawnSound;

        public CollisionEvent OnHit;
    }

    public class HitColliderBehaviour : ColliderBehaviour
    {
        [SerializeField]
        private HitColliderData _colliderInfo;
        private bool _playedSpawnEffects;

        private float _startTime;
        private float _currentTimeActive;

        public float StartTime { get => _startTime; set => _startTime = value; }
        public float CurrentTimeActive { get => _currentTimeActive; set => _currentTimeActive = value; }
        public HitColliderData ColliderInfo { get => _colliderInfo; set => _colliderInfo = value; }

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
           CollisionLayers = ColliderInfo.CollisionLayers;
           StartTime = Time.time;
        }

        public override void AddCollisionEvent(CollisionEvent collisionEvent)
        {
            ColliderInfo.OnHit += collisionEvent;
        }

        /// <summary>
        /// Checks if the collider has waited long enough to register another collision event with the same game object.
        /// Mainly useful when IsMultiHit is enabled.
        /// </summary>
        /// <param name="gameObject">The game object that this collider has already collider with.</param>
        private bool CheckHitTime(GameObject gameObject)
        {
            float lastHitTime = 0;

            //If the game object hasn't been collided with yet...
            if (!Collisions.TryGetValue(gameObject, out lastHitTime))
            {
                //...add it to the collision dictionary.
                Collisions.Add(gameObject, Time.time);
                return true;
            }

            //If the collider has waited long enough...
            if (Time.time - lastHitTime >= ColliderInfo.MultiHitWaitTime)
            {
                //...update the time collided with the object.
                Collisions[gameObject] = Time.time;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets the amount of time this collider has been active.
        /// Useful to renew the lifespan of objects that despawn over time.
        /// </summary>
        public void ResetActiveTime()
        {
            StartTime = Time.time;
        }

        /// <summary>
        /// Checks to see if the collision is valid, calls collision events, and deals damage if applicable.
        /// </summary>
        /// <param name="other">The collider of the other object.</param>
        private void ResolveCollision(Collider other)
        {
            //If collision isn't allowed with the object collided with or if the collider should be destroyed...
            if (!CheckIfCollisionAllowed(other.gameObject.layer) || (Collisions.Count > 0 && ColliderInfo.DestroyOnHit))
            {
                return;
            }

            GameObject otherGameObject = null;
            //If there is a rigidy body in the object's hierarchy...
            if (other.attachedRigidbody)
                //...store its game object.
                otherGameObject = other.attachedRigidbody.gameObject;
            //If there isn't a rigid body attached in the hierarchy...
            else
                //...store the game object of the collider.
                otherGameObject = other.gameObject;

            ColliderBehaviour otherCollider = other.GetComponent<ColliderBehaviour>();

            //If collision isn't allowed with the potential parent of the object or if it's the owner...
            if (!CheckIfCollisionAllowed(otherGameObject.layer) || otherGameObject == Owner)
                return;

            //If the other object is a hit collider, check if that collider allows collision with this object/
            if (otherCollider && (otherCollider.Owner == Owner || !otherCollider.CheckIfCollisionAllowed(gameObject.layer)))
                return;

            if (ColliderInfo.HitEffect)
                Instantiate(ColliderInfo.HitEffect, transform.position, Camera.main.transform.rotation);
            
            //If this object hasn't been collided with...
            if (!Collisions.ContainsKey(otherGameObject.gameObject))
                //...added to the collision dictionary.
                Collisions.Add(otherGameObject.gameObject, Time.time);

            HealthBehaviour damageScript = otherGameObject.GetComponent<HealthBehaviour>();

            if (damageScript)
                damageScript.TakeDamage(Owner, ColliderInfo);

            ColliderInfo.OnHit?.Invoke(otherGameObject, otherCollider, other, this, damageScript);

            if (ColliderInfo.DestroyOnHit)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Collisions.ContainsKey(other.gameObject))
                return;

            ResolveCollision(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!ColliderInfo.IsMultiHit || !CheckHitTime(other.gameObject))
                return;

            ResolveCollision(other);

        }

        private void OnTriggerExit(Collider other)
        {
            Collisions.Remove(other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (Collisions.ContainsKey(collision.gameObject))
                return;

            ResolveCollision(collision.collider);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!ColliderInfo.IsMultiHit || !CheckHitTime(collision.gameObject))
                return;

            ResolveCollision(collision.collider);
        }

        private void OnCollisionExit(Collision other)
        {
            Collisions.Remove(other.gameObject);
        }

        private void Update()
        {
            if (!gameObject)
                return;

            CurrentTimeActive = Time.time - StartTime;

            //Checks to see when the collider should be destroyed.
            if (CurrentTimeActive >= ColliderInfo.TimeActive && ColliderInfo.DespawnAfterTimeLimit)
            {
                if (ColliderInfo.HitEffect)
                    Instantiate(ColliderInfo.HitEffect, transform.position, Camera.main.transform.rotation);

                Destroy(gameObject);
            }
        }
    }
}