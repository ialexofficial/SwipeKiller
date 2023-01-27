using Models;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace ViewModels
{
    [RequireComponent(
        typeof(Collider)
    )]
    public class EnemyViewModel : MonoBehaviour
    {
        public UnityEvent OnDie = new UnityEvent();
        
        [SerializeField] private float vibrationTime = 0.2f;
        [SerializeField] private EnemyScriptableObject enemyData;

        private EnemyModel _model;
        private int _health;
        private Collider _collider;
        private Rigidbody[] _ragdollRigidbodies;

        public int Health => _health;
    #if UNITY_EDITOR
        public EnemyScriptableObject EnemyData => enemyData;
    #endif

        public void Damage(int damage)
        {
            _model.Damage(damage);
        }

        private void Awake()
        {
            _model = new EnemyModel(this);

            _model.OnDamage += OnDamaged;
            _model.OnDie += OnDead;
        }
        
        private void Start()
        {
            foreach (Transform childTransform in GetComponentsInChildren<Transform>())
            {
                if (childTransform == null || childTransform.gameObject == gameObject)
                    continue;
                
                Destroy(childTransform.gameObject);
            }
            
            Instantiate(enemyData.Prefab, transform);
            _health = enemyData.Health;

            _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        private void OnDestroy()
        {
            _model.OnDamage -= OnDamaged;
            _model.OnDie -= OnDead;
        }

        private void OnDamaged(int takenDamage)
        {
            Vibration.Vibrate((long) (vibrationTime * 1000));
        }

        private void OnDead()
        {
            _collider.enabled = false;
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