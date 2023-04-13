using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ViewModels;

namespace Models
{
    public class SwipableModel
    {
        private SwipableViewModel _viewModel;
        private Vector2 _lastSwipeDelta;
        private bool _isDeltaAplied = true;

        public event Action<Vector2> OnSwipe;

        public SwipableModel(SwipableViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void FixedUpdate()
        {
            if (!_isDeltaAplied)
            {
                Vector2 delta = _lastSwipeDelta;
                _lastSwipeDelta = Vector2.zero;
                _isDeltaAplied = true;
                OnSwipe?.Invoke(delta);
            }
        }
        
        public void Swipe(PointerEventData pointerEventData, float swipeTime)
        {
            if (swipeTime < _viewModel.MinTimeInteraction)
                swipeTime = _viewModel.MinTimeInteraction;

            if (swipeTime > _viewModel.MaxTimeInteraction)
                swipeTime = _viewModel.MaxTimeInteraction;

            Vector2 swipe = pointerEventData.position - pointerEventData.pressPosition;
            Vector2 direction = ((Vector2)
                    (pointerEventData.pointerCurrentRaycast.worldPosition - _viewModel.transform.position)
                ).normalized;

            _lastSwipeDelta = direction * swipe.magnitude * _viewModel.SwipeDeadZone *
                _viewModel.SwipeStrength / swipeTime;

            if (_lastSwipeDelta.magnitude > _viewModel.MaxVelocity)
            {
                _lastSwipeDelta *= _viewModel.MaxVelocity / _lastSwipeDelta.magnitude;
            }

            _isDeltaAplied = false;
        }
    }
}