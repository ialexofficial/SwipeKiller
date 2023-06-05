using System;
using Ji2.CommonCore;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Entities.Models
{
    public class SwipableModel : IFixedUpdatable
    {
        private readonly SwipeSettings _settings;
        private readonly UpdateService _updateService;
        private readonly Camera _camera;
        private Vector2 _lastSwipeDelta;
        private bool _isDeltaApplied = true;

        public event Action<Vector2> OnForceAdd;

        public SwipableModel(
            SwipeSettings settings,
            UpdateService updateService
        )
        {
            _settings = settings;
            _updateService = updateService;
            _camera = Camera.main;

            _updateService.Add(this);
        }

        public void OnFixedUpdate()
        {
            if (!_isDeltaApplied)
            {
                Vector2 delta = _lastSwipeDelta;
                _lastSwipeDelta = Vector2.zero;
                _isDeltaApplied = true;
                OnForceAdd?.Invoke(delta);
            }
        }

        public void Clear()
        {
            _updateService.Remove(this);
        }
        
        public void Swipe(
            PointerEventData pointerEventData, 
            float swipeTime,
            Vector3 position
        )
        {
            Vector2 screenPosition = _camera.WorldToScreenPoint(position);
            
            if (swipeTime < _settings.MinTimeInteraction)
                swipeTime = _settings.MinTimeInteraction;

            if (swipeTime > _settings.MaxTimeInteraction)
                swipeTime = _settings.MaxTimeInteraction;
            
            Vector2 swipe = pointerEventData.position - pointerEventData.pressPosition;
            Vector2 direction = (
                pointerEventData.position - screenPosition
            ).normalized;

            _lastSwipeDelta = direction * swipe.magnitude * _settings.DeadZone *
                _settings.Strength / swipeTime;

            if (_lastSwipeDelta.magnitude > _settings.MaxVelocity)
            {
                _lastSwipeDelta *= _settings.MaxVelocity / _lastSwipeDelta.magnitude;
            }

            _isDeltaApplied = false;
        }
    }
}