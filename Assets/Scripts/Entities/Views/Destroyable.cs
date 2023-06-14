using UnityEngine;

namespace Entities.Views
{
    public class Destroyable : MonoBehaviour, IDamagable, ICombustible
    {
        [SerializeField] private ParticleSystem destroyingEffect;
        
        private Rigidbody[] _partRigidbodies;
        private Collider _collider;
        private bool _isDestroyed = false;

        private void Start()
        {
            _collider = GetComponentInChildren<Collider>();
            _partRigidbodies = GetComponentsInChildren<Rigidbody>();
        }

        public void Damage(int damage, Collider part)
        {
            _collider.enabled = false;
            destroyingEffect.Play();
            
            foreach (Rigidbody partRigidbody in _partRigidbodies)
            {
                partRigidbody.isKinematic = false;
            }
        }

        public bool BurnDown()
        {
            if (_isDestroyed)
                return false;
            
            gameObject.SetActive(false);
            return true;
        }
    }
}