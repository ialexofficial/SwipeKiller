using UnityEngine;

namespace Components
{
    [RequireComponent(
        typeof(Rigidbody),
        typeof(AudioSource)
    )]
    public class Explosive : MonoBehaviour, ICombustible
    {
        [SerializeField] private float strength = 1f;
        [SerializeField] private float radius = 1f;
        [SerializeField] private int damage = 1;
        [SerializeField] private float explosionSpeed = 5f;
        [SerializeField] private LayerMask explodeInteractingLayers;
        [SerializeField] private GameObject model;
        [SerializeField] private ParticleSystem particles;

        private Rigidbody _rigidbody;
        private Collider _collider;
        private AudioSource _audio;
        private bool _isExploded = false;
        private Collider[] _hitables = new Collider[64];

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

            Physics.OverlapSphereNonAlloc(
                transform.position,
                radius,
                _hitables,
                explodeInteractingLayers.value
            );

            foreach (Collider hitable in _hitables)
            {
                Rigidbody rigidbody = hitable.attachedRigidbody;

                hitable.GetComponent<IDamagable>()?.Damage(damage, hitable);

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
            if (_isExploded)
                return;

            float speed = _rigidbody.velocity.magnitude + (other.collider.attachedRigidbody?.velocity.magnitude ?? 0);

            if (speed >= explosionSpeed)
            {
                Explode();
            }
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