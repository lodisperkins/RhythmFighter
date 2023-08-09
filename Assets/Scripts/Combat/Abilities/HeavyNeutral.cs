using DelayedActions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

    /// <summary>
    /// Enter ability description here
    /// </summary>
    public class HeavyNeutral : Ability
    {
        private float _offset;
        private float _hitBoxSpawnDelay;
        private Material _ownerMaterial;

        //Called when ability is created
        protected override void OnInit(CombatBehaviour newOwner, AbilityData_SO data)
        {
            _offset = AbilityData.GetCustomStatValue("Hitbox Offset");
            _hitBoxSpawnDelay = AbilityData.GetCustomStatValue("Hitbox Spawn Delay");
            _ownerMaterial = newOwner.GetComponent<MeshRenderer>().material;
        }

        protected override void OnStart(params object[] args)
        {
            _ownerMaterial.color = Color.green;
        }

        private void SpawnHitBox(int index)
        {
            Vector3 spawnPosition = Owner.transform.position + Owner.transform.forward * _offset;
            GameObject instance = MonoBehaviour.Instantiate(AbilityData.VisualPrefab, spawnPosition, AbilityData.VisualPrefab.transform.rotation);
            HitColliderBehaviour hitCollider = instance.AddComponent<HitColliderBehaviour>();
            hitCollider.ColliderInfo = AbilityData.GetCollliderInfo(0);
            hitCollider.Owner = Owner.gameObject;
        }

        //Called when ability is used
        protected override void OnActivate(params object[] args)
        {
            _ownerMaterial.color = Color.blue;
            SpawnHitBox(0);

            CoroutineManager.Instance.StartNewTimedAction(arguments => SpawnHitBox(1), TimeUnit.SCALEDTIME, _hitBoxSpawnDelay);
        }

        protected override void OnRecover(params object[] args)
        {
            base.OnRecover(args);
            _ownerMaterial.color = Color.red;
        }

        protected override void OnEnd(params object[] args)
        {
            _ownerMaterial.color = Color.white;
        }
    }
}