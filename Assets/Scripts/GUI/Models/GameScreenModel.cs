using System;
using System.Collections.Generic;
using Entities.Views;
using Ji2.CommonCore.SaveDataContainer;
using SwipeKiller;
using Utilities;

namespace GUI.Models
{
    public class GameScreenModel
    {
        private readonly TimeScaler _timeScaler;
        private readonly ISaveDataContainer _saveDataContainer;
        private GameScreenState _state = GameScreenState.Game;
        private GameScreenState _previousState = GameScreenState.Game;
        private bool _isVibrationEnabled;
        private bool _isSoundEnabled;
        
        public event Action<int> OnEnemyCountChange;
        public event Action<int> OnSwipeCountChange;
        public event Action<bool> OnToggleSettings;
        public event Action<bool> OnToggleVibration;
        public event Action<bool> OnToggleSound;
        public event Action OnWin;
        public event Action OnLose;

        public int SwipeCount { get; private set; }
        public int EnemyCount { get; private set; }

        private GameScreenState PreviousState
        {
            get => _previousState;
            set
            {
                _previousState = value;

                switch (_previousState)
                {
                    case GameScreenState.Settings:
                        _timeScaler.Confirm();
                        OnToggleSettings?.Invoke(false);
                        break;
                }
            }
        }

        private GameScreenState State
        {
            get => _state;
            set
            {
                PreviousState = _state;
                _state = value;

                switch (_state)
                {
                    case GameScreenState.Settings:
                        _timeScaler.Pause();
                        OnToggleSound?.Invoke(_isSoundEnabled);
                        OnToggleVibration?.Invoke(_isVibrationEnabled);
                        OnToggleSettings?.Invoke(true);
                        break;
                    case GameScreenState.Win:
                        SaveData();
                        OnWin?.Invoke();
                        break;
                    case GameScreenState.Lose:
                        SaveData();
                        OnLose?.Invoke();
                        break;
                }
            }
        }

        public GameScreenModel(
            List<Enemy> enemies,
            int swipeCount,
            TimeScaler timeScaler,
            ISaveDataContainer saveDataContainer
        )
        {
            SwipeCount = swipeCount;
            EnemyCount = enemies.Count;
            _timeScaler = timeScaler;
            _saveDataContainer = saveDataContainer;

            _isSoundEnabled = _saveDataContainer.GetValue(Constants.SOUND_SAVE_DATA_KEY, true);
            _isVibrationEnabled = _saveDataContainer.GetValue(Constants.VIBRATION_SAVE_DATA_KEY, true);

            foreach (var enemy in enemies)
            {
                enemy.OnDie += DecreaseEnemyCount;
            }
        }

        public void ToggleSettings()
        {
            ToggleSettings(!(State is GameScreenState.Settings));
        }

        public void ToggleSettings(bool isOpened)
        {
            State = isOpened ? GameScreenState.Settings : PreviousState;
        }

        public void Win()
        {
            State = GameScreenState.Win;
        }

        public void Lose()
        {
            State = GameScreenState.Lose;
        }

        public void ToggleSound()
        {
            _isSoundEnabled = !_isSoundEnabled;
            OnToggleSound?.Invoke(_isSoundEnabled);
        }

        public void ToggleVibration()
        {
            _isVibrationEnabled = !_isVibrationEnabled;
            OnToggleVibration?.Invoke(_isVibrationEnabled);
        }

        public void SaveData()
        {
            _saveDataContainer.SaveValue(Constants.SOUND_SAVE_DATA_KEY, _isSoundEnabled);
            _saveDataContainer.SaveValue(Constants.VIBRATION_SAVE_DATA_KEY, _isVibrationEnabled);
        }

        private void DecreaseEnemyCount()
        {
            OnEnemyCountChange?.Invoke(--EnemyCount);
        }
    }

    public enum GameScreenState
    {
        Game,
        Settings,
        Win,
        Lose
    }
}