﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using ViewModels;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public UnityEvent<int> OnEnemyCountChange = new UnityEvent<int>();
        public UnityEvent OnEnemyOver = new UnityEvent();
        public UnityEvent<int> OnSwipeCountChange = new UnityEvent<int>();
        public UnityEvent OnLose = new UnityEvent();
        
        [Tooltip("Enter -1 to make Infinity")]
        [SerializeField] private int swipeCount = -1;
        [SerializeField] private string nextLevelSceneName;
        [SerializeField] private string mainMenuSceneName;

        private float _savedTimeScale;
        private float _savedFixedDeltaTime;
        private int _enemyCount;
        private bool _isWin;

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(nextLevelSceneName);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }

        public void Pause()
        {
            GameManager.isGamePaused = true;

            _savedTimeScale = Time.timeScale;
            _savedFixedDeltaTime = Time.fixedDeltaTime;
        
            Time.timeScale = 0;
            Time.fixedDeltaTime = 0;
        }

        public void Confirm()
        {
            Time.timeScale = _savedTimeScale;
            Time.fixedDeltaTime = _savedFixedDeltaTime;

            GameManager.isGamePaused = false;
        }

        public void OnSwiped()
        {
            OnSwipeCountChange.Invoke(--swipeCount);

            if (swipeCount == 0)
            {
                OnLose.Invoke();
            }
        }
        
        public void OnEnemyDead()
        {
            OnEnemyCountChange.Invoke(--_enemyCount);

            if (_enemyCount == 0)
            {
                _isWin = true;
                OnEnemyOver.Invoke();
            }
        }

        public void OnWeaponDestroyed()
        {
            if (_isWin)
                return;
            
            OnLose.Invoke();
        }
        
        private void Start() 
        {
            _enemyCount = 0;
            
            foreach (EnemyViewModel enemy in FindObjectsOfType<EnemyViewModel>())
            {
                ++_enemyCount;
                enemy.OnDie.AddListener(OnEnemyDead);
            }
            
            OnSwipeCountChange.Invoke(swipeCount);
            OnEnemyCountChange.Invoke(_enemyCount);
        }
    }
}