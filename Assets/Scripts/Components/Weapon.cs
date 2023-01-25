using ScriptableObjects;
using UnityEngine;
using Utilities;
using ViewModels;

namespace Components
{
    [RequireComponent(
    typeof(Rigidbody),
    typeof(MeshCollider),
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
        private MeshCollider _meshCollider;

        public void UpdateData(WeaponScriptableObject data)
        {
            _meshFilter ??= GetComponent<MeshFilter>();
            _meshRenderer ??= GetComponent<MeshRenderer>();
            _meshCollider ??= GetComponent<MeshCollider>();
            
            _meshCollider.sharedMesh = _meshFilter.mesh = data.MeshFilter.sharedMesh;
            _meshRenderer.material = data.Material;
            damage = data.Damage;
        }

        private void OnCollisionEnter(Collision other)
        {
            GameObject gameObject = other.gameObject;
            
            if (LayerMasker.CheckLayer(damagableLayers, gameObject.layer))
            {
                gameObject.GetComponent<EnemyViewModel>().Damage(damage);
            }
        }
    }
}