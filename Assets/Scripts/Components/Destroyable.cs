using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(Collider))]
    public class Destroyable : MonoBehaviour
    {
        private Rigidbody[] _partRigidbodies;
        private Collider _collider;

        public void BreakDown()
        {
            _collider.enabled = false;
            
            foreach (Rigidbody part in _partRigidbodies)
            {
                part.isKinematic = false;
            }
        }
        
        private void Start()
        {
            _collider = GetComponent<Collider>();
            _partRigidbodies = GetComponentsInChildren<Rigidbody>();
        }
    }
}