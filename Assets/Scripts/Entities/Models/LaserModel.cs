using System;
using UnityEngine;

namespace Entities.Models
{
    public class LaserModel
    {
        private bool _isEnabled;
        private float _lastStateTime = 0f;

        public event Action<bool> OnSwitch;
        
        public LaserModel()
        {
        }

        public void OnUpdate(
            float workTime = 0f,
            float sleepTime = 0f
        )
        {
            if (workTime == 0f)
                return;
            
            _lastStateTime += Time.deltaTime;

            float nextStateTime = _isEnabled ? workTime : sleepTime;

            if (_lastStateTime >= nextStateTime)
            {
                _lastStateTime = 0f;
                Switch();
            }
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
    }
}