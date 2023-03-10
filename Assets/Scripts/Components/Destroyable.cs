using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(Collider))]
    public class Destroyable : MonoBehaviour, IDamagable, ICombustible
    {
        [SerializeField] private ParticleSystem destroyingEffect;
        
        private Rigidbody[] _partRigidbodies;
        private Collider _collider;
        private bool _isDestroyed = false;

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
        
        private void Start()
        {
            _collider = GetComponent<Collider>();
            _partRigidbodies = GetComponentsInChildren<Rigidbody>();
        }
    }
}