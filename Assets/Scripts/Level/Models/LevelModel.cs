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
        private bool _isLevelEnd = false;

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

            _enemyCount = _enemies.Count;
            
            foreach (var enemy in _enemies)
            {
                enemy.OnDie += OnEnemyDead;
            }
        }

        public void OnEnemyDead()
        {
            --_enemyCount;
            OnEnemyCountChanged?.Invoke(_enemyCount);

            if (_isLevelEnd)
                return;
            
            if (_enemyCount == 0)
            {
                _isLevelEnd = true;
                _levelDataProvider.IncreaseLevel();
                OnLevelWin?.Invoke();
            }
        }

        public void OnPlayerDead()
        {
            if (_isLevelEnd)
                return;

            _isLevelEnd = true;
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