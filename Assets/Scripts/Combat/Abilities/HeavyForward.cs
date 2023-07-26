using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

    /// <summary>
    /// Enter ability description here
    /// </summary>
    public class HeavyForward : Ability
    {
        private float _offset;
        private Material _ownerMaterial;

        //Called when ability is created
        protected override void OnInit(CombatBehaviour newOwner, AbilityData_SO data)
        {
            _offset = AbilityData.GetCustomStatValue("Hitbox Offset");
            _ownerMaterial = newOwner.GetComponent<MeshRenderer>().material;
        }

        protected override void OnStart(params object[] args)
        {
            _ownerMaterial.color = Color.green;
        }

        //Called when ability is used
        protected override void OnActivate(params object[] args)
        {
            _ownerMaterial.color = Color.blue;

            Vector3 spawnPosition = Owner.transform.position + Owner.transform.right * _offset;
            GameObject instance = MonoBehaviour.Instantiate(AbilityData.VisualPrefab, spawnPosition, AbilityData.VisualPrefab.transform.rotation);
            HitColliderBehaviour hitCollider = instance.AddComponent<HitColliderBehaviour>();
            hitCollider.ColliderInfo = AbilityData.GetCollliderInfo(0);
            hitCollider.Owner = Owner.gameObject;
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