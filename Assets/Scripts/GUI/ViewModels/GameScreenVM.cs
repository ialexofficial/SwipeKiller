using System;
using GUI.Models;
using Ji2Core.Core;
using Level.Models;

namespace GUI.ViewModels
{
    public class GameScreenVM : IBootstrapable
    {
        private readonly GameScreenModel _screenModel;
        private readonly LevelModel _levelModel;
        private readonly MoneyDataModel _moneyDataModel;

        public event Action<int> OnEnemyCountChange;
        public event Action<int> OnSwipeCountChange;
        public event Action<bool> OnToggleSettings;
        public event Action<bool> OnSoundToggle;
        public event Action<bool> OnVibrationToggle;
        public event Action OnMoneyAmountChange;
        public event Action OnWin;
        public event Action OnLose;

        public int MoneyAmount => _moneyDataModel.MoneyAmount;
        public int SwipeCount => _screenModel.SwipeCount;

        public GameScreenVM(
            GameScreenModel screenModel,
            LevelModel levelModel,
            MoneyDataModel moneyDataModel
            )
        {
            _screenModel = screenModel;
            _levelModel = levelModel;
            _moneyDataModel = moneyDataModel;

            _screenModel.OnEnemyCountChange += OnEnemyCountChanged;
            _screenModel.OnSwipeCountChange += OnSwipeCountChanged;
            _screenModel.OnWin += OnWon;
            _screenModel.OnLose += OnLost;
            _screenModel.OnToggleSettings += isActive => OnToggleSettings?.Invoke(isActive);
            _screenModel.OnToggleSound += isActive => OnSoundToggle?.Invoke(isActive);
            _screenModel.OnToggleVibration += isActive => OnVibrationToggle?.Invoke(isActive);
        }

        public void Bootstrap()
        {
            OnEnemyCountChange?.Invoke(_screenModel.EnemyCount);
            OnSwipeCountChange?.Invoke(_screenModel.SwipeCount);
        }

        public void EarnMoney()
        {
            OnMoneyAmountChange?.Invoke();
        }

        public void ClickSettingsButton()
        {
            _screenModel.ToggleSettings();
        }

        public void ClickCloseSettingsButton()
        {
            _screenModel.ToggleSettings(false);
        }

        public void ClickReloadButton()
        {
            _levelModel.LoadLevel();
        }

        public void ClickNextLevelButton()
        {
            _levelModel.LoadLevel();
        }

        public void ClickMainMenuButton()
        {
        }

        public void ClickVibrationButton()
        {
            _screenModel.ToggleVibration();
        }

        public void ClickVolumeButton()
        {
            _screenModel.ToggleSound();
        }

        private void OnEnemyCountChanged(int enemyCount)
        {
            OnEnemyCountChange?.Invoke(enemyCount);
        }

        private void OnSwipeCountChanged(int swipeCount)
        {
            OnSwipeCountChange?.Invoke(swipeCount);
        }

        private void OnWon()
        {
            OnWin?.Invoke();
        }

        private void OnLost()
        {
            OnLose?.Invoke();
        }
    }
}