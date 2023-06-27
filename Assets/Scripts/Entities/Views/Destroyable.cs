using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entities.Views
{
    public class Destroyable : MonoBehaviour, IDamagable, ICombustible
    {
        [SerializeField] private ParticleSystem destroyEffect;
        [SerializeField] private AudioSource destroySound;

        protected Collider _collider;
        private Rigidbody[] _partRigidbodies;
        private bool _isCombusted = false;

        protected void Start()
        {
            _collider = GetComponentInChildren<Collider>();
            _partRigidbodies = GetComponentsInChildren<Rigidbody>();
        }

        public virtual void Damage(int damage, Collider part)
        {
            _collider.enabled = false;
            destroyEffect.Play();
            destroySound.Play();
            
            foreach (Rigidbody partRigidbody in _partRigidbodies)
            {
                partRigidbody.isKinematic = false;
            }
        }

        public bool BurnDown()
        {
            if (_isCombusted)
                return false;
            
            gameObject.SetActive(false);
            return true;
        }
    }
}