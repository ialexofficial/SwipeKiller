using System;
using System.Collections.Generic;
using Entities.Views;
using Utilities;

namespace Level.Models
{
    public class LevelModel
    {
        private readonly TimeScaler _timeScaler;
        private readonly List<Enemy> _enemies;
        private readonly LevelDataProvider _levelDataProvider;

        private int _enemyCount;
        private int _swipeCount;
        private bool _isPaused = false;

        public event Action OnLevelLose;
        public event Action OnLevelWin;
        public event Action OnLevelLoad;
        public event Action<int> OnSwipeCountChanged;
        public event Action<int> OnEnemyCountChanged;

        public LevelModel(
            TimeScaler timeScaler, 
            int swipeCount, 
            List<Enemy> enemies,
            LevelDataProvider levelDataProvider
        )
        {
            _timeScaler = timeScaler;
            _swipeCount = swipeCount;
            _enemies = enemies;
            _levelDataProvider = levelDataProvider;

            foreach (var enemy in _enemies)
            {
                enemy.OnDie += OnEnemyDead;
            }
        }

        public void OnEnemyDead()
        {
            OnEnemyCountChanged?.Invoke(--_enemyCount);
            
            if (_enemyCount == 0)
            {
                _levelDataProvider.IncreaseLevel();
                OnLevelWin?.Invoke();
            }
        }

        public void OnPlayerDead()
        {
            OnLevelLose?.Invoke();
        }

        public void LoadLevel()
        {
            OnLevelLoad?.Invoke();
        }

        public void OnSwiped()
        {
            --_swipeCount;
        }
    }
}