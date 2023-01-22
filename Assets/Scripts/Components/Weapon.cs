using ScriptableObjects;
using UnityEngine;
using Utilities;
using ViewModels;

namespace Components
{
    [RequireComponent(
    typeof(Rigidbody),
    typeof(Collider),
    typeof(MeshFilter)
    )]
    [RequireComponent(typeof(MeshRenderer))]
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private LayerMask damagableLayers;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        
        public void UpdateData(WeaponScriptableObject data)
        {
            _meshFilter.mesh = data.WeaponMeshFilter.sharedMesh;
            _meshRenderer.material = data.WeaponMaterial;
            damage = data.Damage;
        }

        private void Start()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
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