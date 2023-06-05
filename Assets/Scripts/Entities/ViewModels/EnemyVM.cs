using System;
using Entities.Models;
using UnityEngine;

namespace Entities.ViewModels
{
    public class EnemyVM
    {
        private readonly EnemyModel _model;

        public event Action<int, EnemyDamagedPart> OnDamage;
        public event Action OnDie;

        public EnemyVM(EnemyModel model)
        {
            _model = model;

            _model.OnDamage += (damage, part) => OnDamage?.Invoke(damage, part);
            _model.OnDie += () => OnDie?.Invoke();
        }
        
        public void Damage(int damage, Collider part)
        {
            _model.Damage(damage, part);
        }
    }
}