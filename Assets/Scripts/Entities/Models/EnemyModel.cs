using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Entities.Models
{
    public class EnemyModel
    {
        private readonly int _health;
        private readonly Collider _headCollider;
        private int _takenDamage = 0;
        private bool _isDead = false;

        public event Action<int, EnemyDamagedPart> OnDamage;
        public event Action OnDie;

        public EnemyModel(
            int health, 
            Collider headCollider
        )
        {
            _health = health;
            _headCollider = headCollider;
        }

        public void Damage(int damage, [CanBeNull] Collider part)
        {
            if (_isDead)
                return;
            
            _takenDamage += damage;

            EnemyDamagedPart damagedPart = EnemyDamagedPart.Body;

            if (part && part == _headCollider)
                damagedPart = EnemyDamagedPart.Head;

            OnDamage?.Invoke(_takenDamage, damagedPart);

            if (_takenDamage >= _health)
            {
                _isDead = true;
                OnDie?.Invoke();
            }
        }
    }
}