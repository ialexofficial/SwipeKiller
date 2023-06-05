using UnityEngine;
using Utilities;


namespace Entities.Models
{
    public class SlowMotionModel
    {
        private readonly SlowMotionSettings _settings;
        private readonly TimeScaler _timeScaler;
        private float _passedTime = 0;
        private float _previousSpeed = 0;
        private SlowMotionState _slowMotionState = SlowMotionState.Passed;

        public SlowMotionModel(
            SlowMotionSettings settings,
            TimeScaler timeScaler,
            SwipableModel swipableModel
        )
        {
            _settings = settings;
            _timeScaler = timeScaler;

            swipableModel.OnForceAdd += OnForceAdded;
        }

        public void OnUpdate(float currentSpeed)
        {
            if (_slowMotionState is SlowMotionState.Started)
                ScaleTime();

            float deltaSpeed = _previousSpeed - currentSpeed;
            _previousSpeed = currentSpeed;

            if (
                _slowMotionState is SlowMotionState.Passed ||
                deltaSpeed <= 0 ||
                currentSpeed > _settings.StartSpeed
            )
            {
                return;
            }

            _slowMotionState = SlowMotionState.Started;
            _passedTime = 0;
        }

        private void ScaleTime()
        {
            if (_passedTime > _settings.Duration)
            {
                Debug.Log("End");
                _slowMotionState = SlowMotionState.Passed;
                _timeScaler.ScaleTime(1f);
                return;
            }

            float timeScale = Mathf.Lerp(
                1,
                _settings.ScaleFactor,
                _settings.Timeline.Evaluate(_passedTime / _settings.Duration)
            );

            _passedTime += Time.unscaledDeltaTime;
            _timeScaler.ScaleTime(timeScale);
        }

        private void OnForceAdded(Vector2 delta)
        {
            _previousSpeed = 0f;
            _slowMotionState = SlowMotionState.None;
            _timeScaler.ScaleTime(1f);
        }
    }

    public enum SlowMotionState
    {
        None,
        Started,
        Passed
    }
}