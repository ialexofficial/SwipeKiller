using System;
using Entities.ViewModels;
using UnityEngine;

namespace Entities.Views
{
    [RequireComponent(typeof(Collider))]
    public class Enemy : MonoBehaviour, IDamagable, ICombustible
    {
        [SerializeField] private ParticleSystem headshotEffect;
        [SerializeField] private Collider headCollider;
        [SerializeField] private int health;

        private EnemyVM _viewModel;
        private Collider[] _colliders;
        private Rigidbody[] _ragdollRigidbodies;

        public event Action OnDie;

        public int Health => health;
        public Collider HeadCollider => headCollider;

        public void Construct(EnemyVM viewModel)
        {
            _viewModel = viewModel;

            _viewModel.OnDamage += OnDamaged;
            _viewModel.OnDie += OnDead;
        }
        
        public void Damage(int damage, Collider part)
        {
            _viewModel.Damage(damage, part);
        }

        public bool BurnDown()
        {
            _viewModel.Damage(health, null);
            gameObject.SetActive(false);
            
            return true;
        }
        
        private void Start()
        {
            _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            _colliders = GetComponents<Collider>();
        }
        
        private void OnDamaged(int takenDamage, EnemyDamagedPart part)
        {
            if (part == EnemyDamagedPart.Head)
            {
                headshotEffect.Play();
            }
        }
        
        private void EnableRagdoll()
        {
            foreach (Rigidbody rigidbody in _ragdollRigidbodies)
            {
                if (rigidbody == null)
                    continue;

                rigidbody.isKinematic = false;
            }
        }

        private void OnDead()
        {
            foreach (Collider collider in _colliders)
            {
                collider.enabled = false;
            }
            
            EnableRagdoll();
            OnDie?.Invoke();
        }
    }
}