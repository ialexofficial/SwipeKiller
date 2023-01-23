using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;
using ViewModels;

namespace Components
{
    [RequireComponent(
    typeof(Rigidbody),
    typeof(Collider),
    typeof(MeshFilter)
    )]
    [RequireComponent(
        typeof(MeshRenderer),
        typeof(Animator)
    )]
    public class Weapon : MonoBehaviour
    {
    #if UNITY_EDITOR
        [Header("Variable only for editor previewing")]
        public WeaponScriptableObject PreviewingWeapon;
        [Space]
    #endif
        
        [SerializeField] private int damage;
        [SerializeField] private LayerMask damagableLayers;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        public void UpdateData(WeaponScriptableObject data)
        {
            _meshFilter ??= GetComponent<MeshFilter>();
            _meshRenderer ??= GetComponent<MeshRenderer>();
            
            _meshFilter.mesh = data.WeaponMeshFilter.sharedMesh;
            _meshRenderer.material = data.WeaponMaterial;
            damage = data.Damage;
        }

        private void OnCollisionEnter(Collision other)
        {
            GameObject gameObject = other.gameObject;
            
            if (LayerMasker.CheckLayer(damagableLayers, gameObject.layer))
            {
                gameObject.GetComponent<DamagableViewModel>().Damage(damage);
            }
        }
    }
}