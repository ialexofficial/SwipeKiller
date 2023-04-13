using Components;
using Models;
using Models.Enums;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace ViewModels
{
    [RequireComponent(
        typeof(Collider)
    )]
    public class EnemyViewModel : MonoBehaviour, IDamagable, ICombustible
    {
        public UnityEvent OnDie = new UnityEvent();
        
        [SerializeField] private float vibrationTime = 0.2f;
        [SerializeField] private ParticleSystem headshotEffect;
        [SerializeField] private Collider headCollider;
        [SerializeField] private int health;

        private EnemyModel _model;
        private Collider[] _colliders;
        private Rigidbody[] _ragdollRigidbodies;

        public int Health => health;
        public Collider HeadCollider => headCollider;

        public void Damage(int damage, Collider part)
        {
            _model.Damage(damage, part);
        }

        public bool BurnDown()
        {
            _model.Damage(Health, null);
            gameObject.SetActive(false);
            
            return true;
        }

        private void Awake()
        {
            _model = new EnemyModel(this);

            _model.OnDamage += OnDamaged;
            _model.OnDie += OnDead;
        }
        
        private void Start()
        {
            _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            _colliders = GetComponents<Collider>();
        }

        private void OnDestroy()
        {
            _model.OnDamage -= OnDamaged;
            _model.OnDie -= OnDead;
        }

        private void OnDamaged(int takenDamage, EnemyDamagedPart part)
        {
            Vibration.Vibrate((long) (vibrationTime * 1000));

            if (part == EnemyDamagedPart.Head)
            {
                headshotEffect.Play();
            }
        }

        private void OnDead()
        {
            foreach (Collider collider in _colliders)
            {
                collider.enabled = false;
            }
            
            EnableRagdoll();
            OnDie.Invoke();
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
    }
}