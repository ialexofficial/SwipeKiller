using System;
using UnityEngine;
using ViewModels;

namespace Models
{
    public class SlowMotionModel
    {
        private readonly SlowMotionViewModel _viewModel;
        private readonly float _defaultFixedDeltaTime;
        private float _passedTime = 0;
        private float _previousSpeed = 0;
        private bool _isSlowMotionStarted = false;
        private bool _isSlowMotionPassed = true;

        public event Action OnSlowMotionStart;
        public event Action<float, float> OnTimeScale;
        public event Action OnSlowMotionEnd;

        public SlowMotionModel(SlowMotionViewModel viewModel)
        {
            _viewModel = viewModel;
            // _defaultFixedDeltaTime = Time.fixedDeltaTime;
            _defaultFixedDeltaTime = 0.02f;
        }

        public void OnForceAdded()
        {
            _previousSpeed = 0f;
            _isSlowMotionPassed = _isSlowMotionStarted = false;
        }

        public void CheckSpeed(float currentSpeed)
        {
            float deltaSpeed = _previousSpeed - currentSpeed;
            _previousSpeed = currentSpeed;

            if (
                _isSlowMotionPassed || 
                deltaSpeed <= 0 ||
                currentSpeed > _viewModel.StartSpeed
            )
            {
                return;
            }

            _isSlowMotionPassed = _isSlowMotionStarted = true;
            _passedTime = 0;
            OnSlowMotionStart?.Invoke();
        }

        public void ScaleTime()
        {
            if (_passedTime > _viewModel.SlowingTime)
            {
                _isSlowMotionStarted = false;
            }

            if (!_isSlowMotionStarted)
            {
                OnTimeScale?.Invoke(1, _defaultFixedDeltaTime);
                OnSlowMotionEnd?.Invoke();
                return;
            }

            float timeScale = Mathf.Lerp(
                1,
                _viewModel.ScaleFactor,
                _viewModel.SlowingTimeline.Evaluate(_passedTime / _viewModel.SlowingTime)
            );
            float fixedDeltaTime = _defaultFixedDeltaTime * timeScale;
            
            _passedTime += Time.unscaledDeltaTime;
            OnTimeScale?.Invoke(timeScale, fixedDeltaTime);
        }
    }
}