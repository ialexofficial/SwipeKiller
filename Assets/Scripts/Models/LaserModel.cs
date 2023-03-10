using System;
using ViewModels;

namespace Models
{
    public class LaserModel
    {
        private LaserViewModel _viewModel;
        private bool _isEnabled;
        private float _lastStateTime = 0f;

        public event Action<bool> OnSwitch;
        
        public LaserModel(LaserViewModel viewModel)
        {
            _viewModel = viewModel;
            _isEnabled = _viewModel.IsEnabled;
        }

        public void Switch()
        {
            Switch(!_isEnabled);
        }

        public void Switch(bool isEnabled)
        {
            _isEnabled = isEnabled;
            
            OnSwitch?.Invoke(_isEnabled);
        }

        public void TickOnTime(float deltaTime)
        {
            if (!_viewModel.IsControlledByTime)
                return;
            
            _lastStateTime += deltaTime;

            float nextStateTime = _isEnabled ? _viewModel.WorkTime : _viewModel.SleepTime;

            if (_lastStateTime >= nextStateTime)
            {
                _lastStateTime = 0;
                Switch();
            }
        }
    }
}