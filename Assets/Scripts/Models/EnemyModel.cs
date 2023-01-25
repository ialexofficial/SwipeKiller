using System;
using ViewModels;

namespace Models
{
    public class EnemyModel
    {
        private EnemyViewModel _viewModel;
        private int _takenDamage = 0;

        public event Action<int> OnDamage;
        public event Action OnDie;
        
        public EnemyModel(EnemyViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Damage(int damage)
        {
            _takenDamage += damage;

            OnDamage?.Invoke(_takenDamage);

            if (_takenDamage >= _viewModel.Health)
            {
                OnDie?.Invoke();
            }
        }
    }
}