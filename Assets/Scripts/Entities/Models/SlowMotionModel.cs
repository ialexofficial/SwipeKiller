using UnityEngine;
using Utilities;


namespace Entities.Models
{
    public class SlowMotionModel
    {
        private readonly SlowMotionSettings _settings;
        private readonly TimeScaler _timeScaler;
        private float _passedTime = 0;
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

            if (
                _slowMotionState is SlowMotionState.Passed or SlowMotionState.Started ||
                currentSpeed > _settings.StartSpeed
            )
            {
                return;
            }

            if (_slowMotionState is SlowMotionState.Ready)
            {
                _slowMotionState = SlowMotionState.Started;
                _passedTime = 0;
            }
            else
            {
                _slowMotionState = SlowMotionState.Ready;
            }
        }

        private void ScaleTime()
        {
            if (_passedTime > _settings.Duration)
            {
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
            _slowMotionState = SlowMotionState.None;
            _timeScaler.ScaleTime(1f);
        }
    }

    public enum SlowMotionState
    {
        None,
        Ready,
        Started,
        Passed
    }
}