using System;
using JetBrains.Annotations;
using Models.Enums;
using UnityEngine;
using ViewModels;

namespace Models
{
    public class EnemyModel
    {
        private readonly EnemyViewModel _viewModel;
        private int _takenDamage = 0;
        private bool _isDead = false;

        public event Action<int, EnemyDamagedPart> OnDamage;
        public event Action OnDie;
        
        public EnemyModel(EnemyViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool Damage(int damage, [CanBeNull] Collider part)
        {
            if (_isDead)
                return false;
            
            _takenDamage += damage;

            EnemyDamagedPart damagedPart = EnemyDamagedPart.Body;

            if (part && part == _viewModel.HeadCollider)
                damagedPart = EnemyDamagedPart.Head;

            OnDamage?.Invoke(_takenDamage, damagedPart);

            if (_takenDamage >= _viewModel.Health)
            {
                _isDead = true;
                OnDie?.Invoke();
            }

            return true;
        }
    }
}