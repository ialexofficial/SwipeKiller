using Components;
using Models;
using UnityEngine;
using Utilities;

namespace ViewModels
{
    [RequireComponent(typeof(Collider), typeof(MeshRenderer))]
    public class DamagableViewModel : MonoBehaviour
    {
        [SerializeField] private int health;
        [SerializeField] private float vibrationTime = 0.2f;

        private DamagableModel _model;
        private MeshRenderer _meshRenderer;
        private Debugger _debugger;

        public int Health => health;

        public void Damage(int damage)
        {
            _model.Damage(damage);
        }

        private void Awake()
        {
            _model = new DamagableModel(this);

            _model.OnDamage += Damaged;
            _model.OnDie += Dead;
        }

        private void OnDestroy()
        {
            _model.OnDamage -= Damaged;
            _model.OnDie -= Dead;
        }

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _debugger = FindObjectOfType<Debugger>();
        }

        private void Damaged(int takenDamage)
        {
            Debug.Log($"Left health: {health - takenDamage}");
            _meshRenderer.material.color = Color.red;
            _debugger?.Write($"IsAndroid: {Vibration.IsAndroid}");
            Vibration.Vibrate((long) (vibrationTime * 1000));
        }

        private void Dead()
        {
            Debug.Log("Dead");
        }
    }
}