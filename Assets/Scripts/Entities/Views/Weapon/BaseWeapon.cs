using System;
using Entities.ViewModels;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;

namespace Entities.Views.Weapon
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseWeapon : MonoBehaviour, ICombustible
    {
        [SerializeField] protected int damage = 1;
        [SerializeField] protected Collider bladeCollider;
        [SerializeField] protected Vector3 centerOfMass;
        [SerializeField] protected LayerMask damagableLayers;
        
        [Header("FX")]
        [SerializeField] protected ParticleSystem speedEffect;
        [SerializeField] protected ParticleSystem destroyEffect;
        [SerializeField] protected float speedEffectVelocity = 1f;

        protected WeaponVM _viewModel;
        protected Rigidbody _rigidbody;

        public event Action OnWeaponDestroy;
        public event Action OnForceAdd;

        public void Construct(WeaponVM viewModel)
        {
            _viewModel = viewModel;

            _viewModel.OnForceAdd += OnForceAdded;
        }
        
        public bool BurnDown()
        {
            if (!gameObject.activeSelf)
                return false;
            
            gameObject.SetActive(false);
            // destroyEffect.transform.parent = null;
            // destroyEffect.Play();
            OnWeaponDestroy?.Invoke();
            return true;
        }

        public void Swipe(PointerEventData pointerEventData, float swipeTime)
        {
            _viewModel.Swipe(pointerEventData, swipeTime, transform.position);
        }

        protected void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _rigidbody.centerOfMass = centerOfMass;
        }

        protected void Update()
        {
            _viewModel.OnUpdate(_rigidbody.velocity.magnitude);
        }

        protected virtual void OnForceAdded(Vector2 delta)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(delta, ForceMode.VelocityChange);
            
            if (delta.magnitude >= speedEffectVelocity)
            {
                speedEffect.Play();
            }
            
            OnForceAdd?.Invoke();
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

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 1f, 0, .3f);
            Gizmos.DrawSphere(centerOfMass, .1f);
        }
    }
}