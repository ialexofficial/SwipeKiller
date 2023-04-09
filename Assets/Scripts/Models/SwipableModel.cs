using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ViewModels;

namespace Models
{
    public class SwipableModel
    {
        private SwipableViewModel _viewModel;

        public event Action<Vector2> OnSwipe;

        public SwipableModel(SwipableViewModel viewModel)
        {
            _viewModel = viewModel;
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

            direction.x = (float) Math.Round(direction.x, 1);
            direction.y = (float) Math.Round(direction.y, 1);

            Vector2 delta = direction * swipe.magnitude * _viewModel.SwipeDeadZone *
                _viewModel.SwipeStrength / swipeTime;
            Vector2 resultVelocity = delta;

            if (Mathf.Abs(resultVelocity.x) > _viewModel.MaxVelocity)
            {
                delta.x -= Mathf.Sign(delta.x) * (Mathf.Abs(resultVelocity.x) - _viewModel.MaxVelocity);
            }
            
            if (Mathf.Abs(resultVelocity.y) > _viewModel.MaxVelocity)
            {
                delta.y -= Mathf.Sign(delta.y) * (Mathf.Abs(resultVelocity.y) - _viewModel.MaxVelocity);
            }

            OnSwipe?.Invoke(delta);
        }
    }
}