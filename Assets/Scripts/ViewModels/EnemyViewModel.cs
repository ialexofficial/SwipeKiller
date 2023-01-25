using Models;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace ViewModels
{
    [RequireComponent(
        typeof(Collider), 
        typeof(MeshRenderer),
        typeof(MeshFilter)
    )]
    public class EnemyViewModel : MonoBehaviour
    {
        public UnityEvent OnDie = new UnityEvent();
        
        [SerializeField] private float vibrationTime = 0.2f;

        private EnemyModel _model;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private int _health;

        public int Health => _health;

        public void UpdateData(EnemyScriptableObject data)
        {
            _meshRenderer ??= GetComponent<MeshRenderer>();
            _meshFilter ??= GetComponent<MeshFilter>();

            _meshFilter.mesh = data.MeshFilter.sharedMesh;
            _meshRenderer.material = data.Material;
            _health = data.Health;
        }

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
            Debug.Log("Dead");
            OnDie.Invoke();
        }
    }
}