using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.Views
{
    [RequireComponent(
        typeof(Rigidbody),
        typeof(AudioSource)
    )]
    public class Explosive : MonoBehaviour, ICombustible, IDamagable
    {
        [SerializeField] private float strength = 1f;
        [SerializeField] private float radius = 1f;
        [SerializeField] private int damage = 1;
        [SerializeField] private float explosionSpeed = 5f;
        [SerializeField] private LayerMask explodeInteractingLayers;
        [SerializeField] private GameObject model;
        [SerializeField] private ParticleSystem explosionParticles;
        [SerializeField] private AudioSource explosionSound;

        private Rigidbody _mainRigidbody;
        private Rigidbody[] _fragmentsRigidbodies;
        private Collider _mainCollider;
        private bool _isExploded = false;
        private Collider[] _hitables = new Collider[64];
        private Collider[] _fragmentsColliders;

        private void Start()
        {
            _mainRigidbody = GetComponent<Rigidbody>();
            _mainCollider = GetComponent<Collider>();
            _fragmentsRigidbodies = model.GetComponentsInChildren<Rigidbody>();
            _fragmentsColliders = model.GetComponentsInChildren<Collider>();
        }

        public void Damage(int damage, Collider part = null)
        {
            if (_isExploded)
                return;
            
            Explode();
        }

        public bool BurnDown()
        {
            if (_isExploded)
                return false;
            
            Explode();
            return true;
        }

        public void Explode()
        {
            _isExploded = true;

            int length = Physics.OverlapSphereNonAlloc(
                transform.position,
                radius,
                _hitables,
                explodeInteractingLayers.value
            );

            foreach (Collider hitable in _hitables.Take(length))
            {
                if (hitable.gameObject == gameObject)
                    continue;
                
                Rigidbody rigidbody = hitable.attachedRigidbody;

                hitable.GetComponent<IDamagable>()?.Damage(damage, hitable);

                if (rigidbody != null)
                {
                    rigidbody.AddExplosionForce(strength, transform.position, radius);
                }
            }

            _mainCollider.enabled = false;
            _mainRigidbody.isKinematic = true;
            foreach (var rigidbody in _fragmentsRigidbodies)
            {
                rigidbody.isKinematic = false;
            }
            foreach (var collider in _fragmentsColliders)
            {
                collider.enabled = true;
            }
            
            explosionParticles.Play();
            explosionSound.Play();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_isExploded)
                return;

            float speed = (_mainRigidbody.velocity +
                           (other.collider.attachedRigidbody?.velocity ?? Vector3.zero)
                ).magnitude;

            if (speed >= explosionSpeed)
            {
                Explode();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0, 0, .3f);
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
}