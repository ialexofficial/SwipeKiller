using UnityEngine;
using UnityEngine.Events;
using Utilities;
using ViewModels;

namespace Components
{
    [RequireComponent(
    typeof(Rigidbody),
    typeof(SwipableViewModel)
    )]
    public abstract class Weapon : MonoBehaviour, ICombustible
    {
        public UnityEvent OnWeaponDestroy = new UnityEvent();
        [SerializeField] protected int damage = 1;
        [SerializeField] protected Collider bladeCollider;
        [SerializeField] protected Vector3 centerOfMass;
        [SerializeField] protected LayerMask damagableLayers;
        [SerializeField] protected ParticleSystem speedEffect;
        [SerializeField] protected ParticleSystem destroyEffect;
        [SerializeField] protected float speedEffectVelocity = 1f;

        protected SwipableViewModel _swipableViewModel;
        protected Rigidbody _rigidbody;

        public bool BurnDown()
        {
            if (!gameObject.activeSelf)
                return false;
            
            gameObject.SetActive(false);
            destroyEffect.transform.parent = null;
            destroyEffect.Play();
            OnWeaponDestroy.Invoke();
            return true;
        }

        protected virtual void OnForceAdded(Vector2 delta)
        {
            if (delta.magnitude >= speedEffectVelocity)
            {
                speedEffect.Play();
            }
        }

        protected void OnCollisionEnter(Collision collision)
        {
            ContactPoint contactPoint = collision.GetContact(0);

            if (contactPoint.thisCollider != bladeCollider)
                return;

            GameObject other = collision.gameObject;

            if (LayerMasker.CheckLayer(damagableLayers, other.layer))
            {
                other.GetComponentInParent<IDamagable>().Damage(damage, collision.collider);
            }
        }

        protected void Start()
        {
            _swipableViewModel = GetComponent<SwipableViewModel>();
            _rigidbody = GetComponent<Rigidbody>();

            _rigidbody.centerOfMass = centerOfMass;
            _swipableViewModel.OnForceAdd.AddListener(OnForceAdded);
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 1f, 0, .3f);
            Gizmos.DrawSphere(centerOfMass, .1f);
        }
    }
}