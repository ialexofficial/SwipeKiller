using UnityEngine;
using Utilities;
using ViewModels;

namespace Components
{
    [RequireComponent(
        typeof(Rigidbody),
        typeof(Collider),
        typeof(AudioSource)
    )]
    public class Explosive : MonoBehaviour
    {
        [SerializeField] private float strength = 1f;
        [SerializeField] private float radius = 1f;
        [SerializeField] private int damage = 1;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private LayerMask destroyableLayer;
        [SerializeField] private LayerMask weaponLayer;
        [SerializeField] private GameObject model;
        [SerializeField] private ParticleSystem particles;

        private Rigidbody _rigidbody;
        private Collider _collider;
        private AudioSource _audio;
        private bool _isExploded = false;
        
        public void Explode()
        {
            _isExploded = true;
            
            Collider[] hitables = Physics.OverlapSphere(
                transform.position,
                radius,
                LayerMasker.MergeLayerMasks(enemyLayer, destroyableLayer, weaponLayer).value
            );

            foreach (Collider hitable in hitables)
            {
                Rigidbody rigidbody = hitable.attachedRigidbody;
                
                if (LayerMasker.CheckLayer(enemyLayer, hitable.gameObject.layer))
                {
                    hitable.GetComponent<EnemyViewModel>()?.Damage(damage);
                }
                else if(LayerMasker.CheckLayer(destroyableLayer, hitable.gameObject.layer))
                {
                    hitable.GetComponent<Destroyable>()?.BreakDown();
                }

                if (rigidbody != null)
                {
                    rigidbody.AddExplosionForce(strength, transform.position, radius);
                }
            }
            
            model.SetActive(false);
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            particles.Play();
            _audio.Play();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_isExploded || !LayerMasker.CheckLayer(weaponLayer, other.gameObject.layer))
                return;
            
            Explode();
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _audio = GetComponent<AudioSource>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0, 0, .3f);
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
}