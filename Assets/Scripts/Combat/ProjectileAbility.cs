using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// Base class for all projectile based abilities
    /// </summary>
    [System.Serializable]
    public class ProjectileAbility : Ability
    {
        public Transform SpawnTransform;
        //Usd to store a reference to the projectile prefab
        public GameObject ProjectileRef;
        public Vector3 ShotDirection;
        public GameObject Projectile;
        //The collider attached to the projectile
        public HitColliderData ProjectileColliderData;
        public List<GameObject> ActiveProjectiles = new List<GameObject>();
        public bool DestroyOnHit = true;
        public bool IsMultiHit = false;
        public bool UseGravity;
        public bool despawnAfterTimeLimit { get; private set; }
        public float Speed;
        public int MaxInstances;

        protected override void OnInit(CombatBehaviour newOwner, AbilityData_SO data)
        {

            //initialize default stats

            ProjectileRef = AbilityData.VisualPrefab;
            Speed = AbilityData.GetCustomStatValue("Speed");
            MaxInstances = (int)AbilityData.GetCustomStatValue("MaxInstances");
        }

        public void CleanProjectileList(bool useName = false)
        {
            for (int i = 0; i < ActiveProjectiles.Count; i++)
            {
                if (!ActiveProjectiles[i] || !ActiveProjectiles[i].activeInHierarchy || (ActiveProjectiles[i].name != ProjectileRef.name + "(" + AbilityData.name + ")" && useName))
                {
                    ActiveProjectiles.RemoveAt(i);
                    i--;
                }
            }
        }

        protected override void OnStart(params object[] args)
        {
            base.OnStart(args);
            ProjectileColliderData = GetColliderData(0);
            CleanProjectileList();
        }
         
        protected override void OnActivate(params object[] args)
        {

            //Log if a projectile couldn't be found
            if (!ProjectileRef)
            {
                Debug.LogError("Projectile for " + AbilityData.AbilityName + " could not be found.");
                return;
            }

            if (ActiveProjectiles.Count >= MaxInstances && MaxInstances != -1)
                return;

            ShotDirection = Owner.transform.forward;

            HitColliderData data = ProjectileColliderData;

            Projectile = MonoBehaviour.Instantiate(ProjectileRef, Owner.transform.position, Owner.transform.rotation);

            HitColliderBehaviour collider;
            if (!Projectile.TryGetComponent(out collider))
                collider = Projectile.AddComponent<HitColliderBehaviour>();

            collider.ColliderInfo = GetColliderData(0);
            collider.Owner = Owner.gameObject;
            Rigidbody rigidBody = Projectile.GetComponent<Rigidbody>();

            if (rigidBody == null)
            {
                rigidBody = Projectile.AddComponent<Rigidbody>();
            }

            rigidBody.useGravity = UseGravity;

            rigidBody.AddForce(ShotDirection * Speed, ForceMode.VelocityChange);

            //Fire projectile
            Projectile.name += "(" + AbilityData.name + ")";
            ActiveProjectiles.Add(Projectile);
        }

        public void DestroyActiveProjectiles()
        {
            CleanProjectileList();
            for (int i = 0; i < ActiveProjectiles.Count; i++)
            {
                MonoBehaviour.Destroy(ActiveProjectiles[i]);
            }
        }
    }
}