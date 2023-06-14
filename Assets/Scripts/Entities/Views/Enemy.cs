using System;
using System.Collections.Generic;
using Entities.ViewModels;
using Ji2Core.Core;
using UnityEngine;

namespace Entities.Views
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Enemy : MonoBehaviour, IDamagable, ICombustible
    {
        [SerializeField] private ParticleSystem headshotEffect;
        [SerializeField] private Collider headCollider;
        [SerializeField] private int health;

        private EnemyVM _viewModel;
        private Rigidbody _mainRigidbody;
        private Rigidbody[] _ragdollRigidbodies;
        private Collider _mainCollider;
        private Collider[] _ragdollColliders;

        public event Action OnDie;

        public int Health => health;
        public Collider HeadCollider => headCollider;

        public void Construct(EnemyVM viewModel)
        {
            _viewModel = viewModel;

            _viewModel.OnDamage += OnDamaged;
            _viewModel.OnDie += OnDead;
        }

        private void Awake()
        {
            Context
                .GetInstance()
                .GetService<List<Enemy>>()
                .Add(this);
        }

        private void Start()
        {
            _mainRigidbody = GetComponent<Rigidbody>();
            _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            _mainCollider = GetComponent<Collider>();
            _ragdollColliders = GetComponentsInChildren<Collider>();
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

        private void OnDamaged(int takenDamage, EnemyDamagedPart part)
        {
            if (part == EnemyDamagedPart.Head)
            {
                headshotEffect.Play();
            }
        }
        
        private void EnableRagdoll()
        {
            _mainRigidbody.isKinematic = true;
            _mainCollider.enabled = false;
            
            foreach (Rigidbody rigidbody in _ragdollRigidbodies)
            {
                if (rigidbody is null)
                    continue;

                rigidbody.isKinematic = false;
            }

            foreach (Collider collider in _ragdollColliders)
            {
                if (collider is null)
                    continue;

                collider.enabled = true;
            }
        }

        private void OnDead()
        {
            _mainCollider.enabled = false;
            
            foreach (Collider collider in _ragdollColliders)
            {
                collider.enabled = false;
            }
            
            EnableRagdoll();
            OnDie?.Invoke();
        }
    }
}